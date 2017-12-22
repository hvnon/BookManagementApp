using System;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;

using DAL.Entities;

namespace DAL
{
    public class BookDebtDAO
    {
        private BookContext db = new BookContext();

        // By default: 
        //  + every get method has 'publisherID' as parameter
        //  + if time is not specified => get latest
        //  + if bookID is not specified => get all books

        public BookDebt GetByBookID(int publisherID, int bookID)
        {
            return db.BookDebts
                            .Where(s => s.BookID == bookID && s.PublisherID == publisherID)
                            .OrderByDescending(s => s.Date)
                            .FirstOrDefault();
        }

        public List<BookDebt> GetByTime(int publisherID, DateTime date)
        {
            return db.BookDebts.Where(s => s.PublisherID == publisherID
                                && DbFunctions.TruncateTime(s.Date) <= date)
                           .Include(s => s.Book)
                           .OrderByDescending(s => s.Date)
                           .ToList();
        }

        public void Add(BookDebt agencyBookDebt)
        {
            db.BookDebts.Add(agencyBookDebt);

            db.SaveChanges();
        }
    }
}
