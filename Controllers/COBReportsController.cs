using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using E_Pay_Web_API.Models;
using System.Web.Http.Cors;
using E_Pay_Web_API.Helpers;

namespace E_Pay_Web_API.Controllers
{
    [RoutePrefix("api/COBReports")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class COBReportsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /*
        [Route("SettlementTransfers")]
        public IEnumerable<E_Pay_Web_API.Models.Transfer> GetSettlementTransfers()
        {
            return db.Transfers.Where(p => p.SettlementRequired);
        }

        [Route("SettlementsByDate")]
        public IEnumerable<E_Pay_Web_API.Models.Transfer> GetSettlementsByDate(DateTime settlementDate)
        {
            DateTime dayAfter = settlementDate.AddDays(1);
            var settlements = db.Transfers.Where(p => p.SettlementRequired && p.Created >= settlementDate && p.Created < dayAfter);

            return settlements;
        }

        [Route("TotalSettlementByDate")]
        public decimal GetTotalSettlementByDate(DateTime settlementDate)
        {
            DateTime dayAfter = settlementDate.AddDays(1);
            decimal totalSettlement = db.Transfers.Where(p => p.SettlementRequired && p.Created >= settlementDate && p.Created < dayAfter).Sum(p => p.TransferAmount);

            return totalSettlement;
        }

        [Route("TotalSettlementByDate")]
        public IEnumerable<DailyTotal> GetTotalSettlementByDate(DateTime settlementFromDate, DateTime settlementToDate)
        {
            settlementToDate = settlementToDate.AddDays(1);
            var dailySettlements = from p in db.Transfers
                                      where
                                      p.SettlementRequired
                                      && p.Created > settlementFromDate
                                      && p.Created < settlementToDate
                                      group p by new { p.Created.Year, p.Created.Month, p.Created.Day } into q
                                      select new DailyTotal
                                      {
                                          Year = q.Key.Year,
                                          Month = q.Key.Month,
                                          Day = q.Key.Day,
                                          Transfers = q.Count(),
                                          Total = q.Sum(r => r.TransferAmount)
                                      };

            return dailySettlements;
        }
        */
    }
}
