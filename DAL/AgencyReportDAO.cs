using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using DAL.Entities;

namespace DAL
{
    public class AgencyReportDAO
    {
        private BookContext db = new BookContext();

        public List<AgencyReport> GetAll()
        {
            return db.AgencyReports.Include(i => i.Agency).ToList();
        }

        public AgencyReport GetByID(int id)
        {
            return db.AgencyReports.Include(r => r.Agency)
                              .Include(r => r.AgencyReportDetails.Select(q => q.Book))
                              .SingleOrDefault(x => x.ID == id);
        }

        public AgencyReport GenerateID()
        {
            return db.AgencyReports.OrderByDescending(s => s.ID).FirstOrDefault();
        }

        public List<AgencyReport> GetByTimeAndAgencyID(int agencyID, DateTime fromDate, DateTime toDate)
        {
            return db.AgencyReports
                            .Where(s => s.AgencyID == agencyID
                            && DbFunctions.TruncateTime(s.Date) >= fromDate
                            && DbFunctions.TruncateTime(s.Date) <= toDate)
                            .Include(s => s.AgencyReportDetails)
                            .ToList();
        }

        public void Add(AgencyReport agencyReport)
        {
            db.AgencyReports.Add(agencyReport);

            db.SaveChanges();
        }

    }
}
