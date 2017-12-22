using System;
using System.Collections.Generic;

using DAL;
using DAL.Entities;

namespace Services
{
    public class AgencyReportService
    {
        AgencyReportDAO agencyReportDAO = new AgencyReportDAO();

        public List<AgencyReport> GetAll()
        {
            return agencyReportDAO.GetAll();
        }

        public AgencyReport GetByID(int id)
        {
            return agencyReportDAO.GetByID(id);
        }

        public int GenerateID()
        {
            AgencyReport agencyReport = agencyReportDAO.GenerateID();
            if (agencyReport == null)
                return 1;
            else return (agencyReport.ID + 1);
        }

        public List<AgencyReport> GetByTimeAndAgencyID(int agencyID, DateTime fromDate, DateTime toDate)
        {
            return agencyReportDAO.GetByTimeAndAgencyID(agencyID, fromDate, toDate);
        }

        public void Add(AgencyReport agencyReport)
        {
            agencyReportDAO.Add(agencyReport);
        }
    }
}
