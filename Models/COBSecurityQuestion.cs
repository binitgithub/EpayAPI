using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Pay_Web_API.Models
{
    public class COBSecurityQuestion
    {
        public int COBSecurityQuestionID { get; set; }
        public string SecurityQuestionText { get; set; }
        public bool Active { get; set; }
    }
}