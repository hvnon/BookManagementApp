using System;
using System.Collections.Generic;
using System.Linq;

using DAL;
using DAL.Entities;

namespace Services
{
    public class BookDebtService
    {
        BookDebtDAO bookDebtDAO = new BookDebtDAO();
        BookDAO bookDAO = new BookDAO();

        public BookDebt GetByBookID(int publisherID, int bookID)
        {
            return bookDebtDAO.GetByBookID(publisherID, bookID);
        }

        public List<BookDebt> GetByTime(int publisherID, DateTime date)
        {
            List<Book> books = bookDAO.GetAll();
            List<BookDebt> bookDebts = bookDebtDAO.GetByTime(publisherID, date);
            List<BookDebt> result = new List<BookDebt>();

            foreach (var b in books)
            {
                BookDebt bookDebt = bookDebts
                    .Where(s => s.BookID == b.ID)
                    .OrderByDescending(s => s.Date)
                    .FirstOrDefault();
                if (bookDebt != null)
                    result.Add(bookDebt);
            }

            return result;
        }

        public void Add(int publisherID, int bookID, int quantity)
        {
            BookDebt bookDebt = bookDebtDAO.GetByBookID(publisherID, bookID);

            if (bookDebt == null)
            {
                bookDebt = new BookDebt()
                {
                    PublisherID = publisherID,
                    BookID = bookID,
                    Quantity = Math.Abs(quantity),
                    Date = DateTime.Now
                };
            }
            else
            {
                bookDebt.Quantity += quantity;
                bookDebt.Date = DateTime.Now;
            }

            bookDebtDAO.Add(bookDebt);
        }
    }
}
