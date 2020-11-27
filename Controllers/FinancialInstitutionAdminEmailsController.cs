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
    public class FinancialInstitutionAdminEmailsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/FinancialInstitutionAdminEmails
        public IQueryable<FinancialInstitutionAdminEmail> GetFinancialInstitutionAdminEmails()
        {
            return db.FinancialInstitutionAdminEmails;
        }

        // GET: api/FinancialInstitutionAdminEmails/5
        [ResponseType(typeof(FinancialInstitutionAdminEmail))]
        public async Task<IHttpActionResult> GetFinancialInstitutionAdminEmail(int id)
        {
            FinancialInstitutionAdminEmail financialInstitutionAdminEmail = await db.FinancialInstitutionAdminEmails.FindAsync(id);
            if (financialInstitutionAdminEmail == null)
            {
                return NotFound();
            }

            return Ok(financialInstitutionAdminEmail);
        }

        // PUT: api/FinancialInstitutionAdminEmails/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFinancialInstitutionAdminEmail(int id, FinancialInstitutionAdminEmail financialInstitutionAdminEmail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != financialInstitutionAdminEmail.FinancialInstitutionAdminEmailID)
            {
                return BadRequest();
            }

            db.Entry(financialInstitutionAdminEmail).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FinancialInstitutionAdminEmailExists(id))
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

        // POST: api/FinancialInstitutionAdminEmails
        [ResponseType(typeof(FinancialInstitutionAdminEmail))]
        public async Task<IHttpActionResult> PostFinancialInstitutionAdminEmail(FinancialInstitutionAdminEmail financialInstitutionAdminEmail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.FinancialInstitutionAdminEmails.Add(financialInstitutionAdminEmail);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = financialInstitutionAdminEmail.FinancialInstitutionAdminEmailID }, financialInstitutionAdminEmail);
        }

        // DELETE: api/FinancialInstitutionAdminEmails/5
        [ResponseType(typeof(FinancialInstitutionAdminEmail))]
        public async Task<IHttpActionResult> DeleteFinancialInstitutionAdminEmail(int id)
        {
            FinancialInstitutionAdminEmail financialInstitutionAdminEmail = await db.FinancialInstitutionAdminEmails.FindAsync(id);
            if (financialInstitutionAdminEmail == null)
            {
                return NotFound();
            }

            db.FinancialInstitutionAdminEmails.Remove(financialInstitutionAdminEmail);
            await db.SaveChangesAsync();

            return Ok(financialInstitutionAdminEmail);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FinancialInstitutionAdminEmailExists(int id)
        {
            return db.FinancialInstitutionAdminEmails.Count(e => e.FinancialInstitutionAdminEmailID == id) > 0;
        }
    }
}