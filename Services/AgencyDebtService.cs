using System;
using DAL;
using DAL.Entities;

namespace Services
{
    public class AgencyDebtService
    {
        AgencyDebtDAO agencyDebtDAO = new AgencyDebtDAO();

        public AgencyDebt GetByTime(int agencyID, DateTime date)
        {
            return agencyDebtDAO.GetByTime(agencyID, date);
        }

        public void Add(int agencyID, int total)
        {
            AgencyDebt agencyDebt = agencyDebtDAO.GetByTime(agencyID, DateTime.Now); 

            if(agencyDebt == null)
            {
                agencyDebt = new AgencyDebt()
                {
                    AgencyID = agencyID,
                    Amount = Math.Abs(total),
                    Date = DateTime.Now
                };
            }
            else
            {
                agencyDebt.Amount += total;
                agencyDebt.Date = DateTime.Now;
            }
            

            agencyDebtDAO.Add(agencyDebt);
        }
    }
}
