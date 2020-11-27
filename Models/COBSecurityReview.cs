using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Pay_Web_API.Models
{
    public class COBSecurityReview
    {
        public int COBSecurityReviewID { get; set; }
        public string ReviewerID { get; set; }
        public string RevieweeID { get; set; }
        public string Status { get; set; }
        public string StatusDescription { get; set; }
        public DateTime Created { get; set; }
    }

    public class COBSecurityReview_Deep
    {
        public COBSecurityReview COBSecurityReview;
        public IQueryable<AssociatedAccount> AssociatedAccounts;
    }

    public class COBSecurityReview_Light
    {
        public string RevieweeID;
        public string Status;
        public string StatusDescription;
        public int[] AssociatedAccountIDs;
    }

    public class COBSecurityReviewSummary
    {
        public int ReviewID;
        public string UserID;
        public string FirstName;
        public string LastName;
        public string Email;
        public int Accounts;
    }
}