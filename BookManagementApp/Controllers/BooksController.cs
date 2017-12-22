using System;
using System.Web.Mvc;
using System.Net;

using DAL.Entities;
using Services;

namespace BookManagementApp.Controllers
{
    public class BooksController : Controller
    {
        GenreService genreServ = new GenreService();
        PublisherService publisherServ = new PublisherService();
        BookService bookServ = new BookService();
        StockService stockServ = new StockService();

        // GET: Books
        public ActionResult Index()
        {
             return View(bookServ.GetAll());
        }

        
        // GET: Books/Create
        public ActionResult Create()
        {
            ViewBag.GenreID = new SelectList(genreServ.GetAll(), "ID", "Name");
            ViewBag.PublisherID = new SelectList(publisherServ.GetAll(), "ID", "Name");

            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PublisherID,GenreID,Name,Author,PublicationDate,SellingPrice,PurchasePrice")] Book book)
        {
            if (ModelState.IsValid)
            {
                bookServ.Add(book);

                stockServ.Add(book.ID, 0);

                return RedirectToAction("Index");
            }

            ViewBag.GenreID = new SelectList(genreServ.GetAll(), "ID", "Name", book.GenreID);
            ViewBag.PublisherID = new SelectList(publisherServ.GetAll(), "ID", "Name", book.PublisherID);

            return View(book);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int bookID = Convert.ToInt32(id);
            Book book = bookServ.GetByID(bookID);
            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.GenreID = new SelectList(genreServ.GetAll(), "ID", "Name", book.GenreID);
            ViewBag.PublisherID = new SelectList(publisherServ.GetAll(), "ID", "Name", book.PublisherID);

            return View(book);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PublisherID,GenreID,Name,Author,PublicationDate,SellingPrice,PurchasePrice")] Book book)
        {
            if (ModelState.IsValid)
            {
                bookServ.Update(book);

                return RedirectToAction("Index");
            }
            ViewBag.GenreID = new SelectList(genreServ.GetAll(), "ID", "Name", book.GenreID);
            ViewBag.PublisherID = new SelectList(publisherServ.GetAll(), "ID", "Name", book.PublisherID);

            return View(book);
        }
    }
}
