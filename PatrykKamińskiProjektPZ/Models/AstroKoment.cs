using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PatrykKamińskiProjektPZ.Models
{
    public class AstroKoment
    {
        public int ID { get; set; }
        public int AstroID { get; set; }
        [Required]
        [StringLength(5000, ErrorMessage = "Tekst nie może być dłuższy niż 5000 znaków")]
        [Display(Name = "Tekst")]
        public string Text { get; set; }
        public int ProfileID { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data wysłania")]
        public DateTime SendDate { get; set; }
        //[Required]
        public virtual Profile Profile { get; set; }
        public virtual Astro Astro { get; set; }
    }
}