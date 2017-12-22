using System;
using System.Collections.Generic;
using System.Linq;

using DAL.Entities;

namespace DAL
{
    public class GenreDAO
    {
        private BookContext db = new BookContext();

        public List<Genre> GetAll()
        {
            return db.Genres.ToList();
        }

        public Genre GetByID(int id)
        {
            return db.Genres.Find(id);
        }

        public void Add(Genre genre)
        {
            db.Genres.Add(genre);

            db.SaveChanges();
        }

        public void Update(Genre g)
        {
            Genre genre = db.Genres.Find(g.ID);

            genre.Name = g.Name;

            db.SaveChanges();
        }
    }
}
