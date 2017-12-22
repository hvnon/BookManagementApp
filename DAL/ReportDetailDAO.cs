using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using DAL.Entities;

namespace DAL
{
    public class ReportDetailDAO
    {
        private BookContext db = new BookContext();

        public List<ReportDetail> GetByReportID(int reportID)
        {
            return db.ReportDetails.Include(r => r.Book)
                                    .Where(x => x.ReportID == reportID)
                                    .ToList();
        }

        public void Add(ReportDetail reportDetail)
        {
            db.ReportDetails.Add(reportDetail);

            db.SaveChanges();
        }

    }
}
