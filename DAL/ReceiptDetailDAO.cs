using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using DAL.Entities;

namespace DAL
{
    public class ReceiptDetailDAO
    {
        private BookContext db = new BookContext();

        public List<ReceiptDetail> GetByReceiptID(int receiptID)
        {
            return db.ReceiptDetails.Include(r => r.Book)
                                    .Where(x => x.ReceiptID == receiptID)
                                    .ToList();
        }

        public void Add(ReceiptDetail receiptDetail)
        {
            db.ReceiptDetails.Add(receiptDetail);

            db.SaveChanges();
        }
    }
}
