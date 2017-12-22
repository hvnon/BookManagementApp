using System.Collections.Generic;
using DAL;
using DAL.Entities;

namespace Services
{
    public class ReportDetailService
    {
        ReportDetailDAO reportDetailDAO = new ReportDetailDAO();

        public List<ReportDetail> GetByReportID(int reportID)
        {
            return reportDetailDAO.GetByReportID(reportID);
        }

        public void Add(ReportDetail reportDetail)
        {
            reportDetailDAO.Add(reportDetail);
        }
    }
}
