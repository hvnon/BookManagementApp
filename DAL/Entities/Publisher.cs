using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Publisher
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Tên không được trống")]

        public string Name { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string AccountNumber { get; set; }

        public virtual ICollection<Book> Books { get; set; }

        public virtual ICollection<Receipt> Receipts { get; set; }

        public virtual ICollection<Report> Reports { get; set; }
    }
}