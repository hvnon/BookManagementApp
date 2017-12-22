using System;
using System.Data.Entity;
using System.Linq;

using DAL.Entities;

namespace DAL
{
    public class AgencyDebtDAO
    {
        private BookContext db = new BookContext();

        // By default: every get method has 'agencyID' as parameter

        public AgencyDebt GetByTime(int agencyID, DateTime date)
        {
            return db.AgencyDebts.Where
                                   (
                                    s => s.AgencyID == agencyID
                                    && DbFunctions.TruncateTime(s.Date) <= date
                                    )
                                 .OrderByDescending(s => s.Date)
                                 .FirstOrDefault();
        }

        public void Add(AgencyDebt agencyDebt)
        {
            db.AgencyDebts.Add(agencyDebt);

            db.SaveChanges();
        }
    }
}
