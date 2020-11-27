using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Pay_Web_API.Models
{
    public class COBSecurityResponse
    {
        public int COBSecurityResponseID { get; set; }
        public string UserID { get; set; }
        public string QuestionText { get; set; }
        public string ResponseText { get; set; }
        public DateTime Created { get; set; }
    }

    public class COBSecurityResponse_Light
    {
        public string QuestionText;
        public string ResponseText;
    }
}