using System;
using System.Web.Mvc;
using System.Net;
using System.Collections.Generic;
using System.Linq;

using DAL.Entities;
using Services;

namespace BookManagementApp.Controllers
{
    public class ReceiptsController : Controller
    {
        ReceiptService receiptServ = new ReceiptService();
        ReceiptDetailService receiptDetailServ = new ReceiptDetailService();
        StockService stockServ = new StockService();
        DebtService debtServ = new DebtService();
        BookDebtService bookDebtServ = new BookDebtService();
        PublisherService publisherServ = new PublisherService();
        BookService bookServ = new BookService();

        // GET: Receipts
        public ActionResult Index()
        {
            return View(receiptServ.GetAll());
        }

        // GET: Receipts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int receiptID = Convert.ToInt32(id);
            Receipt receipt = receiptServ.GetByID(receiptID);
            if (receipt == null)
            {
                return HttpNotFound();
            }

            return View(receipt);
        }

        // GET: Receipts/Create
        public ActionResult Create()
        {
            ViewBag.publishers = new SelectList(publisherServ.GetAll(), "ID", "Name");
            ViewBag.currentDate = DateTime.Now.ToString("dd/MM/yyyy h:mm:ss");

            return View();
        }

        [HttpPost]
        public ActionResult CreateReceipt()
        {
            Session.Clear();

            Publisher publisher = 
                publisherServ.GetByID(Convert.ToInt32(Request.Form["publisherID"].ToString()));

            Receipt receipt = new Receipt
            {
                ID = receiptServ.GenerateID(),
                PublisherID = publisher.ID,
                Total = 0,
                Date = DateTime.Now,
                Description = Request.Form["Description"].ToString(),
                Status = true,
                Publisher = publisher
            };

            Session["receipt"] = receipt;

            return RedirectToAction("ShowReceiptDetails");
        }

        public ActionResult ShowReceiptDetails()
        {
            if (Session["receipt"] != null)
            {
                // show receipt info
                Receipt receipt = (Session["receipt"] as Receipt);
                ViewBag.receiptInfo = receipt;

                // show books 
                ViewBag.books = 
                    new SelectList(bookServ.GetByPublisherID(Convert.ToInt32(receipt.PublisherID))
                    , "ID", "Name");

                // show receipt details
                List<ReceiptDetail> receiptDetails = new List<ReceiptDetail>();
                if (Session["receiptDetails"] != null)
                    receiptDetails = (Session["receiptDetails"] as List<ReceiptDetail>);
                else Session["receiptDetails"] = receiptDetails;

                if(Session["errorMessage"] != null)
                {                   
                    ViewBag.errorMessage = (Session["errorMessage"] as String);
                    ViewBag.errorInfo = (Session["errorInfo"] as String);
                }

                return View(receiptDetails);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddUpdateReceiptDetails()
        {
            if (Session["receipt"] != null)
            {
                Receipt receipt = (Session["receipt"] as Receipt);

                List<ReceiptDetail> receiptDetails = (Session["receiptDetails"] as List<ReceiptDetail>);

                // if user has chosen a book from books list
                if (!string.IsNullOrWhiteSpace(Request.Form["bookID"]))
                {
                    int bookID;
                    Int32.TryParse(Request.Form["bookID"].ToString(), out bookID);

                    var receiptDetail = receiptDetails.Where(s => s.BookID == bookID)
                        .FirstOrDefault();
                    if (receiptDetail != null)
                    {
                        // if chosen book already exists, increase its quantity by one
                        receiptDetail.Quantity++;

                    }
                    else
                    {
                        // if chosen book not exists, add new
                        Book book = bookServ.GetByID(bookID);
                        ReceiptDetail a = new ReceiptDetail
                        {
                            ReceiptID = receipt.ID,
                            BookID = bookID,
                            Quantity = 1,
                            UnitPrice = book.PurchasePrice,
                            Book = book
                        };
                        receiptDetails.Add(a);
                    }
                }
                // if user doesn't choose a book
                else
                {
                    if (Session["receiptDetails"] != null)
                    // if this is not a newly created receipt with no details
                    {
                        for (int i = 0; i < receiptDetails.Count; i++)
                        {
                            int bookID = Convert.ToInt32(Request.Form["receiptDetail_" + i]);
                            int quantity = Convert.ToInt32(Request.Form["quantity_" + i]);

                            if (quantity > 0)
                            {
                                ReceiptDetail a = receiptDetails.Where(s => s.BookID == bookID).FirstOrDefault();

                                a.Quantity = quantity;
                            }

                        }
                    }
                }

                int total = 0;
                foreach (var item in receiptDetails)
                {
                    total += item.Quantity * item.UnitPrice;
                }

                receipt.Total = total;

                return RedirectToAction("ShowReceiptDetails");
            }

            return RedirectToAction("Index");
        }

        public ActionResult DeleteDetail(int bookID)
        {
            List<ReceiptDetail> receiptDetails = (Session["receiptDetails"] as List<ReceiptDetail>);
            ReceiptDetail a = receiptDetails.Where(s => s.BookID == bookID).FirstOrDefault();
            receiptDetails.Remove(a);

            return RedirectToAction("ShowReceiptDetails");
        }

        public ActionResult DeleteReceipt(int receiptID)
        {
            Session.Clear();

            return RedirectToAction("Index");
        }

        public ActionResult Save(int receiptID, int publisherID)
        {
            // add receipt
            Receipt tempReceipt = (Session["receipt"] as Receipt);
            Receipt receipt = new Receipt()
            {
                PublisherID = tempReceipt.PublisherID,
                Total = tempReceipt.Total,
                Date = tempReceipt.Date,
                Description = tempReceipt.Description,
                Status = tempReceipt.Status
            };
            receiptServ.Add(receipt);

            int totalDebt = 0;

            List<ReceiptDetail> tempReceiptDetails = (Session["receiptDetails"] as List<ReceiptDetail>);
            List<ReceiptDetail> receiptDetails = new List<ReceiptDetail>();
            foreach (var item in tempReceiptDetails)
            {
                ReceiptDetail x = new ReceiptDetail()
                {
                    ReceiptID = item.ReceiptID,
                    BookID = item.BookID,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                };
                receiptDetails.Add(x);
            }

            // add receipt details & update stock & update agency book debt
            foreach (ReceiptDetail i in receiptDetails)
            {
                receiptDetailServ.Add(i);

                stockServ.Add(i.BookID, i.Quantity);

                bookDebtServ.Add(publisherID, i.BookID, i.Quantity);

                totalDebt += (i.Quantity * i.UnitPrice);
            }

            // update debt
            debtServ.Add(publisherID, totalDebt);

            Session.Clear();

            return RedirectToAction("Index");
        }

    }
}
