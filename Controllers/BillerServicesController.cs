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
using E_Pay_Web_API.Models;
using System.Web.Http.Cors;

namespace E_Pay_Web_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BillerServicesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/BillerServices
        public IQueryable<BillerService> GetBillerServices()
        {
            return db.BillerServices;
        }

        // GET: api/BillerServices
        public IQueryable<BillerService> GetBillerServices(Guid billerId)
        {
            return db.BillerServices.Where(p=>p.BillerID == billerId);
        }

        // GET: api/BillerServices/5
        [ResponseType(typeof(BillerService))]
        public async Task<IHttpActionResult> GetBillerService(Guid id)
        {
            BillerService billerService = await db.BillerServices.FindAsync(id);
            if (billerService == null)
            {
                return NotFound();
            }

            return Ok(billerService);
        }

        // PUT: api/BillerServices/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutBillerService(Guid id, BillerService billerService)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != billerService.BillerID)
            {
                return BadRequest();
            }

            db.Entry(billerService).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BillerServiceExists(id))
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

        // POST: api/BillerServices
        [ResponseType(typeof(BillerService))]
        public async Task<IHttpActionResult> PostBillerService(BillerService billerService)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BillerServices.Add(billerService);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BillerServiceExists(billerService.BillerID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = billerService.BillerID }, billerService);
        }

        // DELETE: api/BillerServices/5
        [ResponseType(typeof(BillerService))]
        public async Task<IHttpActionResult> DeleteBillerService(Guid id)
        {
            BillerService billerService = await db.BillerServices.FindAsync(id);
            if (billerService == null)
            {
                return NotFound();
            }

            db.BillerServices.Remove(billerService);
            await db.SaveChangesAsync();

            return Ok(billerService);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BillerServiceExists(Guid id)
        {
            return db.BillerServices.Count(e => e.BillerID == id) > 0;
        }
    }
}