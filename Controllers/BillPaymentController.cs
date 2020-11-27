using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;
using E_Pay_Web_API.Models;
using E_Pay_Web_API.MiddleWare.Ozone;
using System.Web.Http.Cors;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace E_Pay_Web_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BillPaymentController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private const string fiIdString = "8DB34F14-A886-4BDB-AC85-2351CDD0F715";

        public async Task<BillPaymentResponse> PostBillPaymentRequest(BillPaymentRequest billPaymentRequest)
        {
            string responseString = string.Empty;
            BillPaymentResponse billPaymentResponse = new BillPaymentResponse();
            Guid fiId = Guid.Parse(fiIdString);

            switch (billPaymentRequest.BillerID.ToLower())
            {
                case "869cd85f-c155-429e-8ded-b92f41b61ec1":
                    responseString = await OzoneBillPaymentProxy.PayBill(new Helpers.OzoneBillPaymentRequest()
                    {
                        MobileNumber = billPaymentRequest.BillerAccountNumber,
                        Amount = billPaymentRequest.Amount
                    });

                    responseString = Regex.Unescape(responseString);
                    responseString = responseString.Substring(1,responseString.LastIndexOf("\"")-1);

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
                    
                    if(paymentNodes.Count > 0)
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
                    break;
            }

            return billPaymentResponse;
        }
    }
}
