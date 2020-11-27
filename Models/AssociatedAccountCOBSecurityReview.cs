using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Pay_Web_API.Models
{
    public class AssociatedAccountCOBSecurityReview
    {
        [Key]
        [Column(Order = 1)]
        public int AssociatedAccountID { get; set; }
        [Key]
        [Column(Order = 2)]
        public int COBSecurityReviewID { get; set; }
    }
}