using System;
using System.Web.Mvc;
using System.Net;

using DAL.Entities;
using Services;
using System.Collections.Generic;
using System.Linq;

namespace BookManagementApp.Controllers
{
    public class InvoicesController : Controller
    {
        InvoiceService invoiceServ = new InvoiceService();
        InvoiceDetailService invoiceDetailServ = new InvoiceDetailService();
        StockService stockServ = new StockService();
        AgencyDebtService agencyDebtServ = new AgencyDebtService();
        AgencyBookDebtService agencyBookDebtServ = new AgencyBookDebtService();
        AgencyService agencyServ= new AgencyService();
        BookService bookServ = new BookService();

        // GET: Invoices
        public ActionResult Index()
        {
            return View(invoiceServ.GetAll());
        }

        // GET: Invoices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int invoiceID = Convert.ToInt32(id);
            Invoice invoice = invoiceServ.GetByID(invoiceID);

            if (invoice == null)
            {
                return HttpNotFound();
            }

            return View(invoice);
        }

        // GET: Invoices/Create
        public ActionResult Create()
        {
            ViewBag.agencies = new SelectList(agencyServ.GetAll(), "ID", "Name");
            ViewBag.currentDate = DateTime.Now.ToString("dd/MM/yyyy h:mm:ss");

            return View();
        }

        [HttpPost]
        public ActionResult CreateInvoice()
        {
            Session.Clear();

            Agency agency = agencyServ.GetByID(Convert.ToInt32(Request.Form["agencyID"].ToString()));

            Invoice invoice = new Invoice
            {
                ID = invoiceServ.GenerateID(),
                AgencyID = agency.ID,
                Total = 0,
                Date = DateTime.Now,
                Description = Request.Form["Description"].ToString(),
                Status = true,
                Agency = agency
            };

            Session["invoice"] = invoice;

            return RedirectToAction("ShowInvoiceDetails");
        }

        public ActionResult ShowInvoiceDetails()
        {
            if (Session["invoice"] != null)
            {
                // show invoice info
                Invoice invoice = (Session["invoice"] as Invoice);
                ViewBag.invoiceInfo = invoice;

                // show books 
                ViewBag.books = new SelectList(bookServ.GetAll(), "ID", "Name");

                // show invoice details
                List<InvoiceDetail> invoiceDetails = new List<InvoiceDetail>();
                if (Session["invoiceDetails"] != null)
                    invoiceDetails = (Session["invoiceDetails"] as List<InvoiceDetail>);
                else Session["invoiceDetails"] = invoiceDetails;

                if(Session["errorMessage"] != null)
                {                   
                    ViewBag.errorMessage = (Session["errorMessage"] as String);
                    ViewBag.errorInfo = (Session["errorInfo"] as String);
                }

                return View(invoiceDetails);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddUpdateInvoiceDetails()
        {
            if (Session["invoice"] != null)
            {
                Session["errorMessage"] = null;
                Session["errorInfo"] = null;

                Invoice invoice = (Session["invoice"] as Invoice);

                List<InvoiceDetail> invoiceDetails = (Session["invoiceDetails"] as List<InvoiceDetail>);

                // get latest debt of agency
                int agencyDebtAmount = 0;
                AgencyDebt agencyDebt = agencyDebtServ.GetByTime(invoice.AgencyID, DateTime.Now);
                if(agencyDebt != null)
                    agencyDebtAmount = agencyDebt.Amount;

                // if user has chosen a book from books list
                if (!string.IsNullOrWhiteSpace(Request.Form["bookID"]))
                {
                    int bookID;
                    Int32.TryParse(Request.Form["bookID"].ToString(), out bookID);

                    var invoiceDetail = invoiceDetails
                                               .Where(s => s.BookID == bookID)
                                               .FirstOrDefault();

                    Book book = bookServ.GetByID(bookID);
                    Stock stock = stockServ.GetByTimeAndBookID(DateTime.Now, bookID)
                        .FirstOrDefault();
                    
                    if ( (invoiceDetail == null && stock.Quantity > 0) 
                        || (invoiceDetail != null && stock.Quantity >= invoiceDetail.Quantity) 
                       )
                    {
                        // check if agency debt is greater than total of creating invoice
                        if (agencyDebtAmount > invoice.Total || agencyDebtAmount == 0)
                        {
                            if (invoiceDetail != null)
                            {
                                // if chosen book already exists, increase its quantity by one
                                invoiceDetail.Quantity++;
                            }
                            else
                            {
                                // if chosen book not exists, add new
                                InvoiceDetail a = new InvoiceDetail
                                {
                                    InvoiceID = invoice.ID,
                                    BookID = bookID,
                                    Quantity = 1,
                                    UnitPrice = book.SellingPrice,
                                    Book = book
                                };
                                invoiceDetails.Add(a);
                            }
                        }
                        else
                        {
                            Session["errorMessage"] = "Tổng tiền vượt quá số tiền còn nợ! ";
                            Session["errorInfo"] = agencyDebtAmount.ToString();
                        }
                    }
                    else
                    {
                        Session["errorMessage"] = "Không đủ sách! ";
                        Session["errorInfo"] = stock.Quantity.ToString();
                    }
                }
                // if user doesn't choose a book
                else
                {
                    if (Session["invoiceDetails"] != null)
                    // if this is not a newly created invoice with no details
                    {
                        for (int i = 0; i < invoiceDetails.Count; i++)
                        {
                            int bookID = Convert.ToInt32(Request.Form["invoiceDetail_" + i]);
                            int quantity = Convert.ToInt32(Request.Form["quantity_" + i]);

                            Book book = bookServ.GetByID(bookID);
                            
                            if (quantity > 0)
                            {
                                Stock stock = stockServ.GetByTimeAndBookID(DateTime.Now, bookID)
                                    .FirstOrDefault();
                                if (stock.Quantity >= quantity)
                                {
                                    if (agencyDebtAmount > (invoice.Total + quantity * book.SellingPrice)
                                        || agencyDebtAmount == 0)
                                    {
                                        InvoiceDetail a = invoiceDetails
                                            .Where(s => s.BookID == bookID).FirstOrDefault();

                                        a.Quantity = quantity;
                                    }
                                    else
                                    {
                                        Session["errorMessage"] = "Tổng tiền vượt quá số tiền còn nợ! ";
                                        Session["errorInfo"] = agencyDebtAmount.ToString();
                                    }
                                }
                                else
                                {
                                    Session["errorMessage"] = "Không đủ sách! ";
                                    Session["errorInfo"] = stock.Quantity.ToString();
                                }
                            }

                        }
                    }
                }

                int total = 0;
                foreach (var item in invoiceDetails)
                {
                    total += item.Quantity * item.UnitPrice;
                }

                invoice.Total = total;

                return RedirectToAction("ShowInvoiceDetails");
            }

            return RedirectToAction("Index");
        }

        public ActionResult DeleteDetail(int bookID)
        {
            List<InvoiceDetail> invoiceDetails = (Session["invoiceDetails"] as List<InvoiceDetail>);
            InvoiceDetail a = invoiceDetails.Where(s => s.BookID == bookID).FirstOrDefault();
            invoiceDetails.Remove(a);

            return RedirectToAction("ShowInvoiceDetails");
        }

        public ActionResult DeleteInvoice(int invoiceID)
        {
            Session.Clear();

            return RedirectToAction("Index");
        }

        public ActionResult Save(int invoiceID, int agencyID)
        {
            // add invoice
            Invoice tempInvoice = (Session["invoice"] as Invoice);
            Invoice invoice = new Invoice()
            {
                AgencyID = tempInvoice.AgencyID,
                Total = tempInvoice.Total,
                Date = tempInvoice.Date,
                Description = tempInvoice.Description,
                Status = tempInvoice.Status
            };
            invoiceServ.Add(invoice);

            int totalAgencyDebt = 0;

            List<InvoiceDetail> tempInvoiceDetails = (Session["invoiceDetails"] as List<InvoiceDetail>);
            List<InvoiceDetail> invoiceDetails = new List<InvoiceDetail>();
            foreach (var item in tempInvoiceDetails)
            {
                InvoiceDetail x = new InvoiceDetail()
                {
                    InvoiceID = item.InvoiceID,
                    BookID = item.BookID,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                };
                invoiceDetails.Add(x);
            }

            // add invoice details & update stock & update agency book debt
            foreach (InvoiceDetail i in invoiceDetails)
            {
                invoiceDetailServ.Add(i);

                stockServ.Add(i.BookID, -i.Quantity);// decrease quantity 

                agencyBookDebtServ.Add(agencyID, i.BookID, i.Quantity);

                totalAgencyDebt += (i.Quantity * i.UnitPrice);
            }

            // update debt
            agencyDebtServ.Add(agencyID, totalAgencyDebt);

            Session.Clear();

            return RedirectToAction("Index");
        }

    }
}
