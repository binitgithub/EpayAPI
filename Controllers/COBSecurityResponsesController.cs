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

namespace E_Pay_Web_API.Controllers
{
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class COBSecurityResponsesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/COBSecurityResponses
        public IQueryable<COBSecurityResponse> GetCOBSecurityResponses()
        {
            string userId = RequestContext.Principal.Identity.GetUserId();
            return db.COBSecurityResponses.Where(p=>p.UserID == userId).OrderBy(p=> Guid.NewGuid()).Take(3);
        }

        // GET: api/COBSecurityResponses/5
        [ResponseType(typeof(COBSecurityResponse))]
        public IHttpActionResult GetCOBSecurityResponse(int id)
        {
            COBSecurityResponse cOBSecurityResponse = db.COBSecurityResponses.Find(id);
            if (cOBSecurityResponse == null)
            {
                return NotFound();
            }

            return Ok(cOBSecurityResponse);
        }

        public IQueryable<COBSecurityResponse> GetCOBSecurityResponse(string userId)
        {
            return db.COBSecurityResponses.Where(p => p.UserID == userId);
        }

        // PUT: api/COBSecurityResponses/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCOBSecurityResponse(int id, COBSecurityResponse cOBSecurityResponse)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cOBSecurityResponse.COBSecurityResponseID)
            {
                return BadRequest();
            }

            db.Entry(cOBSecurityResponse).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!COBSecurityResponseExists(id))
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

        // POST: api/COBSecurityResponses
        [ResponseType(typeof(COBSecurityResponse))]
        public IHttpActionResult PostCOBSecurityResponse(COBSecurityResponse cOBSecurityResponse)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.COBSecurityResponses.Add(cOBSecurityResponse);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = cOBSecurityResponse.COBSecurityResponseID }, cOBSecurityResponse);
        }

        [Route("api/COBSecurityResponseArray")]
        public IHttpActionResult PostCOBSecurityResponseArray(COBSecurityResponse_Light[] cOBSecurityResponses)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (COBSecurityResponse_Light cobSecurityResponse in cOBSecurityResponses)
            {
                db.COBSecurityResponses.Add(new COBSecurityResponse
                {
                    UserID = RequestContext.Principal.Identity.GetUserId(),
                    QuestionText = cobSecurityResponse.QuestionText,
                    ResponseText = cobSecurityResponse.ResponseText,
                    Created = DateTime.Now
                });
            }
            db.SaveChanges();
            return Ok();
        }

        // DELETE: api/COBSecurityResponses/5
        [ResponseType(typeof(COBSecurityResponse))]
        public IHttpActionResult DeleteCOBSecurityResponse(int id)
        {
            COBSecurityResponse cOBSecurityResponse = db.COBSecurityResponses.Find(id);
            if (cOBSecurityResponse == null)
            {
                return NotFound();
            }

            db.COBSecurityResponses.Remove(cOBSecurityResponse);
            db.SaveChanges();

            return Ok(cOBSecurityResponse);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool COBSecurityResponseExists(int id)
        {
            return db.COBSecurityResponses.Count(e => e.COBSecurityResponseID == id) > 0;
        }
    }
}