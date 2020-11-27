using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Threading.Tasks;
using E_Pay_Web_API.Models;
using E_Pay_Web_API.MiddleWare.Digicel;
using E_Pay_Web_API.MiddleWare;
using E_Pay_Web_API.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Http.Cors;

namespace E_Pay_Web_API.Controllers
{
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TopUpsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private const string fiIdString = "8DB34F14-A886-4BDB-AC85-2351CDD0F715";

        [ResponseType(typeof(TopUpResponse))]
        public async Task<IHttpActionResult> PostTopUpRequest(TopUpRequest topUpRequest)
        {
            //return NotFound();
            Guid fiId = Guid.Parse(fiIdString);
            TopUpResponse topUpResponse = new TopUpResponse();
            Helpers.Transfer_Light transfer = new Helpers.Transfer_Light();
            switch (topUpRequest.CarrierID)
            {
                case "Digicel":
                    string userId = User.Identity.GetUserId();
                    var userAccounts = db.AssociatedAccounts.Where(p => p.UserID.Equals(userId));
                    var fromAcct = userAccounts.Where(p => p.AssociatedAccountID == topUpRequest.AssociatedAccountID).FirstOrDefault();
                    var minorAcct = userAccounts.Where(p => p.Description2.Equals("EPAY DIGICEL TOPUP")).FirstOrDefault();
                    if(fromAcct == null)
                    {
                        topUpResponse.Message = "Invalid source account specified.";
                        topUpResponse.Successful = false;
                    }
                    //if(minorAcct != null)
                    //{
                    ////###################################################
                    //// Set maximum TopUps to a single number
                    ////###################################################
                    //int totalTopUps = db.Transfers.Where(p => p.BillerAccountNumber.Equals(topUpRequest.MobileNumber)).Count();
                    //if (totalTopUps > 2)
                    //{
                    //    topUpResponse.Message = "Test credit has been depleted.";
                    //    topUpResponse.Successful = false;
                    //}
                    //else
                    //{
                    //    topUpRequest.Amount = 5;
                    //    //return Ok(DigicelTopUp.TopUp(topUpRequest.MobileNumber, topUpRequest.Amount));
                    //    DigicelTopUpResponse digicelTopUpResponse = DigicelTopUp.TopUp(topUpRequest.Territory, topUpRequest.MobileNumber, topUpRequest.Amount);
                    //    topUpResponse.Successful = digicelTopUpResponse.Code == "0";
                    //    topUpResponse.Message = digicelTopUpResponse.Message;
                    //    topUpResponse.TransactionID = digicelTopUpResponse.TransactionID;
                    //}
                    ////##############################################

                    //################################################
                    //      LIVE CODE
                    //################################################
                    //DigicelTopUpResponse digicelTopUpResponse = DigicelTopUp.TopUp(topUpRequest.Territory, topUpRequest.MobileNumber, topUpRequest.Amount);
                    //topUpResponse.Successful = digicelTopUpResponse.Code == "0";
                    //topUpResponse.Message = digicelTopUpResponse.Message;
                    //topUpResponse.TransactionID = digicelTopUpResponse.TransactionID;
                    //################################################

                    //################################################
                    //      TEST CODE
                    //################################################
                    DigicelTopUpResponse digicelTopUpResponse = new DigicelTopUpResponse
                    {
                        Code = "0",
                        Message = "TopUp to " + topUpRequest.MobileNumber + " Successful",
                        TransactionID = Guid.NewGuid().ToString()
                    };
                    topUpResponse.Successful = digicelTopUpResponse.Code == "0";
                    topUpResponse.Message = digicelTopUpResponse.Message;
                    topUpResponse.TransactionID = digicelTopUpResponse.TransactionID;
                    //################################################
                    //}
                    //else
                    //{
                    //    topUpResponse.Message = "Your account has not been provisioned for this transaction.";
                    //    topUpResponse.Successful = false;
                    //}

                    if (!topUpResponse.Successful)
                    {
                        return Ok(topUpResponse.Message);
                    }


                    //################################################
                    //      LIVE CODE
                    //################################################
                    //string encryptedPin = db.PINs.Where(p => p.FinancialInstitutionID == fiId && p.UserID == userId).Select(p => p.Pin).First();
                    //string decryptedPin = Helpers.StringCipher.Decrypt(encryptedPin, userId);

                    //transfer.fromAcct = fromAcct.AccountNumber;
                    //transfer.amount = (double)topUpRequest.Amount;
                    //transfer.toAccountNumber = minorAcct.AccountNumber;
                    //transfer.memid = fromAcct.MemberID;
                    //transfer.pin = decryptedPin;

                    //string transferResponse = await COB.TransferFunds(transfer);
                    //string[] transferResponseArray = transferResponse.Split(new string[] { "ITC" }, StringSplitOptions.RemoveEmptyEntries)[0].Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    //if (transferResponseArray[1][0] != '1')
                    //{
                    //    //topUpResponse.Successful = false;
                    //    topUpResponse.Message = transferResponseArray[1];
                    //    //return Ok(topUpResponse);
                    //}
                    //################################################


                    break;
                case "Ozone":
                    BillPaymentResponse response = await MiddleWare.Ozone.OzoneBillPaymentProxy.PayBill(new BillPaymentRequest
                    {
                        Amount = topUpRequest.Amount,
                        AssociatedAccountID = topUpRequest.AssociatedAccountID,
                        BillerAccountNumber = topUpRequest.MobileNumber,
                        BillerID = "869cd85f-c155-429e-8ded-b92f41b61ec1"
                    });
                    topUpResponse.Successful = response.Successful;
                    topUpResponse.Message = response.Message;
                    topUpResponse.TransactionID = response.TransactionID;
                    break;
                default:
                    topUpResponse.Successful = false;
                    topUpResponse.Message = "Invalid Mobile Provider";
                    break;
            }

            AssociatedAccount account = db.AssociatedAccounts.Where(p => p.AssociatedAccountID == topUpRequest.AssociatedAccountID).First();
            Guid billerId = db.Billers.Where(p => p.Name.Equals(topUpRequest.CarrierID)).Select(p => p.BillerID).First();
            db.Transfers.Add(new E_Pay_Web_API.Models.Transfer
            {
                SourceTransactionID = DateTime.Now.ToString("yyyyMMdd-mmss"),
                SourceAccountNumber = account.AccountNumber,
                SourceAccountID = account.AssociatedAccountID,
                SourceInstitutionID = account.FinancialInstitutionID,
                MinorAccountNumber = transfer.toAccountNumber,
                BillerID = billerId,
                //BillerID = Guid.Parse(topUpRequest.CarrierID),
                BillerAccountNumber = topUpRequest.MobileNumber,
                BillerTransactionID = topUpResponse.TransactionID,
                TransferAmount = topUpRequest.Amount,
                TransferDescription = topUpRequest.CarrierID + " - TopUp",
                Status = topUpResponse.Successful ? "Successful" : "Error",
                StatusDescription = topUpResponse.Message,
                Created = DateTime.Now
            });
            db.SaveChanges();

            if (topUpResponse.Successful)
            {
                MailSender.SendMessage(
                    User.Identity.GetUserName(),
                    "E-Pay Top Up Confirmation and Receipt of Mobile Top Up Sent",
                    "Congratulations your order has been completed successfully.\r\n\r\n"
                    + "1 successful\r\n\r\n"
                    + "Order ID: " + topUpResponse.TransactionID + "\r\n"
                    + "Product: Mobile Top Up\r\n"
                    + "Number: " + topUpRequest.MobileNumber + "\r\n"
                    + "Country: Barbados\r\n"
                    + "Top-up Amount: BBD $" + topUpRequest.Amount.ToString() + "\r\n"
                    + "Receive Amount: BBD $" + topUpRequest.Amount.ToString() + "\r\n\r\n\r\n"
                    + "Date & Time: " + DateTime.Now.ToString() + "\r\n\r\n"
                    + "Amount Paid:  BBD $" + topUpRequest.Amount.ToString() + "\r\n\r\n"
                    + "From Account: " + account.Description + ": " + account.Description3 +" - "+account.AccountNumber + "\r\n\r\n"
                    + "If you have any queries, please contact our customer care team.\r\n"
                    + "Thanks,\r\n"
                    + "The E-Pay Team"
                    );
            }
            return Ok(topUpResponse);
            //return NotFound();
        }

        [Route("DigicelTopUps")]
        public IQueryable GetDigicelTopUps(string date)
        {
            DateTime currentDate = DateTime.Parse(date);
            DateTime nextDate = currentDate.AddDays(1);
            return db.Transfers.Where(p => p.TransferDescription == "Digicel - TopUp" && p.Created >= currentDate && p.Created < nextDate);
        }

    }
}
