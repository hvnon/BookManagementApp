using System;
using System.Collections.Generic;
using System.Linq;

using DAL;
using DAL.Entities;

namespace Services
{
    public class StockService
    {
        StockDAO stockDAO = new StockDAO();
        BookDAO bookDAO = new BookDAO();

        public List<Stock> GetByTimeAndBookID(DateTime date, int bookID)
        {
            bool bookNotChosen = false;
            if (bookID == 0)
                bookNotChosen = true;

            List<Book> books = bookDAO.GetAll();
            List<Stock> stocks = stockDAO.GetByTimeAndBookID(date, bookID, bookNotChosen);
            List<Stock> stockResult = new List<Stock>();

            foreach (var b in books)
            {
                Stock stock = stocks
                    .Where(s => s.BookID == b.ID)
                    .OrderByDescending(s => s.Date)
                    .FirstOrDefault();
                if(stock != null)
                    stockResult.Add(stock);
            }

            return stockResult;
        }      

        public void Add(int bookID, int quantity)
        {
            List<Stock> stocks = stockDAO.GetByTimeAndBookID(DateTime.Now, bookID, false);
            Stock stock = new Stock();

            if (stocks.Count == 0)
            {
                stock.BookID = bookID;
                stock.Quantity = quantity;
                stock.Date = DateTime.Now;
            }
            else
            {
                stock = stocks.FirstOrDefault();
                stock.Quantity += quantity;
                stock.Date = DateTime.Now;
            }
            

            stockDAO.Add(stock);
        }
    }
}
