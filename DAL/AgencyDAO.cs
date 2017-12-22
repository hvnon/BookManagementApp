using System;
using System.Collections.Generic;
using System.Linq;

using DAL.Entities;

namespace DAL
{
    public class AgencyDAO
    {
        private BookContext db = new BookContext();

        public List<Agency> GetAll()
        {
            return db.Agencies.ToList();
        }

        public Agency GetByID(int id)
        {
            return db.Agencies.Find(id);
        }

        public void Add(Agency agency)
        {
            db.Agencies.Add(agency);

            db.SaveChanges();
        }

        public void Update(Agency a)
        {
            Agency agency = db.Agencies.Find(a.ID);

            agency.Name = a.Name;
            agency.Address = a.Address;
            agency.AccountNumber = a.AccountNumber;
            agency.Phone = a.Phone;

            db.SaveChanges();
        }
    }
}
