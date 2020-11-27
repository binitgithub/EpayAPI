using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using E_Pay_Web_API.Models;
using Microsoft.AspNet.Identity;
using System.Web.Http.Cors;
using Microsoft.AspNet.Identity.EntityFramework;

namespace E_Pay_Web_API.Controllers
{
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class COBSecurityReviewsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/COBSecurityReviews
        public IQueryable<COBSecurityReview> GetCOBSecurityReviews()
        {
            return db.COBSecurityReviews;
        }

        // GET: api/COBSecurityReviews/5
        [ResponseType(typeof(COBSecurityReview_Deep))]
        public IHttpActionResult GetCOBSecurityReview(int id)
        {
            COBSecurityReview_Deep cOBSecurityReviewDeep = (from p in db.COBSecurityReviews
                                                           where p.COBSecurityReviewID == id
                                                           select new COBSecurityReview_Deep
                                                           {
                                                               COBSecurityReview = p,
                                                               AssociatedAccounts = from q in db.AssociatedAccountsCOBSecurityReviews
                                                                                    from r in db.AssociatedAccounts
                                                                                    where q.COBSecurityReviewID == p.COBSecurityReviewID
                                                                                    && r.AssociatedAccountID == q.AssociatedAccountID
                                                                                    select r
                                                           }).First();
                //db.COBSecurityReviews.Include("AssociatedAccounts").Where(p=>p.COBSecurityReviewID==id).FirstOrDefault();
            
            if (cOBSecurityReviewDeep == null)
            {
                return NotFound();
            }

            return Ok(cOBSecurityReviewDeep);
        }

        [Route("api/PendingCOBSecurityReivews")]
        public IEnumerable<ApplicationUser> GetPendingCOBSecurityReviews()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var users = manager.Users.ToList();
            var pendingReviews = from p in users
                                 from q in db.AssociatedAccounts
                                 where
                                 p.Id == q.UserID
                                 && q.Verified == null
                                 select p;

            return pendingReviews;
        }

        [Route("api/COBRecentSecurityReivews")]
        public IQueryable<COBSecurityReviewSummary> GetCOBRecentSecurityReivews()
        {
            DateTime previousMonth = DateTime.Now.AddDays(-30);
            var recentReviews = from p in db.COBSecurityReviews
                                from q in db.Users
                                where
                                p.RevieweeID == q.Id
                                && p.Created >= previousMonth
                                select new COBSecurityReviewSummary
                                {
                                    ReviewID = p.COBSecurityReviewID,
                                    UserID = p.RevieweeID,
                                    FirstName = q.FirstName,
                                    LastName = q.LastName,
                                    Email = q.Email,
                                    Accounts = db.AssociatedAccounts.Where(r => r.UserID == p.RevieweeID && r.Verified != null).Count()
                                };

            return recentReviews;
        }

        [Route("api/ApprovedCOBSecurityReivews")]
        public IEnumerable<object> GetApprovedCOBSecurityReivews()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var users = manager.Users.ToList();
            var lastReviews = from p in db.COBSecurityReviews
                              where
                              p.Status.Equals("Approved")
                              group p by p.RevieweeID into g
                              select new
                              {
                                  RevieweeID = g.Key,
                                  LastReviewID = g.Max(x => x.COBSecurityReviewID)
                              };

            var userReviews = from p in users
                              from q in lastReviews
                              where
                              p.Id == q.RevieweeID
                              select new
                              {
                                  User = p,
                                  LastReviewID = q.LastReviewID
                              };

            /*
            List < ApplicationUser > approvedCOBUsers = (from p in users
                                                         from q in db.AssociatedAccounts
                                                         where p.Id == q.UserID
                                                         && q.Verified != null
                                                         select p).Distinct().ToList();
            */
            return userReviews;
        }

        [Route("api/DeniedCOBSecurityReivews")]
        public IEnumerable<object> GetDeniedCOBSecurityReviews()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var users = manager.Users.ToList();
            var lastReviews = from p in db.COBSecurityReviews
                              where
                              p.Status.Equals("Denied")
                              group p by p.RevieweeID into g
                              select new
                              {
                                  RevieweeID = g.Key,
                                  LastReviewID = g.Max(x => x.COBSecurityReviewID)
                              };

            var lastUserReviews = from p in users
                                  from q in lastReviews
                                  where
                                  p.Id == q.RevieweeID
                                  select new
                                  {
                                      User = p,
                                      LastReviewID = q.LastReviewID
                                  };

            return lastUserReviews;
        }

        [Route("api/AllCOBSecurityReivews")]
        public IEnumerable<object> GetAllCOBSecurityReviews()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var users = manager.Users.ToList();
            var lastReviews = from p in db.COBSecurityReviews
                              group p by p.RevieweeID into g
                              select new
                              {
                                  RevieweeID = g.Key,
                                  LastReviewID = g.Max(x => x.COBSecurityReviewID)
                              };

            var reviews = from p in users
                          join q in lastReviews
                          on p.Id equals q.RevieweeID into pq
                          from s in pq.DefaultIfEmpty()
                          select new
                          {
                              User = p,
                              LastReviewID = s == null ? 0 : s.LastReviewID
                          };

            return reviews;
        }
        
        [Route("api/COBSecurityReivews")]
        public List<ApplicationUser> GetCOBSecurityReivews()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var users = manager.Users.ToList();
            List<ApplicationUser> allCOBUsers = (from p in users
                                                 from q in db.AssociatedAccounts
                                                 where p.Id == q.UserID
                                                 select p).Distinct().ToList();
            return allCOBUsers;
        }

        // PUT: api/COBSecurityReviews/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCOBSecurityReview(int id, COBSecurityReview cOBSecurityReview)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cOBSecurityReview.COBSecurityReviewID)
            {
                return BadRequest();
            }

            db.Entry(cOBSecurityReview).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!COBSecurityReviewExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/COBSecurityReviews
        [ResponseType(typeof(COBSecurityReview))]
        public IHttpActionResult PostCOBSecurityReview(COBSecurityReview cOBSecurityReview)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.COBSecurityReviews.Add(cOBSecurityReview);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = cOBSecurityReview.COBSecurityReviewID }, cOBSecurityReview);
        }

        [Route("api/COBSecurityReviewsLight")]
        public IHttpActionResult PostCOBSecurityReviewLight(COBSecurityReview_Light cOBSecurityReviewLight)
        {
            DateTime currentDate = DateTime.Now;
            Nullable<DateTime> nullableCurrentDate = currentDate;
            IQueryable<AssociatedAccount> accounts;
            if (cOBSecurityReviewLight.AssociatedAccountIDs!=null)
            {
                accounts = db.AssociatedAccounts.Where(p => cOBSecurityReviewLight.AssociatedAccountIDs.Contains(p.AssociatedAccountID));
            }
            else
            {
                accounts = db.AssociatedAccounts.Where(p => p.UserID == cOBSecurityReviewLight.RevieweeID);
            }

            foreach (var reviewedAccount in accounts)
            {
                reviewedAccount.Verified = cOBSecurityReviewLight.Status.Equals("Approved") ? nullableCurrentDate : null;
            }

            COBSecurityReview cOBSecurityReview = new COBSecurityReview
            {
                RevieweeID = cOBSecurityReviewLight.RevieweeID,
                Status = cOBSecurityReviewLight.Status,
                StatusDescription = cOBSecurityReviewLight.StatusDescription,
                ReviewerID = RequestContext.Principal.Identity.GetUserId(),
                Created = currentDate
            };
            db.COBSecurityReviews.Add(cOBSecurityReview);
            db.SaveChanges();

            foreach (var reviewedAccount in accounts)
            {
                db.AssociatedAccountsCOBSecurityReviews.Add(new AssociatedAccountCOBSecurityReview
                {
                    AssociatedAccountID = reviewedAccount.AssociatedAccountID,
                    COBSecurityReviewID = cOBSecurityReview.COBSecurityReviewID
                });
            }
            db.SaveChanges();

            return Ok(new COBSecurityReview_Deep
            {
                COBSecurityReview = cOBSecurityReview,
                AssociatedAccounts = accounts
            });
        }

        // DELETE: api/COBSecurityReviews/5
        [ResponseType(typeof(COBSecurityReview))]
        public IHttpActionResult DeleteCOBSecurityReview(int id)
        {
            COBSecurityReview cOBSecurityReview = db.COBSecurityReviews.Find(id);
            if (cOBSecurityReview == null)
            {
                return NotFound();
            }

            db.COBSecurityReviews.Remove(cOBSecurityReview);
            db.SaveChanges();

            return Ok(cOBSecurityReview);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool COBSecurityReviewExists(int id)
        {
            return db.COBSecurityReviews.Count(e => e.COBSecurityReviewID == id) > 0;
        }
    }
}