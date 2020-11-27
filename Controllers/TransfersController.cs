using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using E_Pay_Web_API.Models;
using Microsoft.AspNet.Identity;
using System.Web.Http.Cors;
using E_Pay_Web_API.MiddleWare;
using Microsoft.AspNet.Identity.EntityFramework;

namespace E_Pay_Web_API.Controllers
{
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TransfersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private const string fiIdString = "8DB34F14-A886-4BDB-AC85-2351CDD0F715";

        public IQueryable<Transfer> GetTransfers()
        {
            string userId = RequestContext.Principal.Identity.GetUserId();
            var userAccounts = db.AssociatedAccounts.Where(p => p.UserID == userId);
            var userTransfers = (from p in db.Transfers
                                from q in userAccounts
                                where p.SourceAccountID == q.AssociatedAccountID
                                select p).OrderByDescending(p=>p.Created);

            return userTransfers;
        }

        public IQueryable<Transfer> GetTransfers(int count)
        {
            string userId = RequestContext.Principal.Identity.GetUserId();
            var userAccounts = db.AssociatedAccounts.Where(p => p.UserID == userId);
            var userTransfers = (from p in db.Transfers
                                 from q in userAccounts
                                 where p.SourceAccountID == q.AssociatedAccountID
                                 && p.Status.Equals("Successful")
                                 select p).OrderByDescending(p => p.Created).Take(count);

            return userTransfers;
        }

        [Route("api/TransfersByAccount")]
        public IQueryable<SourceInstitutionTransferInfo> GetTransfersByAccount(Guid sourceInstitutionId, string accountNumber)
        {
            var transfers = (from p in db.Transfers
                             from q in db.AssociatedAccounts
                             from r in db.ServiceTypes
                             where (p.SourceAccountID == q.AssociatedAccountID
                             || p.BillerAccountNumber == q.AccountNumber)
                             && p.SourceInstitutionID == sourceInstitutionId
                             && q.AccountNumber == accountNumber
                             && p.ServiceTypeID == r.ServiceTypeID
                             select new SourceInstitutionTransferInfo
                             {
                                 TransferID = p.TransferID,
                                 SourceAccountID = p.SourceAccountID,
                                 SourceAccountNumber = p.SourceAccountNumber,
                                 BillerID = p.BillerID,
                                 BillerAccountNumber = p.BillerAccountNumber,
                                 BillerTransactionID = p.BillerTransactionID,
                                 ServiceName = r.ServiceName,
                                 TransferDescription = p.TransferDescription,
                                 TransferAmount = p.TransferAmount,
                                 Status = p.Status,
                                 StatusDescription = p.StatusDescription,
                                 TransferYear = p.Created.Year,
                                 TransferMonth = p.Created.Month,
                                 TransferDay = p.Created.Day,
                                 Created = p.Created.ToString()
                             }).OrderByDescending(p => p.Created);

            return transfers;
        }

        [Route("api/TransfersByUser")]
        public IQueryable<SourceInstitutionTransferInfo> GetTransfersByUser(Guid sourceInstitutionId, string userId)
        {
            var transfers = (from p in db.Transfers
                             from q in db.AssociatedAccounts
                             from r in db.ServiceTypes
                             where (p.SourceAccountID == q.AssociatedAccountID
                             || p.BillerAccountNumber == q.AccountNumber)
                             && p.SourceInstitutionID == sourceInstitutionId
                             && q.UserID == userId
                             && p.ServiceTypeID == r.ServiceTypeID
                             select new SourceInstitutionTransferInfo
                             {
                                 TransferID = p.TransferID,
                                 SourceAccountID = p.SourceAccountID,
                                 SourceAccountNumber = p.SourceAccountNumber,
                                 BillerID = p.BillerID,
                                 BillerAccountNumber = p.BillerAccountNumber,
                                 BillerTransactionID = p.BillerTransactionID,
                                 ServiceName = r.ServiceName,
                                 TransferDescription = p.TransferDescription,
                                 TransferAmount = p.TransferAmount,
                                 Status = p.Status,
                                 StatusDescription = p.StatusDescription,
                                 TransferYear = p.Created.Year,
                                 TransferMonth = p.Created.Month,
                                 TransferDay = p.Created.Day,
                                 Created = p.Created.ToString()
                             }).OrderByDescending(p => p.Created);

            return transfers;
        }

        [Route("api/TransfersByBiller")]
        public IQueryable<SourceInstitutionTransferInfo> GetTransfersByBiller(Guid sourceInstitutionId, Guid billerId)
        {
            var transfers = (from p in db.Transfers
                             from q in db.AssociatedAccounts
                             from r in db.ServiceTypes
                             where (p.SourceAccountID == q.AssociatedAccountID
                             || p.BillerAccountNumber == q.AccountNumber)
                             && p.SourceInstitutionID == sourceInstitutionId
                             && p.BillerID == billerId
                             && p.ServiceTypeID == r.ServiceTypeID
                             select new SourceInstitutionTransferInfo
                             {
                                 TransferID = p.TransferID,
                                 SourceAccountID = p.SourceAccountID,
                                 SourceAccountNumber = p.SourceAccountNumber,
                                 BillerID = p.BillerID,
                                 BillerAccountNumber = p.BillerAccountNumber,
                                 BillerTransactionID = p.BillerTransactionID,
                                 ServiceName = r.ServiceName,
                                 TransferDescription = p.TransferDescription,
                                 TransferAmount = p.TransferAmount,
                                 Status = p.Status,
                                 StatusDescription = p.StatusDescription,
                                 TransferYear = p.Created.Year,
                                 TransferMonth = p.Created.Month,
                                 TransferDay = p.Created.Day,
                                 Created = p.Created.ToString()
                             }).OrderByDescending(p => p.Created);

            return transfers;
        }

        [Route("api/BillerTransafersByServiceType")]
        public IQueryable<BillerTransferInfo> GetBillerTransafersByServiceType(Guid billerId, int serviceType)
        {
            var transfers = from t in db.Transfers
                            from b in db.Billers
                            from u in db.Users
                            from a in db.AssociatedAccounts
                            from s in db.ServiceTypes
                            where
                            t.ServiceTypeID == serviceType
                            && t.ServiceTypeID == s.ServiceTypeID
                            && t.BillerID == billerId
                            && b.BillerID == t.BillerID
                            && t.SourceAccountNumber == a.AccountNumber
                            && a.UserID == u.Id
                            select new BillerTransferInfo
                            {
                                BillerTransactionID = t.BillerTransactionID,
                                CustomerName = u.FirstName + " " + u.LastName,
                                ServiceName = s.ServiceName,
                                TransferAmount = t.TransferAmount,
                                TransferYear = t.Created.Year,
                                TransferMonth = t.Created.Month,
                                TransferDay = t.Created.Day,
                                Created = t.Created.ToString()
                            };

            return transfers;
        }

        [Route("api/TransfersByRecipientEmail")]
        public IQueryable<Transfer> GetTransfersByRecipientEmail(Guid institutionId, string email)
        {
            var transfers = from p in db.Transfers
                            from r in db.AssociatedAccounts
                            from q in db.Users
                            where p.BillerAccountNumber == r.AccountNumber
                            && r.UserID == q.Id
                            && p.SourceInstitutionID == institutionId
                            && r.FinancialInstitutionID == institutionId
                            && q.Email.Equals(email)
                            select p;

            return transfers;
        }

        //################################################
        //      LIVE CODE
        //################################################
        //public async Task<string> PostTransfer(E_Pay_Web_API.Helpers.Transfer_Light transfer)
        //################################################

        //################################################
        //      TEST CODE
        //################################################
        public string PostTransfer(E_Pay_Web_API.Helpers.Transfer_Light transfer)
        //################################################
        {
            try
            {
                Guid fiId = Guid.Parse(fiIdString);
                string userId = User.Identity.GetUserId();
                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                ApplicationUser targetUser = new ApplicationUser();
                AssociatedAccount targetAccount = new AssociatedAccount();
                switch (transfer.toInfoType)
                {
                    case "EmailAddress":
                        targetUser = manager.FindByEmail(transfer.toEmailAddress);
                        if (targetUser == null)
                        {
                            return "The email address provided was invalid. No account found.";
                        }
                        targetAccount = db.AssociatedAccounts.Where(p => p.UserID == targetUser.Id && p.DefaultAccount).FirstOrDefault();
                        break;
                    case "AccountNumber":
                        targetAccount = db.AssociatedAccounts.Where(p => p.AccountNumber.Equals(transfer.toAccountNumber) && p.DefaultAccount).FirstOrDefault();
                        targetUser = manager.FindById(targetAccount.UserID);
                        break;
                    case "MyAccount":
                        targetAccount = db.AssociatedAccounts.Where(p => p.AccountNumber.Equals(transfer.toAccountNumber) && p.UserID== userId).FirstOrDefault();
                        targetUser = manager.FindById(targetAccount.UserID);
                        break;
                }
                //
                var fromAccount = db.AssociatedAccounts.Where(p => p.AssociatedAccountID.ToString() == transfer.fromAcct).First();
                if (targetAccount == null)
                {
                    return "Recipient does not have a default account or the account number provided is invalid.";
                }
                string encryptedPin = db.PINs.Where(p => p.FinancialInstitutionID == fiId && p.UserID == userId).Select(p => p.Pin).First();
                string decryptedPin = Helpers.StringCipher.Decrypt(encryptedPin, userId);

                transfer.fromEmailAddress = userId;
                transfer.memid = fromAccount.MemberID;
                transfer.fromAcct = fromAccount.AccountNumber;
                transfer.toAccountNumber = targetAccount.AccountNumber;
                transfer.pin = decryptedPin;

                //################################################
                //      LIVE CODE
                //################################################
                //string transferResponse = await COB.TransferFunds(transfer);
                //char transferResponseCode = transferResponse.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries)[2][1];
                //string transferMessage = transferResponse.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries)[2].Substring(1);
                //################################################

                //################################################
                //      TEST CODE
                //################################################
                string transferMessage = "OKITC";
                //################################################

                db.Transfers.Add(new Transfer
                {
                    SourceInstitutionID = fiId,
                    SourceAccountID = fromAccount.AssociatedAccountID,
                    SourceAccountNumber = fromAccount.AccountNumber,
                    SourceTransactionID = "",
                    BillerID = Guid.Parse(fiIdString),
                    BillerAccountNumber = targetAccount.AccountNumber,
                    BillerTransactionID = "",
                    TransferAmount = (decimal)transfer.amount,
                    TransferDescription = "COB Internal Transfer",
                    Status = transferMessage.Equals("OKITC") ? "Successful" : "Error",
                    StatusDescription = transferMessage,
                    Created = DateTime.Now
                });
                db.SaveChanges();

                return transferMessage;
            }
            catch(Exception ex)
            {
                return "A fatal error has occurred. Please try again later.";
            }
        }

        [Route("api/SourceInstitutionTransfers")]
        public IQueryable<Transfer> GetSourceInstitutionTransfers(Guid institutionId)
        {
            return db.Transfers.Where(p => p.SourceInstitutionID == institutionId);
        }

        [Route("api/SourceInstitutionTransfersByDate")]
        public IQueryable<Transfer> GetSourceInstitutionTransfersByDate(Guid institutionId, DateTime transferDate)
        {
            DateTime nextDay = transferDate.AddDays(1);
            return db.Transfers.Where(p => p.Created >= transferDate && p.Created < nextDay && p.SourceInstitutionID == institutionId);
        }

        [Route("api/SourceInstitutionTotalTransfersByDate")]
        public IQueryable<Object> GetSourceInstitutionTotalTransfersByDate(Guid institutionId, DateTime fromDate, DateTime toDate)
        {
            DateTime nextDay = toDate.AddDays(1);
            //return db.Transfers.Where(p => p.Created >= fromDate && p.Created < nextDay && p.SourceInstitutionID == institutionId);
            var totals = from p in db.Transfers
                         where (p.SourceInstitutionID == institutionId
                         || p.BillerID == institutionId)
                         && p.Created >= fromDate
                         && p.Created < nextDay
                         group p by p.Created.Date into dailyTotals
                         select new
                         {
                             Date = dailyTotals.Key,
                             Day = dailyTotals.Key.Day,
                             Month = dailyTotals.Key.Month,
                             Year = dailyTotals.Key.Year,
                             Total = dailyTotals.Count()
                         };

            return totals;
        }

        [Route("api/TransferSummaryByDate")]
        public SourceInstitutionBillerSummary GetTransferSummaryByDate(Guid institutionId, DateTime transferDate)
        {
            DateTime nextDay = transferDate.AddDays(1);
            SourceInstitutionBillerSummary summary = new SourceInstitutionBillerSummary();
            var institution = db.FinancialInstitutions.Where(p => p.FinancialInstitutionID == institutionId).First();
            summary.SourceInstitutionID = institution.FinancialInstitutionID;
            summary.SourceInstitutionName = institution.Name;
            summary.BillerSummaries = from p in db.Transfers
                                      from q in db.Billers
                                      where p.SourceInstitutionID == institutionId
                                      && p.BillerID == q.BillerID
                                      && p.Created >= transferDate
                                      && p.Created < nextDay
                                      group new { p, q } by new { p.BillerID, q.DisplayName } into summaries
                                      select new BillerSummary
                                      {
                                          BillerID = summaries.Key.BillerID,
                                          BillerName = summaries.Key.DisplayName,
                                          TransferCount = summaries.Count(),
                                          TransferTotal = summaries.Sum(p => p.p.TransferAmount)
                                      };

            return summary;
        }

        [Route("api/TransferSummaryByMonth")]
        public SourceInstitutionBillerSummary GetTransferSummaryByMonth(Guid institutionId, int transferYear, int transferMonth)
        {
            SourceInstitutionBillerSummary summary = new SourceInstitutionBillerSummary();
            var institution = db.FinancialInstitutions.Where(p => p.FinancialInstitutionID == institutionId).First();
            summary.SourceInstitutionID = institution.FinancialInstitutionID;
            summary.SourceInstitutionName = institution.Name;
            summary.BillerSummaries = from p in db.Transfers
                                      from q in db.Billers
                                      where p.SourceInstitutionID == institutionId
                                      && p.BillerID == q.BillerID
                                      && p.Created.Year == transferYear
                                      && p.Created.Month == transferMonth
                                      group new { p, q } by new { p.BillerID, q.DisplayName } into summaries
                                      select new BillerSummary
                                      {
                                          BillerID = summaries.Key.BillerID,
                                          BillerName = summaries.Key.DisplayName,
                                          TransferCount = summaries.Count(),
                                          TransferTotal = summaries.Sum(p => p.p.TransferAmount)
                                      };

            return summary;
        }

        [Route("api/TransferSummaryByYear")]
        public SourceInstitutionBillerSummary GetTransferSummaryByYear(Guid institutionId, int transferYear)
        {
            SourceInstitutionBillerSummary summary = new SourceInstitutionBillerSummary();
            var institution = db.FinancialInstitutions.Where(p => p.FinancialInstitutionID == institutionId).First();
            summary.SourceInstitutionID = institution.FinancialInstitutionID;
            summary.SourceInstitutionName = institution.Name;
            summary.BillerSummaries = from p in db.Transfers
                                      from q in db.Billers
                                      where p.SourceInstitutionID == institutionId
                                      && p.BillerID == q.BillerID
                                      && p.Created.Year == transferYear
                                      group new { p, q } by new { p.BillerID, q.DisplayName } into summaries
                                      select new BillerSummary
                                      {
                                          BillerID = summaries.Key.BillerID,
                                          BillerName = summaries.Key.DisplayName,
                                          TransferCount = summaries.Count(),
                                          TransferTotal = summaries.Sum(p => p.p.TransferAmount)
                                      };

            return summary;
        }

        [Route("api/SourceInstitutionTransfersByMonth")]
        public IQueryable<Transfer> GetSourceInstitutionTransfersByMonth(Guid institutionId, int transferYear, int transferMonth)
        {
            return db.Transfers.Where(p => p.Created.Year == transferYear && p.Created.Month == transferMonth && p.SourceInstitutionID == institutionId);
        }

        [Route("api/SourceInstitutionTransfersByYear")]
        public IQueryable<Transfer> GetSourceInstitutionTransfersByYear(Guid institutionId, int transferYear)
        {
            return db.Transfers.Where(p => p.Created.Year == transferYear && p.SourceInstitutionID == institutionId);
        }

        [Route("api/TotalFundsTransferedByBiller")]
        public IQueryable<BillerTotalFunds> GetTotalFundsTransferedByBiller(Guid institutionId)
        {
            var totals = from p in db.Transfers
                         from q in db.Billers
                         where
                         p.BillerID == q.BillerID
                         && p.SourceInstitutionID == institutionId
                         group new { p, q } by new { p.BillerID, q.DisplayName } into billerTotals
                         select new BillerTotalFunds
                         {
                             BillerID = billerTotals.Key.BillerID,
                             BillerName = billerTotals.Key.DisplayName,
                             Total = billerTotals.Sum(p => p.p.TransferAmount)
                         };

            return totals;
        }

        [Route("api/TotalTransfersByBiller")]
        public IQueryable<BillerTotalTransfers> GetTotalTransfersByBiller(Guid institutionId)
        {
            var totals = from p in db.Transfers
                         from q in db.Billers
                         where
                         p.BillerID == q.BillerID
                         && p.SourceInstitutionID == institutionId
                         group new { p, q } by new { p.BillerID, q.DisplayName } into billerTotals
                         select new BillerTotalTransfers
                         {
                             BillerID = billerTotals.Key.BillerID,
                             BillerName = billerTotals.Key.DisplayName,
                             Total = billerTotals.Count()
                         };

            return totals;
        }

        [Route("api/TotalBillerTransfersByYear")]
        public IQueryable<BillerTransfersByYear> GetTotalBillerTransfersByYear(Guid institutionId)
        {
            var years = db.Transfers.Select(p => p.Created).ToList().Select(p => p.Year).Distinct().OrderBy(p=>p);

            var totals = from p in db.Transfers
                         from q in db.Billers
                         where
                         p.BillerID == q.BillerID
                         && p.SourceInstitutionID == institutionId
                         group new { p, q } by new { p.BillerID, q.DisplayName, p.Created.Year } into billerTotals
                         orderby billerTotals.Key.Year
                         select new BillerTransfersByYear
                         {
                             BillerID = billerTotals.Key.BillerID,
                             BillerName = billerTotals.Key.DisplayName,
                             Year = billerTotals.Key.Year,
                             Total = billerTotals.Count()
                         };

            return totals;
        }

    }
}
