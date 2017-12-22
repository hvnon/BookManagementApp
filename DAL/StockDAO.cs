using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

using DAL.Entities;


namespace DAL
{
    public class StockDAO
    {
        private BookContext db = new BookContext();

        public List<Stock> GetByTimeAndBookID(DateTime date, int bookID, bool bookNotChosen)
        {
            return db.Stocks
                     .Include(s => s.Book)
                     .Where
                     (
                        s => (s.BookID == bookID || bookNotChosen)
                        && DbFunctions.TruncateTime(s.Date) <= date
                      )
                     .OrderByDescending(s => s.Date)
                     .ToList();
        }

        public void Add(Stock stock)
        {
            db.Stocks.Add(stock);

            db.SaveChanges();
        }
    }
}
