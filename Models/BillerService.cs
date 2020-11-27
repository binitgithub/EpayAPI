using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Pay_Web_API.Models
{
    public class BillerService
    {
        [Key]
        [Column(Order = 1)]
        public Guid BillerID { get; set; }
        [Key]
        [Column(Order = 2)]
        public int ServiceTypeID { get; set; }
    }
}