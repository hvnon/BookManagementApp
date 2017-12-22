using System;
using System.Collections.Generic;
using System.Web.Mvc;

using DAL.Entities;
using Services;

namespace BookManagementApp.Controllers
{
    public class HomeController : Controller
    {
        AgencyService agencyServ = new AgencyService();
        AgencyReportService agencyReportServ = new AgencyReportService();
        AgencyReportDetailService agencyReportDetailServ = new AgencyReportDetailService();

        public ActionResult Index()
        {
            // if user doesn't choose filter date, set default date
            DateTime fromDate = new DateTime(1995,1,1);
            DateTime toDate = DateTime.Now;

            if (!string.IsNullOrWhiteSpace(Request.Form["toDate"]))
            {
                var tempDate = Request.Form["toDate"].ToString();
                TimeSpan time = new TimeSpan(23, 59, 59);
                toDate = DateTime.Parse(tempDate).Add(time);
                // add hour and minute to the end of date
            }
            ViewBag.fromDate = fromDate;
            ViewBag.toDate = toDate;

            // get total payments from agencies => revenue
            int totalPayment = 0;
            ViewBag.totalAgencyPayment = 0;
            ViewBag.revenue = 0;

            List<Agency> agencies = agencyServ.GetAll();
            ViewBag.agencies = agencies;
   
            int[] paymentList = new int[100]; int count = 0;
            int totalSellingPrice = 0;
            int totalPurchasePrice = 0;

            foreach (Agency agency in agencies)
            {
                List<AgencyReport> agencyReports = agencyReportServ.GetByTimeAndAgencyID(agency.ID, fromDate, toDate);

                if(agencyReports.Count != 0)
                {
                    foreach (var x in agencyReports)
                    {
                        paymentList[count] += x.Total;

                        totalPayment += x.Total;

                        List<AgencyReportDetail> agencyReportDetails =
                            agencyReportDetailServ.GetByAgencyReportID(x.ID);

                        foreach (AgencyReportDetail y in agencyReportDetails)
                        {
                            totalSellingPrice += (y.Book.SellingPrice * y.Quantity);
                            totalPurchasePrice += (y.Book.PurchasePrice * y.Quantity);
                        }
                    }
                }
                
                count++;
            }

            ViewBag.revenue = totalSellingPrice - totalPurchasePrice;
            ViewBag.totalPayment = totalPayment;

            return View(paymentList);
        }


    }
}