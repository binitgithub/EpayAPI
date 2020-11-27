using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Pay_Web_API.Models
{
    public class FinancialInstitutionAdminEmail
    {
        public int FinancialInstitutionAdminEmailID { get; set; }
        public Guid FinancialInstitutionID { get; set; }
        public string EmailAddress { get; set; }
        public string ContactName { get; set; }
    }
}