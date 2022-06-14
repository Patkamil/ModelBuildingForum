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
        [StringLength(50, ErrorMessage = "The text cannot be longer than 50 characters")]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Required]
        [StringLength(5000, ErrorMessage = "The text cannot be longer than 5000 characters")]
        [Display(Name = "Text")]
        public string Text { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Send date")]
        public DateTime SendDate { get; set; }
        public int ProfileID { get; set; }
        //[Required]
        public virtual Profile Profile { get; set; }
        public virtual ICollection<ArticleKoment> ArticleKoments { get; set; }
    }
}