using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Pay_Web_API.Models
{
    public class BillerFee
    {
        public int BillerFeeID { get; set; }
        public Guid BillerID { get; set; }
        public int ServiceTypeID { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public bool ApplyAsPercentage { get; set; }
        public DateTime Updated { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime Created { get; set; }
    }
}