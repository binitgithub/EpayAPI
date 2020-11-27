using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace E_Pay_Web_API.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? LastLogin { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<E_Pay_Web_API.Models.FinancialInstitution> FinancialInstitutions { get; set; }

        public System.Data.Entity.DbSet<E_Pay_Web_API.Models.AssociatedAccount> AssociatedAccounts { get; set; }

        public System.Data.Entity.DbSet<E_Pay_Web_API.Models.COBSecurityReview> COBSecurityReviews { get; set; }

        public System.Data.Entity.DbSet<E_Pay_Web_API.Models.AssociatedAccountCOBSecurityReview> AssociatedAccountsCOBSecurityReviews { get; set; }

        public System.Data.Entity.DbSet<E_Pay_Web_API.Models.COBSecurityResponse> COBSecurityResponses { get; set; }

        public System.Data.Entity.DbSet<E_Pay_Web_API.Models.Transfer> Transfers { get; set; }

        public System.Data.Entity.DbSet<E_Pay_Web_API.Models.PIN> PINs { get; set; }

        public System.Data.Entity.DbSet<E_Pay_Web_API.Models.COBSecurityQuestion> COBSecurityQuestions { get; set; }

        public System.Data.Entity.DbSet<E_Pay_Web_API.Models.ResetPasswordRequest> ResetPasswordRequests { get; set; }

        public System.Data.Entity.DbSet<E_Pay_Web_API.Models.FinancialInstitutionAdminEmail> FinancialInstitutionAdminEmails { get; set; }

        public System.Data.Entity.DbSet<E_Pay_Web_API.Models.Biller> Billers { get; set; }

        public System.Data.Entity.DbSet<E_Pay_Web_API.Models.ServiceType> ServiceTypes { get; set; }

        public System.Data.Entity.DbSet<E_Pay_Web_API.Models.BillerService> BillerServices { get; set; }

        public System.Data.Entity.DbSet<E_Pay_Web_API.Models.BillerFee> BillerFees { get; set; }

        public System.Data.Entity.DbSet<E_Pay_Web_API.Models.AppliedFee> AppliedFees { get; set; }
    }
}