using System;
using System.Web.Mvc;
using System.Net;
using System.Collections.Generic;
using System.Linq;

using DAL.Entities;
using Services;

namespace BookManagementApp.Controllers
{
    public class ReportsController : Controller
    {
        ReportService reportServ = new ReportService();
        ReportDetailService reportDetailServ = new ReportDetailService();
        PublisherService publisherServ = new PublisherService();
        StockService stockServ = new StockService();
        DebtService debtServ = new DebtService();
        BookDebtService bookDebtServ = new BookDebtService();
        BookService bookServ = new BookService();

        // GET: Reports
        public ActionResult Index()
        {
            return View(reportServ.GetAll());
        }

        // GET: Reports/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int reportID = Convert.ToInt32(id);
            ViewBag.reportInfo = reportServ.GetByID(reportID);

            return View(reportDetailServ.GetByReportID(reportID));
        }

        // GET: Reports/Create
        public ActionResult Create()
        {
            ViewBag.publishers = new SelectList(publisherServ.GetAll(), "ID", "Name");
            ViewBag.currentDate = DateTime.Now.ToString("dd/MM/yyyy h:mm:ss");

            return View();
        }

        [HttpPost]
        public ActionResult CreateReport()
        {
            Session.Clear();

            Publisher publisher = 
                publisherServ.GetByID(Convert.ToInt32(Request.Form["publisherID"].ToString()));

            Report report = new Report
            {
                ID = reportServ.GenerateID(),
                PublisherID = publisher.ID,
                Total = 0,
                Date = DateTime.Now,
                Description = Request.Form["Description"].ToString(),
                Status = true,
                Publisher = publisher
            };

            Session["report"] = report;

            return RedirectToAction("ShowReportDetails");
        }

        public ActionResult ShowReportDetails()
        {
            if (Session["report"] != null)
            {
                // show report info
                Report report = (Session["report"] as Report);
                ViewBag.reportInfo = report;
                int publisherID = Convert.ToInt32(report.PublisherID);

                // show books 
                ViewBag.publisherBooks = new
                    SelectList(bookServ.GetByPublisherID(publisherID), "ID", "Name");

                // show report details
                List<ReportDetail> reportDetails = new List<ReportDetail>();
                if (Session["reportDetails"] != null)
                    reportDetails = (Session["reportDetails"] as List<ReportDetail>);
                else Session["reportDetails"] = reportDetails;

                if (Session["errorMessage"] != null)
                {
                    ViewBag.errorMessage = (Session["errorMessage"] as String);
                }

                return View(reportDetails);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddUpdateReportDetails()
        {
            if (Session["report"] != null)
            {
                Session["errorMessage"] = null;

                Report report = (Session["report"] as Report);

                List<ReportDetail> reportDetails = (Session["reportDetails"] as List<ReportDetail>);

                // if user has chosen a book from books list
                if (!string.IsNullOrWhiteSpace(Request.Form["bookID"]))
                {
                    int bookID = 0;
                    Int32.TryParse(Request.Form["bookID"].ToString(), out bookID);

                    var reportDetail = reportDetails
                                                .Where(s => s.BookID == bookID)
                                                .FirstOrDefault();


                    BookDebt bookDebt = bookDebtServ.GetByBookID(report.PublisherID, bookID);
                    int debtQuantity = bookDebt.Quantity;

                    if (reportDetail != null)
                    {
                        // if chosen book already exists, increase its quantity by one
                        if ((reportDetail.Quantity + 1) <= debtQuantity)
                        {
                            reportDetail.Quantity++;
                        }
                        else
                        {
                            Session["errorMessage"] = "Số lượng báo cáo vượt quá số lượng còn nợ!" +
                                " Số nợ: " + debtQuantity + " cuốn";
                        }

                    }
                    else
                    {
                        if (debtQuantity != 0)
                        {
                            // if chosen book not exists, add new
                            Book book = bookServ.GetByID(bookID);
                            ReportDetail a = new ReportDetail
                            {
                                ReportID = report.ID,
                                BookID = bookID,
                                Quantity = 1,
                                UnitPrice = book.SellingPrice,
                                Book = book
                            };

                            reportDetails.Add(a);
                        }
                        else
                            Session["errorMessage"] = "Số lượng báo cáo vượt quá số lượng còn nợ!" +
                                " Số nợ: " + debtQuantity + " cuốn";
                    }
                }
                // if user doesn't choose a book
                else
                {
                    if (Session["reportDetails"] != null)
                    // if this is not a newly created report with no details
                    {
                        for (int i = 0; i < reportDetails.Count; i++)
                        {
                            int bookID = Convert.ToInt32(Request.Form["reportDetail_" + i]);
                            int quantity = Convert.ToInt32(Request.Form["quantity_" + i]);

                            if (quantity > 0)
                            {
                                BookDebt bookDebt =
                                    bookDebtServ.GetByBookID(report.PublisherID, bookID);
                                int debtQuantity = bookDebt.Quantity;

                                if (quantity <= debtQuantity)
                                {
                                    ReportDetail a = reportDetails
                                        .Where(s => s.BookID == bookID).FirstOrDefault();

                                    a.Quantity = quantity;
                                }
                                else
                                    Session["errorMessage"] = "Số lượng báo cáo vượt quá số lượng còn nợ!" +
                                        " Số nợ: " + debtQuantity + " cuốn";
                            }

                        }
                    }
                }

                int total = 0;
                foreach (var item in reportDetails)
                {
                    total += item.Quantity * item.UnitPrice;
                }

                report.Total = total;

                return RedirectToAction("ShowReportDetails");
            }

            return RedirectToAction("Index");
        }

        public ActionResult DeleteReportDetail(int bookID)
        {
            List<ReportDetail> reportDetails = (Session["reportDetails"] as List<ReportDetail>);
            ReportDetail a = reportDetails.Where(s => s.BookID == bookID).FirstOrDefault();
            reportDetails.Remove(a);

            return RedirectToAction("ShowReportDetails");
        }

        public ActionResult DeleteReport()
        {
            Session.Clear();

            return RedirectToAction("Index");
        }

        public ActionResult Save()
        {
            // add report to database
            Report tempReport = (Session["report"] as Report);
            Report report = new Report()
            {
                ID = tempReport.ID,
                PublisherID = tempReport.PublisherID,
                Total = tempReport.Total,
                Date = tempReport.Date,
                Description = tempReport.Description,
                Status = tempReport.Status
            };
            reportServ.Add(report);

            List<ReportDetail> tempReportDetails = (Session["reportDetails"] as List<ReportDetail>);
            List<ReportDetail> reportDetails = new List<ReportDetail>();
            foreach (var item in tempReportDetails)
            {
                ReportDetail x = new ReportDetail()
                {
                    ReportID = item.ReportID,
                    BookID = item.BookID,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                };
                reportDetails.Add(x);
            }

            // add report details & update book debt to database
            foreach (ReportDetail i in reportDetails)
            {
                reportDetailServ.Add(i);

                bookDebtServ.Add(report.PublisherID, i.BookID, -i.Quantity);
            }

            // update debt
            debtServ.Add(report.PublisherID, -report.Total);

            Session.Clear();

            return RedirectToAction("Index");
        }
    }
}
