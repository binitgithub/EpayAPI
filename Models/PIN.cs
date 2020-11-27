using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Pay_Web_API.Models
{
    public class PIN
    {
        public Guid PINID { get; set; }
        public Guid FinancialInstitutionID { get; set; }
        public string UserID { get; set; }
        public string Pin { get; set; }
        public DateTime Updated { get; set; }
    }
}