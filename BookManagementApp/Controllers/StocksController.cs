using System;
using System.Web.Mvc;
using System.Net;
using System.Collections.Generic;
using System.Linq;

using DAL.Entities;
using Services;

namespace BookManagementApp.Controllers
{
    public class StocksController : Controller
    {
        StockService stockServ = new StockService();
        BookService bookServ = new BookService();

        public ActionResult Index()
        {
            ViewBag.Books = new SelectList(bookServ.GetAll(), "ID", "Name");

            return View(stockServ.GetByTimeAndBookID(DateTime.Now, 0));
        }

        public ActionResult StockFilter(FormCollection form)
        {
            ViewBag.Books = new SelectList(bookServ.GetAll(), "ID", "Name");

            string tempBookID = Request.Form["Books"].ToString();
            int bookID = 0;
            if (tempBookID != "")
                bookID = Convert.ToInt32(tempBookID);

            DateTime date = DateTime.Now;
            if(!string.IsNullOrWhiteSpace(Request.Form["FilterDate"]))
            {
                var tempDate = Request.Form["FilterDate"].ToString();
                TimeSpan time = new TimeSpan(23, 59, 59);
                date = DateTime.Parse(tempDate).Add(time);
            }           

            ViewBag.chosenDate = date;

            List<Stock> stockResult = new List<Stock>();

            if (bookID != 0)
            {
                stockResult = stockServ.GetByTimeAndBookID(date, bookID);

                ViewBag.chosenBook = bookServ.GetByID(bookID).Name;
            }
            else
            {
                stockResult = stockServ.GetByTimeAndBookID(date, 0);

                ViewBag.chosenBook = "Tất cả sách";
            }

            return View(stockResult);
        }
    }
}