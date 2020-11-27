using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Pay_Web_API.Models
{
    public class TopUpRequest
    {
        public int AssociatedAccountID { get; set; }
        public string CarrierID { get; set; }
        public string Territory { get; set; }
        public string MobileNumber { get; set; }
        public decimal Amount { get; set; }
    }

    public class TopUpResponse
    {
        public bool Successful { get; set; }
        public string Message { get; set; }
        public string TransactionID { get; set; }
    }
}