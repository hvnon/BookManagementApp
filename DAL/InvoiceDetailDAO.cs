using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using DAL.Entities;

namespace DAL
{
    public class InvoiceDetailDAO
    {
        private BookContext db = new BookContext();

        public List<InvoiceDetail> GetByInvoiceID(int invoiceID)
        {
            return db.InvoiceDetails.Include(r => r.Book)           
                                    .Where(x => x.InvoiceID == invoiceID)
                                    .ToList();
        }

        public void Add(InvoiceDetail invoiceDetail)
        {
            db.InvoiceDetails.Add(invoiceDetail);

            db.SaveChanges();
        }

    }
}
