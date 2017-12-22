using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using DAL.Entities;

namespace DAL
{
    public class ReportDAO
    {
        private BookContext db = new BookContext();

        public List<Report> GetAll()
        {
            return db.Reports.Include(a => a.Publisher).ToList();
        }

        public Report GenerateID()
        {
            return db.Reports.OrderByDescending(s => s.ID).FirstOrDefault();
        }

        public Report GetByID(int id)
        {
            return db.Reports.Include(s => s.Publisher)
                             .Where(s => s.ID == id)
                             .FirstOrDefault();
        }

        public void Add(Report report)
        {
            db.Reports.Add(report);

            db.SaveChanges();
        }
    }
}
