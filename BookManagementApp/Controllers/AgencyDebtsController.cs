using System;
using System.Web.Mvc;
using System.Net;
using System.Collections.Generic;
using System.Linq;

using DAL.Entities;
using Services;

namespace BookManagementApp.Controllers
{
    public class AgencyDebtsController : Controller
    {
        AgencyDebtService agencyDebtServ = new AgencyDebtService();
        AgencyService agencyServ = new AgencyService();
        AgencyBookDebtService agencyBookDebtServ = new AgencyBookDebtService();

        public ActionResult Index()
        {
            ViewBag.agencies = new SelectList(agencyServ.GetAll(), "ID", "Name");

            return View();
        }

        [HttpPost]
        public ActionResult Index(int? agencyID)
        {
            ViewBag.agencies = new SelectList(agencyServ.GetAll(), "ID", "Name");

            List<AgencyBookDebt> agencyBookDebts = new List<AgencyBookDebt>();

            if (!string.IsNullOrWhiteSpace(Request.Form["agencyID"]))
            {
                agencyID = Convert.ToInt32(Request.Form["agencyID"]);

                DateTime date;

                if (!string.IsNullOrWhiteSpace(Request.Form["date"]))
                {
                    var tempDate = Request.Form["date"].ToString();
                    TimeSpan time = new TimeSpan(23, 59, 59);
                    date = DateTime.Parse(tempDate).Add(time);
                    ViewBag.date = date;
                }
                else
                {
                    date = DateTime.Now;
                    ViewBag.date = "Hôm nay";
                }
                   
                // get agency's total debt 
                AgencyDebt agencyDebt = agencyDebtServ
                    .GetByTime(Convert.ToInt32(agencyID), date);
                if (agencyDebt != null)
                    ViewBag.total = agencyDebt.Amount;
                else ViewBag.total = 0;

                // get agency books debt
                agencyBookDebts = agencyBookDebtServ.GetByTime(Convert.ToInt32(agencyID), date);

            }

            return View(agencyBookDebts);
        }
    }
}
