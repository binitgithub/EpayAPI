using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Pay_Web_API.Models
{
    public class FinancialInstitution
    {
        public Guid FinancialInstitutionID { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
    }

    public class SourceInstitutionBillerSummary
    {
        public Guid SourceInstitutionID;
        public string SourceInstitutionName;
        public IQueryable<BillerSummary> BillerSummaries;
    }
}