using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Pay_Web_API.Models
{
    public class Biller
    {
        public Guid BillerID { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string MinorAccountDescription { get; set; }
        public string AdminRole { get; set; }
        public string Status { get; set; }
        public string StatusDescription { get; set; }
        public DateTime Updated { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime Created { get; set; }
    }

    public class BillerTotalFunds
    {
        public Guid BillerID;
        public string BillerName;
        public decimal Total;
    }

    public class BillerTotalTransfers
    {
        public Guid BillerID;
        public string BillerName;
        public int Total;
    }

    public class BillerTransfersByYear
    {
        public Guid BillerID;
        public string BillerName;
        public int Year;
        public int Total;
    }

    public class BillerSummary
    {
        public Guid BillerID;
        public string BillerName;
        public int TransferCount;
        public decimal TransferTotal;
    }
}