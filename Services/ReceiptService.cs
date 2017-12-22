using System;
using System.Collections.Generic;

using DAL;
using DAL.Entities;

namespace Services
{
    public class ReceiptService
    {
        ReceiptDAO receiptDAO = new ReceiptDAO();

        public List<Receipt> GetAll()
        {
            return receiptDAO.GetAll();
        }

        public Receipt GetByID(int id)
        {
            return receiptDAO.GetByID(id);
        }

        public int GenerateID()
        {
            Receipt latestReceipt = receiptDAO.GenerateID();
            if (latestReceipt == null)
                return 1;
            else
                return (latestReceipt.ID + 1);
        }

        public List<Receipt> GetByTimeAndPublisherID(DateTime date, int publisherID)
        {
            return receiptDAO.GetByTimeAndPublisherID(date, publisherID);
        }

        public void Add(Receipt receipt)
        {
            receiptDAO.Add(receipt);
        }
    }
}
