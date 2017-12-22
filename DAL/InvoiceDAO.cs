using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using DAL.Entities;

namespace DAL
{
    public class InvoiceDAO
    {
        private BookContext db = new BookContext();

        public List<Invoice> GetAll()
        {
            return db.Invoices.Include(i => i.Agency).ToList();
        }

        public Invoice GetByID(int id)
        {
            return db.Invoices.Include(r => r.Agency)
                              .Include(r => r.InvoiceDetails.Select(q => q.Book))
                              .SingleOrDefault(x => x.ID == id);
        }

        public Invoice GenerateID()
        {
            return db.Invoices.OrderByDescending(s => s.ID).FirstOrDefault();
        }

        public List<Invoice> GetByTimeAndAgencyID(DateTime date, int agencyID)
        {
            return db.Invoices
                            .Where(s => s.AgencyID == agencyID
                            && DbFunctions.TruncateTime(s.Date) <= date)
                            .Include(s => s.InvoiceDetails)
                            .ToList();
        }

        public void Add(Invoice invoice)
        {
            db.Invoices.Add(invoice);

            db.SaveChanges();
        }

    }
}
