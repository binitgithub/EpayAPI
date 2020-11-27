using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Pay_Web_API.Models
{
    public class ResetPasswordRequest
    {
        public int ResetPasswordRequestID { get; set; }
        public string UserID { get; set; }
        public DateTime? Clicked { get; set; }
        public DateTime Created { get; set; }
    }
}