using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;

using DAL.Entities;

namespace DAL
{
    public class BookDAO
    {
        private BookContext db = new BookContext();

        public List<Book> GetAll()
        {
            return db.Books.Include(x => x.Genre).Include(x => x.Publisher).ToList();
        }

        public List<Book> GetByPublisherID(int publisherID)
        {
            return db.Books.Where(s => s.PublisherID == publisherID)
                                    .ToList();
        }

        public Book GetByID(int id)
        {
            return db.Books.Find(id);
        }    

        public void Add(Book book)
        {
            db.Books.Add(book);

            db.SaveChanges();
        }

        public void Update(Book b)
        {
            Book book = db.Books.Find(b.ID);

            book.PublisherID = b.PublisherID;
            book.GenreID = b.GenreID;
            book.Name = b.Name;
            book.Author = b.Author;
            book.PublicationDate = b.PublicationDate;
            book.PurchasePrice = b.PurchasePrice;
            book.SellingPrice = b.SellingPrice;

            db.SaveChanges();
        }
    }
}
