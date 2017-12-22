using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using DAL.Entities;

namespace DAL
{
    public class AgencyReportDetailDAO
    {
        private BookContext db = new BookContext();

        public List<AgencyReportDetail> GetByAgencyReportID(int agencyReportID)
        {
            return db.AgencyReportDetails.Include(r => r.Book)
                                    .Where(x => x.AgencyReportID == agencyReportID)
                                    .ToList();
        }

        public void Add(AgencyReportDetail agencyReportDetail)
        {
            db.AgencyReportDetails.Add(agencyReportDetail);

            db.SaveChanges();
        }

    }
}
