using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DAL.Entities;

namespace Services
{
    public class BookService
    {
        BookDAO bookDAO = new BookDAO();

        public List<Book> GetAll()
        {
            return bookDAO.GetAll();
        }

        public Book GetByID(int id)
        {
            return bookDAO.GetByID(id);
        }

        public List<Book> GetByPublisherID(int publisherID)
        {
            return bookDAO.GetByPublisherID(publisherID);
        }

        public void Add(Book book)
        {
            bookDAO.Add(book);
        }

        public void Update(Book b)
        {
            bookDAO.Update(b);
        }
    }
}
