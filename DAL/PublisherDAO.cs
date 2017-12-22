using System;
using System.Collections.Generic;
using System.Linq;

using DAL.Entities;

namespace DAL
{
    public class PublisherDAO
    {
        private BookContext db = new BookContext();

        public List<Publisher> GetAll()
        {
            return db.Publishers.ToList();
        }

        public Publisher GetByID(int id)
        {
            return db.Publishers.Find(id);
        }

        public void Add(Publisher publisher)
        {
            db.Publishers.Add(publisher);

            db.SaveChanges();
        }

        public void Update(Publisher p)
        {
            Publisher publisher = db.Publishers.Find(p.ID);

            publisher.Name = p.Name;
            publisher.Address = p.Address;
            publisher.AccountNumber = p.AccountNumber;
            publisher.Phone = p.Phone;

            db.SaveChanges();
        }
    }
}
