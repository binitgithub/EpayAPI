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

namespace E_Pay_Web_API.Controllers
{
    public class PINsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/PINs
        public IQueryable<PIN> GetPINs()
        {
            return db.PINs;
        }

        // GET: api/PINs/5
        [ResponseType(typeof(PIN))]
        public async Task<IHttpActionResult> GetPIN(Guid id)
        {
            PIN pIN = await db.PINs.FindAsync(id);
            if (pIN == null)
            {
                return NotFound();
            }

            return Ok(pIN);
        }

        // PUT: api/PINs/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPIN(Guid id, PIN pIN)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pIN.PINID)
            {
                return BadRequest();
            }

            db.Entry(pIN).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PINExists(id))
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

        // POST: api/PINs
        [ResponseType(typeof(PIN))]
        public async Task<IHttpActionResult> PostPIN(PIN pIN)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PINs.Add(pIN);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PINExists(pIN.PINID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = pIN.PINID }, pIN);
        }

        // DELETE: api/PINs/5
        [ResponseType(typeof(PIN))]
        public async Task<IHttpActionResult> DeletePIN(Guid id)
        {
            PIN pIN = await db.PINs.FindAsync(id);
            if (pIN == null)
            {
                return NotFound();
            }

            db.PINs.Remove(pIN);
            await db.SaveChangesAsync();

            return Ok(pIN);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PINExists(Guid id)
        {
            return db.PINs.Count(e => e.PINID == id) > 0;
        }
    }
}