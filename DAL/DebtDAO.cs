using System;
using System.Data.Entity;
using System.Linq;

using DAL.Entities;

namespace DAL
{
    public class DebtDAO
    {
        private BookContext db = new BookContext();

        // By default: every get method has 'publisherID' as parameter

        public Debt GetByTime(int publisherID, DateTime date)
        {
            return db.Debts.Where(s => s.PublisherID == publisherID
                                && DbFunctions.TruncateTime(s.Date) <= date)
                          .OrderByDescending(s => s.Date)
                          .FirstOrDefault();
        }

        public void Add(Debt debt)
        {
            db.Debts.Add(debt);

            db.SaveChanges();
        }
    }
}
