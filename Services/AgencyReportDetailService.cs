using System.Collections.Generic;
using DAL;
using DAL.Entities;

namespace Services
{
    public class AgencyReportDetailService
    {
        AgencyReportDetailDAO agencyReportDetailDAO = new AgencyReportDetailDAO();

        public List<AgencyReportDetail> GetByAgencyReportID(int agencyReportID)
        {
            return agencyReportDetailDAO.GetByAgencyReportID(agencyReportID);
        }

        public void Add(AgencyReportDetail agencyReportDetail)
        {
            agencyReportDetailDAO.Add(agencyReportDetail);
        }
    }
}
