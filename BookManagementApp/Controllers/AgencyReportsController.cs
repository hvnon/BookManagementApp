using System;
using System.Web.Mvc;
using System.Net;
using System.Collections.Generic;
using System.Linq;

using DAL.Entities;
using Services;

namespace BookManagementApp.Controllers
{
    public class AgencyReportsController : Controller
    {
        AgencyReportService agencyReportServ = new AgencyReportService();
        AgencyReportDetailService agencyReportDetailServ= new AgencyReportDetailService();
        AgencyService agencyServ = new AgencyService();
        StockService stockServ = new StockService();
        AgencyDebtService agencyDebtServ = new AgencyDebtService();
        AgencyBookDebtService agencyBookDebtServ = new AgencyBookDebtService();
        BookService bookServ = new BookService();

        // GET: AgencyReports
        public ActionResult Index()
        {
            return View(agencyReportServ.GetAll());
        }

        // GET: AgencyReports/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int agencyReportID = Convert.ToInt32(id);
            ViewBag.agencyReportInfo = agencyReportServ.GetByID(agencyReportID);
            
            return View(agencyReportDetailServ.GetByAgencyReportID(agencyReportID));
        }

        // GET: AgencyReports/Create
        public ActionResult Create()
        {
            ViewBag.agencies = new SelectList(agencyServ.GetAll(), "ID", "Name");
            ViewBag.currentDate = DateTime.Now.ToString("dd/MM/yyyy h:mm:ss");

            return View();
        }

        [HttpPost]
        public ActionResult CreateAgencyReport()
        {
            Session.Clear();

            Agency agency = agencyServ.GetByID(Convert.ToInt32(Request.Form["agencyID"].ToString()));

            AgencyReport agencyReport = new AgencyReport
            {
                ID = agencyReportServ.GenerateID(),
                AgencyID = agency.ID,
                Total = 0,
                Date = DateTime.Now,
                Description = Request.Form["Description"].ToString(),
                Status = true,
                Agency = agency
            };

            Session["agencyReport"] = agencyReport;

            return RedirectToAction("ShowAgencyReportDetails");
        }

        public ActionResult ShowAgencyReportDetails()
        {
            if (Session["agencyReport"] != null)
            {
                // show report info
                AgencyReport agencyReport = (Session["agencyReport"] as AgencyReport);
                ViewBag.agencyReportInfo = agencyReport;
                int agencyID = Convert.ToInt32(agencyReport.AgencyID);

                // show books agency has received
                ViewBag.agencyBooks = new 
                    SelectList(agencyBookDebtServ.GetReceivedBooks(agencyID), "ID", "Name");

                // show agency report details
                List<AgencyReportDetail> agencyReportDetails = new List<AgencyReportDetail>();
                if (Session["agencyReportDetails"] != null)
                    agencyReportDetails = (Session["agencyReportDetails"] as List<AgencyReportDetail>);
                else Session["agencyReportDetails"] = agencyReportDetails;

                if (Session["errorMessage"] != null)
                {
                    ViewBag.errorMessage = (Session["errorMessage"] as String);
                }

                return View(agencyReportDetails);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddUpdateAgencyReportDetails()
        {
            if (Session["agencyReport"] != null)
            {
                Session["errorMessage"] = null;

                AgencyReport agencyReport= (Session["agencyReport"] as AgencyReport);

                List<AgencyReportDetail> agencyReportDetails = (Session["agencyReportDetails"] as List<AgencyReportDetail>);

                // if user has chosen a book from books list
                if (!string.IsNullOrWhiteSpace(Request.Form["bookID"]))
                {
                    int bookID = 0;
                    Int32.TryParse(Request.Form["bookID"].ToString(), out bookID);

                    var agencyReportDetail = agencyReportDetails
                                                .Where(s => s.BookID == bookID)
                                                .FirstOrDefault();


                    AgencyBookDebt bookDebt = agencyBookDebtServ.GetByBookID(agencyReport.AgencyID, bookID);
                    int debtQuantity = bookDebt.Quantity;

                    if (agencyReportDetail != null)
                    {
                        // if chosen book already exists, increase its quantity by one
                        if ((agencyReportDetail.Quantity + 1) <= debtQuantity)
                        {
                            agencyReportDetail.Quantity++;
                        }
                        else {
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
                            AgencyReportDetail a = new AgencyReportDetail
                            {
                                AgencyReportID = agencyReport.ID,
                                BookID = bookID,
                                Quantity = 1,
                                UnitPrice = book.SellingPrice,
                                Book = book
                            };
 
                            agencyReportDetails.Add(a);
                        }
                        else
                            Session["errorMessage"] = "Số lượng báo cáo vượt quá số lượng còn nợ!" +
                                " Số nợ: " + debtQuantity + " cuốn";
                    }
                }
                // if user doesn't choose a book
                else
                {
                    if (Session["agencyReportDetails"] != null)
                    // if this is not a newly created report with no details
                    {
                        for (int i = 0; i < agencyReportDetails.Count; i++)
                        {
                            int bookID = Convert.ToInt32(Request.Form["agencyReportDetail_" + i]);
                            int quantity = Convert.ToInt32(Request.Form["quantity_" + i]);

                            if(quantity > 0)
                            {
                                AgencyBookDebt bookDebt = 
                                    agencyBookDebtServ.GetByBookID(agencyReport.AgencyID, bookID);
                                int debtQuantity = bookDebt.Quantity;

                                if (quantity <= debtQuantity)
                                {
                                    AgencyReportDetail a = agencyReportDetails
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
                foreach (var item in agencyReportDetails)
                {
                    total += item.Quantity * item.UnitPrice;
                }

                agencyReport.Total = total;

                return RedirectToAction("ShowAgencyReportDetails");
            }

            return RedirectToAction("Index");            
        }

        public ActionResult DeleteAgencyReportDetail(int bookID)
        {
            List<AgencyReportDetail> agencyReportDetails = (Session["agencyReportDetails"] as List<AgencyReportDetail>);
            AgencyReportDetail a = agencyReportDetails.Where(s => s.BookID == bookID).FirstOrDefault();
            agencyReportDetails.Remove(a);

            return RedirectToAction("ShowAgencyReportDetails");
        }

        public ActionResult DeleteAgencyReport()
        {
            Session.Clear();

            return RedirectToAction("Index");
        }

        public ActionResult Save()
        {
            // add agency report to database
            AgencyReport tempAgencyReport = (Session["agencyReport"] as AgencyReport);
            AgencyReport agencyReport = new AgencyReport()
            {
                AgencyID = tempAgencyReport.AgencyID,
                Total = tempAgencyReport.Total,
                Date = tempAgencyReport.Date,
                Description = tempAgencyReport.Description,
                Status = tempAgencyReport.Status
            };
            agencyReportServ.Add(agencyReport);

            List<AgencyReportDetail> tempAgencyReportDetails = (Session["agencyReportDetails"] as List<AgencyReportDetail>);
            List<AgencyReportDetail> agencyReportDetails = new List<AgencyReportDetail>();
            foreach (var item in tempAgencyReportDetails)
            {
                AgencyReportDetail x = new AgencyReportDetail()
                {
                    AgencyReportID = item.AgencyReportID,
                    BookID = item.BookID,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                };
                agencyReportDetails.Add(x);
            }

            // add agency report details & update angency book debt to database
            foreach (AgencyReportDetail i in agencyReportDetails)
            {
                agencyReportDetailServ.Add(i);

                agencyBookDebtServ.Add(agencyReport.AgencyID, i.BookID, -i.Quantity);
            }

            // update angency debt
            agencyDebtServ.Add(agencyReport.AgencyID, -agencyReport.Total);

            Session.Clear();

            return RedirectToAction("Index");
        }
    }
}
