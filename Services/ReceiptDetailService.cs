using System.Collections.Generic;
using DAL;
using DAL.Entities;

namespace Services
{
    public class ReceiptDetailService
    {
        ReceiptDetailDAO receiptDetailDAO = new ReceiptDetailDAO();

        public List<ReceiptDetail> GetByReceiptID(int receiptID)
        {
            return receiptDetailDAO.GetByReceiptID(receiptID);
        }

        public void Add(ReceiptDetail receiptDetail)
        {
            receiptDetailDAO.Add(receiptDetail);
        }
    }
}
