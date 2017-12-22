using System;
using System.Collections.Generic;

using DAL;
using DAL.Entities;

namespace Services
{
    public class InvoiceService
    {
        InvoiceDAO invoiceDAO = new InvoiceDAO();

        public List<Invoice> GetAll()
        {
            return invoiceDAO.GetAll();
        }

        public Invoice GetByID(int id)
        {
            return invoiceDAO.GetByID(id);
        }

        public int GenerateID()
        {
            Invoice latestInvoice = invoiceDAO.GenerateID();
            if (latestInvoice == null)
                return 1;
            else
                return (latestInvoice.ID + 1);
        }

        public List<Invoice> GetByTimeAndAgencyID(DateTime date, int agencyID)
        {
            return invoiceDAO.GetByTimeAndAgencyID(date, agencyID);
        }

        public void Add(Invoice invoice)
        {
            invoiceDAO.Add(invoice);
        }
    }
}
