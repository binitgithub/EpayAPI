using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Pay_Web_API.Models
{
    public class ServiceType
    {
        public int ServiceTypeID { get; set; }
        public string ServiceName { get; set; }
        public string ServiceDescription { get; set; }
        public DateTime Created { get; set; }
    }
}