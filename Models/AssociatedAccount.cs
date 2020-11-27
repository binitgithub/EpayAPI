using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace E_Pay_Web_API.Models
{
    public class AssociatedAccount
    {
        public int AssociatedAccountID { get; set; }
        public string UserID { get; set; }
        public Guid FinancialInstitutionID { get; set; }
        public string MemberID { get; set; }
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public string Description { get; set; }
        public string Description2 { get; set; }
        public string Description3 { get; set; }
        public bool DefaultAccount { get; set; }
        public DateTime? Verified { get; set; }
        public DateTime Created { get; set; }
        public string Status { get; set; }
        public DateTime Updated { get; set; }
    }

    public class AccountBalance
    {
        public string AccountNumber { get; set; }
        public string CurrentBalance { get; set; }
        public string AvailableBalance { get; set; }
    }

    public class AssociatedAccount_Light
    {
        public int AssociatedAccountID;
        public Guid FinancialInstitutionID;
        public string FinancialInstitutionName;
        public string AccountNumber;
        public string AccountType;
        public string Description;
        public string Description2;
        public string Description3;
        public bool DefaultAccount;
        public bool Verified;
    }
}