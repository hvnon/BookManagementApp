using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using DAL.Entities;

namespace DAL
{
    public class ReceiptDAO
    {
        private BookContext db = new BookContext();

        public List<Receipt> GetAll()
        {
            return db.Receipts.Include(i => i.Publisher).ToList();
        }

        public Receipt GetByID(int id)
        {
            return db.Receipts.Include(r => r.Publisher)
                                          .Include(r => r.ReceiptDetails.Select(q => q.Book))
                                          .SingleOrDefault(x => x.ID == id);
        }

        public Receipt GenerateID()
        {
            return db.Receipts.OrderByDescending(s => s.ID).FirstOrDefault();
        }

        public List<Receipt> GetByTimeAndPublisherID(DateTime date, int publisherID)
        {
            return db.Receipts
                            .Where(s => s.PublisherID == publisherID
                            && DbFunctions.TruncateTime(s.Date) <= date)
                            .Include(s => s.ReceiptDetails)
                            .ToList();
        }

        public void Add(Receipt receipt)
        {
            db.Receipts.Add(receipt);

            db.SaveChanges();
        }

    }
}
