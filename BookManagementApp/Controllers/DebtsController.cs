using System;
using System.Web.Mvc;
using System.Net;
using System.Collections.Generic;
using System.Linq;

using DAL.Entities;
using Services;

namespace BookManagementApp.Controllers
{
    public class DebtsController : Controller
    {
        DebtService debtServ = new DebtService();
        PublisherService publisherServ = new PublisherService();
        BookDebtService bookDebtServ = new BookDebtService();

        public ActionResult Index()
        {
            ViewBag.publishers = new SelectList(publisherServ.GetAll(), "ID", "Name");

            return View();
        }

        [HttpPost]
        public ActionResult Index(int? publisherID)
        {
            ViewBag.publishers = new SelectList(publisherServ.GetAll(), "ID", "Name");

            List<BookDebt> bookDebts = new List<BookDebt>();

            if (!string.IsNullOrWhiteSpace(Request.Form["publisherID"]))
            {
                publisherID = Convert.ToInt32(Request.Form["publisherID"]);

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

                // get  total debt 
                Debt debt = debtServ
                    .GetByTime(Convert.ToInt32(publisherID), date);
                if (debt != null)
                    ViewBag.total = debt.Amount;
                else ViewBag.total = 0;

                // get  books debt
                bookDebts = bookDebtServ.GetByTime(Convert.ToInt32(publisherID), date);
               
            }

            return View(bookDebts);
        }
    }
}
