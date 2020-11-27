using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Pay_Web_API.Helpers
{
    public class Transfer_Light
    {
        public string fromEmailAddress;
        public string fromAcct;
        public string toInfoType;
        public string toEmailAddress;
        public string toAccountNumber;
        public double amount;
        public string memid;
        public string pin;
    }
}