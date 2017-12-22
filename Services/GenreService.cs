using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DAL.Entities;

namespace Services
{
    public class GenreService
    {
        GenreDAO genreDAO = new GenreDAO();

        public List<Genre> GetAll()
        {
            return genreDAO.GetAll();
        }

        public Genre GetByID(int id)
        {
            return genreDAO.GetByID(id);
        }

        public void Add(Genre genre)
        {
            genreDAO.Add(genre);
        }

        public void Update(Genre g)
        {
            genreDAO.Update(g);
        }
    }
}
