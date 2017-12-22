using System.Collections.Generic;
using DAL;
using DAL.Entities;

namespace Services
{
    public class InvoiceDetailService
    {
        InvoiceDetailDAO invoiceDetailDAO = new InvoiceDetailDAO();

        public List<InvoiceDetail> GetByInvoiceID(int invoiceID)
        {
            return invoiceDetailDAO.GetByInvoiceID(invoiceID);
        }

        public void Add(InvoiceDetail invoiceDetail)
        {
            invoiceDetailDAO.Add(invoiceDetail);
        }
    }
}
