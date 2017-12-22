using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DAL.Entities;

namespace Services
{
    public class AgencyService
    {
        AgencyDAO agencyDAO = new AgencyDAO();

        public List<Agency> GetAll()
        {
            return agencyDAO.GetAll();
        }

        public Agency GetByID(int id)
        {
            return agencyDAO.GetByID(id);
        }

        public void Add(Agency agency)
        {
            agencyDAO.Add(agency);
        }

        public void Update(Agency a)
        {
            agencyDAO.Update(a);
        }
    }
}
