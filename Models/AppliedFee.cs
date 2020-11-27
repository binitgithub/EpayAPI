using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Pay_Web_API.Models
{
    public class AppliedFee
    {
        public int AppliedFeeID { get; set; }
        public int TransfrerID { get; set; }
        public int ServiceTypeID { get; set; }
        public decimal Amount { get; set; }
    }
}