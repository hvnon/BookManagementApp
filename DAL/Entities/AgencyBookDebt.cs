using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class AgencyBookDebt
    {
        public int ID { get; set; }

        public int AgencyID { get; set; }

        public int BookID { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public virtual Agency Agency { get; set; }

        public virtual Book Book { get; set; }
    }
}