using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Pay_Web_API.Models
{
    public class BillPaymentRequest
    {
        public int AssociatedAccountID { get; set; }
        public string BillerID { get; set; }
        public string BillerAccountNumber { get; set; }
        public decimal Amount { get; set; }
    }

    public class BillPaymentResponse
    {
        public bool Successful { get; set; }
        public string Message { get; set; }
        public string TransactionID { get; set; }
    }
}