using System;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;

using DAL.Entities;


namespace DAL
{
    public class AgencyBookDebtDAO
    {
        private BookContext db = new BookContext();
        
        // By default: 
        //  + every get method has 'agencyID' as parameter
        //  + if time is not specified => get latest
        //  + if bookID is not specified => get all books

        public AgencyBookDebt GetByBookID(int agencyID, int bookID)
        {
            return db.AgencyBookDebts
                            .Where(s => s.BookID == bookID && s.AgencyID == agencyID)
                            .OrderByDescending(s => s.Date)
                            .FirstOrDefault();
        }

        public List<AgencyBookDebt> GetByTime(int agencyID, DateTime date)
        {
            return db.AgencyBookDebts
                           .Where(s => s.AgencyID == agencyID
                                && DbFunctions.TruncateTime(s.Date) <= date)
                           .Include(s => s.Book)
                           .OrderByDescending(s => s.Date)
                           .ToList();
        }

        public List<AgencyBookDebt> GetReceivedBooks(int agencyID)
        {
            return db.AgencyBookDebts.Include(s => s.Book)
                                     .Where(s => s.AgencyID == agencyID)
                                     .ToList();
        }


        public void Add(AgencyBookDebt agencyBookDebt)
        {           
            db.AgencyBookDebts.Add(agencyBookDebt);

            db.SaveChanges();
        }
    }
}
