using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PatrykKamińskiProjektPZ.Models
{
    public class Article
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Tytuł nie może być dłuższy niż 50 znaków")]
        [Display(Name = "Tytuł")]
        public string Title { get; set; }
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
        public virtual ICollection<ArticleKoment> ArticleKoments { get; set; }
    }
}