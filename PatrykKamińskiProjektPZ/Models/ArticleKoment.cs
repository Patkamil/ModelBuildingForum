using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Razor;

namespace PatrykKamińskiProjektPZ.Models
{
    public class ArticleKoment
    {
        public int Id { get; set; }
        public int ArticleID { get; set; }
        [Required]
        [StringLength(5000, ErrorMessage = "Tekst nie może być dłuższy niż 5000 znaków")]
        [Display(Name = "Tekst")]
        public string Text { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data wysłania")]
        public DateTime SendDate { get; set; }
        public int ProfileID { get; set; }
        //[Required]
        public virtual Profile Profile { get; set; }
        public virtual Article Article { get; set; }
    }
}