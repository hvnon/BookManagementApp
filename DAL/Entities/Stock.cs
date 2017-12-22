using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Stock
    {
        public int ID { get; set; }

        public int BookID { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public virtual Book Book { get; set; }
    }
}