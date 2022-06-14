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
        [StringLength(5000, ErrorMessage = "The text cannot be longer than 5000 characters")]
        [Display(Name = "Text")]
        public string Text { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Send Date")]
        public DateTime SendDate { get; set; }
        public int ProfileID { get; set; }
        //[Required]
        public virtual Profile Profile { get; set; }
        public virtual Article Article { get; set; }
    }
}