using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using E_Pay_Web_API.Models;
using System.Web.Http.Cors;

namespace E_Pay_Web_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class COBSecurityQuestionsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Route("api/COBSecurityQuestions")]
        public IQueryable<COBSecurityQuestion> GetCOBSecurityQuestions()
        {
            return db.COBSecurityQuestions.Where(p=>p.Active);
        }
    }
}
