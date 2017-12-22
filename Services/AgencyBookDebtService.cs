using System;
using System.Collections.Generic;
using System.Linq;

using DAL;
using DAL.Entities;

namespace Services
{
    public class AgencyBookDebtService
    {
        AgencyBookDebtDAO agencyBookDebtDAO = new AgencyBookDebtDAO();
        BookDAO bookDAO = new BookDAO();

        public AgencyBookDebt GetByBookID(int agencyID, int bookID)
        {
            return agencyBookDebtDAO.GetByBookID(agencyID, bookID);
        }

        public List<AgencyBookDebt> GetByTime(int agencyID, DateTime date)
        {
            List<Book> books = bookDAO.GetAll();
            List<AgencyBookDebt> agencyBookDebts = agencyBookDebtDAO.GetByTime(agencyID, date);
            List<AgencyBookDebt> result = new List<AgencyBookDebt>();

            foreach (var b in books)
            {
                AgencyBookDebt agencyBookDebt = agencyBookDebts
                    .Where(s => s.BookID == b.ID)
                    .OrderByDescending(s => s.Date)
                    .FirstOrDefault();
                if(agencyBookDebt != null)
                    result.Add(agencyBookDebt);
            }

            return result;
        }

        public List<Book> GetReceivedBooks(int agencyID)
        {
            List<AgencyBookDebt> agencyBookDebts = agencyBookDebtDAO.GetReceivedBooks(agencyID);
            List<Book> books = bookDAO.GetAll();
            List<Book> receivedBooks = new List<Book>();

            foreach (Book b in books)
            {
                AgencyBookDebt a = agencyBookDebts
                    .Where(s => s.BookID == b.ID)
                    .OrderByDescending(s => s.Date)
                    .FirstOrDefault();
                if (a != null)
                    receivedBooks.Add(a.Book);
            }

            return receivedBooks;
        }

        public void Add(int agencyID, int bookID, int quantity)
        {
            AgencyBookDebt agencyBookDebt = agencyBookDebtDAO.GetByBookID(agencyID, bookID);

            if (agencyBookDebt == null)
            {
                agencyBookDebt = new AgencyBookDebt()
                {
                    AgencyID = agencyID,
                    BookID = bookID,
                    Quantity = Math.Abs(quantity),
                    Date = DateTime.Now
                };
            }
            else
            {
                agencyBookDebt.Quantity += quantity;
                agencyBookDebt.Date = DateTime.Now;
            }
                
            agencyBookDebtDAO.Add(agencyBookDebt);
        }
    }
}
