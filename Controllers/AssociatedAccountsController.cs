using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Xml.Linq;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using E_Pay_Web_API.Models;
using E_Pay_Web_API.MiddleWare;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using E_Pay_Web_API.Helpers;

namespace E_Pay_Web_API.Controllers
{
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AssociatedAccountsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private const string fiIdString = "8DB34F14-A886-4BDB-AC85-2351CDD0F715";
        private string[] COBExcludedAcctTypes = new string[] { "Bill Payment", "EPAY BILL PAYMENTS", "EPAY DIGICEL TOPUP" };
        private string[] COBSourceAccounts = new string[] { "Shares", "Deposits" };

        // GET: api/AssociatedAccounts
        public IQueryable<AssociatedAccount_Light> GetAssociatedAccounts()
        {
            string userId = RequestContext.Principal.Identity.GetUserId();
            return (from p in db.AssociatedAccounts
                   from q in db.FinancialInstitutions
                   where p.UserID == userId
                   && p.FinancialInstitutionID == q.FinancialInstitutionID
                   && !COBExcludedAcctTypes.Contains(p.Description2)
                   && p.Status.Equals("Active")
                    select new AssociatedAccount_Light
                   {
                       AssociatedAccountID = p.AssociatedAccountID,
                       FinancialInstitutionID = q.FinancialInstitutionID,
                       FinancialInstitutionName = q.Name,
                       AccountType = p.AccountType,
                       Description=p.Description,
                       Description2=p.Description2,
                       Description3=p.Description3,
                       AccountNumber = p.AccountNumber,
                       DefaultAccount = p.DefaultAccount,
                       Verified = p.Verified != null
                   }).OrderBy(p=>p.AccountType);
        }

        [Route("api/SourceAccounts")]
        public IQueryable<AssociatedAccount_Light> GetSourceAccounts()
        {
            string userId = RequestContext.Principal.Identity.GetUserId();
            return (from p in db.AssociatedAccounts
                    from q in db.FinancialInstitutions
                    where p.UserID == userId
                    && p.FinancialInstitutionID == q.FinancialInstitutionID
                    && COBSourceAccounts.Contains(p.Description2)
                    && p.Status.Equals("Active")
                    select new AssociatedAccount_Light
                    {
                        AssociatedAccountID = p.AssociatedAccountID,
                        FinancialInstitutionID = q.FinancialInstitutionID,
                        FinancialInstitutionName = q.Name,
                        AccountType = p.AccountType,
                        Description = p.Description,
                        Description2 = p.Description2,
                        Description3 = p.Description3,
                        AccountNumber = p.AccountNumber,
                        DefaultAccount = p.DefaultAccount,
                        Verified = p.Verified != null
                    }).OrderBy(p => p.AccountType);
        }

        public IQueryable<AssociatedAccount> GetAssociatedAccounts(string userId)
        {
            return db.AssociatedAccounts.Where(p => p.UserID == userId && p.Status.Equals("Active")).OrderBy(p => p.Description);
        }

        public IQueryable<AssociatedAccount> GetAssociatedAccounts(Guid institutionId, string userId)
        {
            return db.AssociatedAccounts.Where(p => p.UserID == userId && p.Status.Equals("Active") && p.FinancialInstitutionID == institutionId && p.Verified != null).OrderBy(p => p.Description);
        }

        [Route("AccountBalance")]
        public async Task<AccountBalance> GetAccountBalance(string acctnum)
        {
            string userId = RequestContext.Principal.Identity.GetUserId();
            Guid fiId = Guid.Parse(fiIdString);
            string encryptedPin = db.PINs.Where(p => p.FinancialInstitutionID == fiId && p.UserID == userId).Select(p => p.Pin).First();
            string decryptedPin = Helpers.StringCipher.Decrypt(encryptedPin, userId);
            string[] balance = await COB.GetAccountBalance(acctnum, decryptedPin);
            if(balance.Length>1)
            return new AccountBalance { AccountNumber = acctnum, CurrentBalance = balance[0],AvailableBalance = balance[1] };

            return new AccountBalance { AccountNumber = acctnum, CurrentBalance = "Error",AvailableBalance = "Error" };
        }

        [Route("AccountBalanceArray")]
        public async Task<AccountBalance[]> GetAccountBalanceArray(string acctnums)
        {
            List<AccountBalance> balanceList = new List<AccountBalance>();
            string[] acctnumArray = acctnums.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            string userId = RequestContext.Principal.Identity.GetUserId();
            Guid fiId = Guid.Parse(fiIdString);
            string encryptedPin = db.PINs.Where(p => p.FinancialInstitutionID == fiId && p.UserID == userId).Select(p => p.Pin).First();
            string decryptedPin = Helpers.StringCipher.Decrypt(encryptedPin, userId);
            foreach (string acctnum in acctnumArray)
            {
                string[] balance = await COB.GetAccountBalance(acctnum, decryptedPin);
                if(balance.Length > 1)
                {
                    balanceList.Add(new AccountBalance { AccountNumber = acctnum, CurrentBalance = balance[0], AvailableBalance = balance[1] });
                }
                else
                {
                    balanceList.Add(new AccountBalance { AccountNumber = acctnum, CurrentBalance = "Error", AvailableBalance = "Error" });
                }
            }
            return balanceList.ToArray();
        }

        [Route("AccountHistory")]
        public async Task<IHttpActionResult> GetAccountHistory(int acctid, int days)
        {
            DateTime fromDate = DateTime.Now.AddDays(days*(-1));
            string fromDateString = fromDate.ToString("MM-dd-yyyy");
            string userId = RequestContext.Principal.Identity.GetUserId();
            Guid fiId = Guid.Parse(fiIdString);
            try
            {
                string encryptedPin = db.PINs.Where(p => p.FinancialInstitutionID == fiId && p.UserID == userId).Select(p => p.Pin).First();
                string decryptedPin = Helpers.StringCipher.Decrypt(encryptedPin, userId);
                var acct = db.AssociatedAccounts.Where(p => p.UserID == userId && p.AssociatedAccountID == acctid).First();
                string[] rows = await COB.GetAccountHistory(acct.AccountNumber, acct.MemberID, decryptedPin, fromDateString);
                string xml = string.Empty;
                foreach (string row in rows)
                {
                    string[] parts = row.Split(new string[] { "\t1" }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length > 1)
                    {
                        xml += parts[1];
                    }
                }
                xml = "<HISTORY>" + xml + "</HISTORY>";
                XElement history = XElement.Parse(xml);
                return Ok(history);
            }
            catch(Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        //public IQueryable<>

        // GET: api/AssociatedAccounts/5
        [ResponseType(typeof(AssociatedAccount))]
        public IHttpActionResult GetAssociatedAccount(int id)
        {
            AssociatedAccount associatedAccount = db.AssociatedAccounts.Find(id);
            if (associatedAccount == null)
            {
                return NotFound();
            }

            return Ok(associatedAccount);
        }

        // PUT: api/AssociatedAccounts/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAssociatedAccount(int id, AssociatedAccount associatedAccount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != associatedAccount.AssociatedAccountID)
            {
                return BadRequest();
            }

            db.Entry(associatedAccount).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssociatedAccountExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/AssociatedAccounts
        [ResponseType(typeof(AssociatedAccount))]
        public IHttpActionResult PostAssociatedAccount(AssociatedAccount associatedAccount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AssociatedAccounts.Add(associatedAccount);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = associatedAccount.AssociatedAccountID }, associatedAccount);
        }

        [Route("api/UpdateAssociatedAccount")]
        public IHttpActionResult PostUpdateAssociatedAccount(AssociatedAccount associatedAccoount)
        {
            var userId = User.Identity.GetUserId();
            var userAccounts = db.AssociatedAccounts.Where(p => p.UserID == userId);
            var currentAccount = userAccounts.Where(p => p.AssociatedAccountID == associatedAccoount.AssociatedAccountID).First();
            currentAccount.Description3 = associatedAccoount.Description3;
            if (associatedAccoount.DefaultAccount)
            {
                foreach(var account in userAccounts)
                {
                    account.DefaultAccount = false;
                }
                currentAccount.DefaultAccount = associatedAccoount.DefaultAccount;
            }
            db.SaveChanges();
            //currentAccount.
            return Ok(currentAccount);
        }

        [Route("AssociateAccounts")]
        [HttpPost]
        public async Task<IHttpActionResult> PostAssociateAccounts(string memberId, string pin)
        {
            string userId = RequestContext.Principal.Identity.GetUserId();
            Guid fiId = Guid.Parse(fiIdString);

            COBAccount[] memberAccountList = await COB.GetMemberAccountList(memberId, pin);
            if (memberAccountList.Length > 0)
            {

                string encryptedPin = Helpers.StringCipher.Encrypt(pin, userId);
                var userPin = db.PINs.Where(p => p.FinancialInstitutionID == fiId && p.UserID == userId).FirstOrDefault();
                if (userPin == null)
                {
                    userPin = new PIN
                    {
                        PINID = Guid.NewGuid(),
                        FinancialInstitutionID = fiId,
                        UserID = userId,
                        Pin = encryptedPin,
                        Updated = DateTime.Now
                    };
                    db.PINs.Add(userPin);
                }
                else
                {
                    userPin.Pin = encryptedPin;
                    userPin.Updated = DateTime.Now;
                }
                db.SaveChanges();

                //Gather existing associated accounts
                var existingAccounts = db.AssociatedAccounts.Where(p => p.UserID == userId);

                //Identify default account if exists
                bool defaultSpecified = existingAccounts.Where(p => p.DefaultAccount).Count() > 0;

                //only link accounts which do not currently exist
                var newAccounts = from p in memberAccountList
                                  where !existingAccounts.Select(q => q.AccountNumber).Contains(p.AcctNbr)
                                  && p.AcctStatusCode.Equals("ACT")
                                  select new AssociatedAccount
                                  {
                                      FinancialInstitutionID = fiId,
                                      MemberID = memberId,
                                      UserID = userId,
                                      AccountNumber = p.AcctNbr,
                                      AccountType = p.MajorTypeDesc,
                                      DefaultAccount = false,
                                      Description = p.ProductCategoryDesc,
                                      Description2 = p.MinorTypeDesc,
                                      Description3 = p.MinorCustDesc,
                                      Created = DateTime.Now,
                                      Status = "Active",
                                      Updated = DateTime.Now
                                      //################################################
                                      //      TEST CODE
                                      //################################################
                                      ,
                                      Verified = DateTime.Now
                                      //################################################
                                  };

                //Reactivate all existing accounts
                foreach (AssociatedAccount acct in existingAccounts)
                {
                    acct.Status = "Active";
                    acct.Updated = DateTime.Now;
                }

                db.AssociatedAccounts.AddRange(newAccounts);
                db.SaveChanges();

                var userAccounts = db.AssociatedAccounts.Where(p => p.UserID == userId && !COBExcludedAcctTypes.Contains(p.Description2));

                //If no default account exists, create it
                if (!defaultSpecified)
                {
                    var newDefault = userAccounts.Where(p => p.AccountType == "Savings" && p.Description2.Equals("Deposits")).OrderByDescending(p => p.Created).FirstOrDefault();
                    if (newDefault != null)
                    {
                        newDefault.DefaultAccount = true;
                        db.SaveChanges();
                    }
                }

                MailSender.SendMessage(User.Identity.GetUserName(), "E-Pay - Account(s) Linked", "We have successfully linked one or more accounts to your profile.");
                return Ok(userAccounts);
            }

            return Ok("No Accounts have been found.");
        }


        [Route("DisassociateAccounts")]
        [HttpPost]
        public IHttpActionResult PostDisassociateAccount(int acctid)
        {
            string userId = RequestContext.Principal.Identity.GetUserId();
            var accts = db.AssociatedAccounts.Where(p => p.UserID == userId);
            var acct = accts.Where(p => p.AssociatedAccountID == acctid).First();
            Guid financialInstitutionId = acct.FinancialInstitutionID;
            acct.Status = "Disabled";
            acct.Updated = DateTime.Now;
            db.SaveChanges();
            if (accts.Where(p => p.Status.Equals("Active") && p.FinancialInstitutionID == financialInstitutionId).Count() == 0)
            {
                var pin = db.PINs.Where(p => p.UserID == userId && p.FinancialInstitutionID == financialInstitutionId).First();
                db.PINs.Remove(pin);
                db.SaveChanges();
            }

            return Ok();
        }

        // DELETE: api/AssociatedAccounts/5
        [ResponseType(typeof(AssociatedAccount))]
        public IHttpActionResult DeleteAssociatedAccount(int id)
        {
            AssociatedAccount associatedAccount = db.AssociatedAccounts.Find(id);
            if (associatedAccount == null)
            {
                return NotFound();
            }

            db.AssociatedAccounts.Remove(associatedAccount);
            db.SaveChanges();

            return Ok(associatedAccount);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AssociatedAccountExists(int id)
        {
            return db.AssociatedAccounts.Count(e => e.AssociatedAccountID == id) > 0;
        }
    }
}