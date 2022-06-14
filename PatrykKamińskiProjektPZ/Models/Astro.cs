using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PatrykKamińskiProjektPZ.Models
{
    public class Astro
    {
        public int ID { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Tytuł nie może być dłuższy niż 50 znaków")]
        [Display(Name = "Name")]
        public string Name { get; set; }
        public int ProfileID { get; set; }
        [Display(Name = "Picture")]
        public string Picture { get; set; }
        [StringLength(5000, ErrorMessage = "Opis nie może być dłuższy niż 5000 znaków")]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Send date")]
        public DateTime SendDate { get; set; }
        //[Required]
        public virtual Profile Profile { get; set; }
        public virtual ICollection<RatingAstro> RatingsAstro { get; set; }
        public virtual ICollection<AstroKoment> AstroKoments { get; set; }
    }
}