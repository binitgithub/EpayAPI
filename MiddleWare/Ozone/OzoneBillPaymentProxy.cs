using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using E_Pay_Web_API.Models;
using E_Pay_Web_API.Helpers;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using System.Xml;

namespace E_Pay_Web_API.MiddleWare.Ozone
{
    public static class OzoneBillPaymentProxy
    {
        private static readonly HttpClient client = new HttpClient();
        private static ApplicationDbContext db = new ApplicationDbContext();
        private const string fiIdString = "8DB34F14-A886-4BDB-AC85-2351CDD0F715";
        public static async Task<string> PayBill(OzoneBillPaymentRequest request)
        {
            var response = await client.PostAsJsonAsync("http://192.168.254.1/api/BillPaymentRequest", request);
            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<BillPaymentResponse> PayBill(BillPaymentRequest billPaymentRequest)
        {
            string responseString = string.Empty;
            BillPaymentResponse billPaymentResponse = new BillPaymentResponse();
            Guid fiId = Guid.Parse(fiIdString);

            Helpers.OzoneBillPaymentRequest ozoneBillPaymentRequest = new Helpers.OzoneBillPaymentRequest()
            {
                MobileNumber = billPaymentRequest.BillerAccountNumber.Substring(3),
                Amount = billPaymentRequest.Amount
            };
            responseString = await OzoneBillPaymentProxy.PayBill(ozoneBillPaymentRequest);

            responseString = Regex.Unescape(responseString);
            responseString = responseString.Substring(1, responseString.LastIndexOf("\"") - 1);

            byte[] encodedString = Encoding.UTF8.GetBytes(responseString);
            MemoryStream ms = new MemoryStream(encodedString);
            ms.Flush();
            ms.Position = 0;
            XmlDocument doc = new XmlDocument();
            doc.Load(ms);
            ms.Close();

            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(doc.NameTable);
            namespaceManager.AddNamespace("soapenv", "http://schemas.xmlsoap.org/soap/envelope/");
            namespaceManager.AddNamespace("ns1", "http://api.csr.mind.com");
            namespaceManager.AddNamespace("ns9", "http://api.csr.mind.com/addPayment");

            XmlNodeList paymentNodes = doc.SelectNodes("/soapenv:Envelope/soapenv:Body/ns1:addPaymentReturn/ns9:response/ns9:payment", namespaceManager);

            if (paymentNodes.Count > 0)
            {

                billPaymentResponse.Successful = true;
                billPaymentResponse.Message = "Payment to mobile number " + billPaymentRequest.BillerAccountNumber + " successful.";
                billPaymentResponse.TransactionID = paymentNodes[0].Attributes["id"].Value;
            }
            else
            {
                billPaymentResponse.Successful = false;
                billPaymentResponse.Message = "There was an error. Please ensure the mobile number is correct.";
            }

            var sourceAccount = db.AssociatedAccounts.Where(p => p.AssociatedAccountID == billPaymentRequest.AssociatedAccountID).First();
            db.Transfers.Add(new Transfer
            {
                SourceInstitutionID = fiId,
                SourceAccountID = billPaymentRequest.AssociatedAccountID,
                SourceAccountNumber = sourceAccount.AccountNumber,
                SourceTransactionID = "",
                BillerID = Guid.Parse(fiIdString),
                BillerAccountNumber = billPaymentRequest.BillerAccountNumber,
                BillerTransactionID = billPaymentResponse.TransactionID,
                TransferAmount = (decimal)billPaymentRequest.Amount,
                TransferDescription = "Ozone Bill Payment",
                Status = paymentNodes.Count > 0 ? "Successful" : "Error",
                StatusDescription = billPaymentResponse.Message,
                Created = DateTime.Now
            });
            db.SaveChanges();

            return billPaymentResponse;
        }
    }
}