using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Pay_Web_API.Helpers
{
    public class DailyTotal
    {
        public int Year;
        public int Month;
        public int Day;
        public decimal Total;
        public int Transfers;
    }
}