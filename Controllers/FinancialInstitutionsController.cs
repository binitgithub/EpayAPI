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

namespace E_Pay_Web_API.Controllers
{
    public class FinancialInstitutionsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/FinancialInstitutions
        public IQueryable<FinancialInstitution> GetFinancialInstitutions()
        {
            return db.FinancialInstitutions;
        }

        // GET: api/FinancialInstitutions/5
        [ResponseType(typeof(FinancialInstitution))]
        public IHttpActionResult GetFinancialInstitution(Guid id)
        {
            FinancialInstitution financialInstitution = db.FinancialInstitutions.Find(id);
            if (financialInstitution == null)
            {
                return NotFound();
            }

            return Ok(financialInstitution);
        }

        // PUT: api/FinancialInstitutions/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFinancialInstitution(Guid id, FinancialInstitution financialInstitution)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != financialInstitution.FinancialInstitutionID)
            {
                return BadRequest();
            }

            db.Entry(financialInstitution).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FinancialInstitutionExists(id))
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

        // POST: api/FinancialInstitutions
        [ResponseType(typeof(FinancialInstitution))]
        public IHttpActionResult PostFinancialInstitution(FinancialInstitution financialInstitution)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.FinancialInstitutions.Add(financialInstitution);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (FinancialInstitutionExists(financialInstitution.FinancialInstitutionID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = financialInstitution.FinancialInstitutionID }, financialInstitution);
        }

        // DELETE: api/FinancialInstitutions/5
        [ResponseType(typeof(FinancialInstitution))]
        public IHttpActionResult DeleteFinancialInstitution(Guid id)
        {
            FinancialInstitution financialInstitution = db.FinancialInstitutions.Find(id);
            if (financialInstitution == null)
            {
                return NotFound();
            }

            db.FinancialInstitutions.Remove(financialInstitution);
            db.SaveChanges();

            return Ok(financialInstitution);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FinancialInstitutionExists(Guid id)
        {
            return db.FinancialInstitutions.Count(e => e.FinancialInstitutionID == id) > 0;
        }
    }
}