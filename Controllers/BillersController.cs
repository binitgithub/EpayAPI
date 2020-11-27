using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Cors;
using E_Pay_Web_API.Models;

namespace E_Pay_Web_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BillersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Billers
        public IQueryable<Biller> GetBillers()
        {
            return db.Billers;
        }

        // GET: api/Billers/5
        [ResponseType(typeof(Biller))]
        public async Task<IHttpActionResult> GetBiller(Guid id)
        {
            Biller biller = await db.Billers.FindAsync(id);
            if (biller == null)
            {
                return NotFound();
            }

            return Ok(biller);
        }

        public IQueryable<Biller> GetBillers(string service)
        {
            var billers = from p in db.Billers
                          from q in db.BillerServices
                          from r in db.ServiceTypes
                          where
                          p.BillerID == q.BillerID
                          && q.ServiceTypeID == r.ServiceTypeID
                          && r.ServiceName.ToLower() == service.ToLower()
                          select p;

            return billers;
        }

        // PUT: api/Billers/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutBiller(Guid id, Biller biller)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != biller.BillerID)
            {
                return BadRequest();
            }

            db.Entry(biller).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BillerExists(id))
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

        // POST: api/Billers
        [ResponseType(typeof(Biller))]
        public async Task<IHttpActionResult> PostBiller(Biller biller)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Billers.Add(biller);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BillerExists(biller.BillerID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = biller.BillerID }, biller);
        }

        // DELETE: api/Billers/5
        [ResponseType(typeof(Biller))]
        public async Task<IHttpActionResult> DeleteBiller(Guid id)
        {
            Biller biller = await db.Billers.FindAsync(id);
            if (biller == null)
            {
                return NotFound();
            }

            db.Billers.Remove(biller);
            await db.SaveChangesAsync();

            return Ok(biller);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BillerExists(Guid id)
        {
            return db.Billers.Count(e => e.BillerID == id) > 0;
        }
    }
}