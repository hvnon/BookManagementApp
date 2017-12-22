using System;
using DAL;
using DAL.Entities;

namespace Services
{
    public class DebtService
    {
        DebtDAO debtDAO = new DebtDAO();

        public Debt GetByTime(int publisherID, DateTime date)
        {
            return debtDAO.GetByTime(publisherID, date);
        }

        public void Add(int publisherID, int total)
        {
            Debt debt = debtDAO.GetByTime(publisherID, DateTime.Now);

            if (debt == null)
            {
                debt = new Debt()
                {
                    PublisherID = publisherID,
                    Amount = Math.Abs(total),
                    Date = DateTime.Now
                };
            }
            else
            {
                debt.Amount += total;
                debt.Date = DateTime.Now;
            }


            debtDAO.Add(debt);
        }
    }
}
