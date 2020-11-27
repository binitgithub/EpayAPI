using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Pay_Web_API.Models
{
    public class Transfer
    {
        public int TransferID { get; set; }
        public Guid SourceInstitutionID { get; set; }
        public string SourceAccountNumber { get; set; }
        public int SourceAccountID { get; set; }
        public string SourceTransactionID { get; set; }
        public string MinorAccountNumber { get; set; }
        public Guid BillerID { get; set; }
        public string BillerAccountNumber { get; set; }
        public string BillerTransactionID { get; set; }
        public string TransferDescription { get; set; }
        public int ServiceTypeID { get; set; }
        public decimal TransferAmount { get; set; }
        ///TransferAmount plus all fees
        public decimal TransferTotal { get; set; }
        public string Status { get; set; }
        public string StatusDescription { get; set; }
        public DateTime Created { get; set; }
    }

    public class Transfer_Light
    {
        public int TransferID;
        public string Direction;
        public string TransferDescription;
        public decimal TransferAmount;
        public int TransferYear;
        public int TransferMonth;
        public int TransferDay;
        public DateTime Created;
    }

    public class SourceInstitutionTransferInfo
    {
        public int TransferID;
        public int SourceAccountID;
        public string SourceAccountNumber;
        public Guid BillerID;
        public string BillerAccountNumber;
        public string BillerTransactionID;
        public string TransferDescription;
        public decimal TransferAmount;
        public string ServiceName;
        public string Status;
        public string StatusDescription;
        public int TransferYear;
        public int TransferMonth;
        public int TransferDay;
        public string Created;
    }

    public class BillerTransferInfo
    {
        public string BillerTransactionID;
        public string CustomerName;
        public string ServiceName;
        public decimal TransferAmount;
        public int TransferYear;
        public int TransferMonth;
        public int TransferDay;
        public string Created;
    }
}