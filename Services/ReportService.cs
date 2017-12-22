using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DAL.Entities;

namespace Services
{
    public class ReportService
    {
        ReportDAO reportDAO = new ReportDAO();
        public List<Report> GetAll()
        {
            return reportDAO.GetAll();
        }

        public int GenerateID()
        {
            Report latestReport = reportDAO.GenerateID();
            if (latestReport == null)
                return 1;
            else
                return (latestReport.ID + 1);
        }

        public Report GetByID(int id)
        {
            return reportDAO.GetByID(id);
        }

        public void Add(Report report)
        {
            reportDAO.Add(report);
        }
    }
}
