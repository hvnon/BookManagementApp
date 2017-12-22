using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DAL.Entities;

namespace Services
{
    public class PublisherService
    {
        PublisherDAO publisherDAO = new PublisherDAO();

        public List<Publisher> GetAll()
        {
            return publisherDAO.GetAll();
        }

        public Publisher GetByID(int id)
        {
            return publisherDAO.GetByID(id);
        }

        public void Add(Publisher publisher)
        {
            publisherDAO.Add(publisher);
        }

        public void Update(Publisher p)
        {
            publisherDAO.Update(p);
        }
    }
}
