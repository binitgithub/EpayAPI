using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using E_Pay_Web_API.Models;

namespace E_Pay_Web_API.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            /*
            ApplicationUser user = await userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }
            */

            //###############################################################################
            var user = await userManager.FindByNameAsync(context.UserName);

            if (user == null)
            {
                context.SetError("invalid_grant", "Wrong username or password."); //user not found
                return;
            }

            if (await userManager.IsLockedOutAsync(user.Id))
            {
                context.SetError("locked_out", "User is locked out");
                return;
            }

            var check = await userManager.CheckPasswordAsync(user, context.Password);

            if (!check)
            {
                await userManager.AccessFailedAsync(user.Id);
                if (await userManager.IsLockedOutAsync(user.Id))
                {
                    SendAdminAlert(user);
                }
                context.SetError("invalid_grant", "Wrong username or password."); //wrong password
                return;
            }

            await userManager.ResetAccessFailedCountAsync(user.Id);
            //###############################################################################

            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
               OAuthDefaults.AuthenticationType);
            ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager,
                CookieAuthenticationDefaults.AuthenticationType);

            AuthenticationProperties properties = CreateProperties(user.UserName);
            List<string> roles = oAuthIdentity.Claims.Where(p => p.Type == ClaimTypes.Role).Select(p=>p.Value).ToList();
            properties.Dictionary.Add("roles",roles.Count>0?roles.Aggregate((current, next)=>current+","+next):"");
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            //UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            ApplicationUserManager manager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            ApplicationUser user = manager.FindByEmail(userName);
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName },
                { "fullName", user.FirstName+" "+user.LastName },
                { "lastLogin", user.LastLogin.ToString() }
            };
            user.LastLogin = DateTime.Now;
            var result = manager.Update(user);
            if (result.Errors.Count() > 0)
            {
                List<string> errorList = result.Errors.ToList();
                for(int i =0;i< errorList.Count();i++)
                {
                    data.Add("Error_" + i.ToString(), errorList[i]);
                }
            }
            return new AuthenticationProperties(data);
        }

        public void SendAdminAlert(ApplicationUser currentUser)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var financialInstitutionEmails = (from p in db.AssociatedAccounts
                                             from q in db.FinancialInstitutionAdminEmails
                                             where p.UserID == currentUser.Id
                                             && p.FinancialInstitutionID == q.FinancialInstitutionID
                                             select q.EmailAddress).ToArray();

            string msgBody = "The user below has reached the maximum failed login attempts and has been locked out.\r\n\r\n";
            msgBody += "Full Name: "+currentUser.FirstName+" "+currentUser.LastName + "\r\n";
            msgBody += "Email Address: " + currentUser.Email;

            Helpers.MailSender.SendMessage(financialInstitutionEmails, "E-Pay - Max Failed Login Attempts ["+currentUser.Email+"]", msgBody);
        }
    }
}