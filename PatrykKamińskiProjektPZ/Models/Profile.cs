using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PatrykKamińskiProjektPZ.Models
{
    public class Profile
    {
        public int Id { get; set; }
        [Display(Name = "User name")]
        public string UserName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Create date")]
        public DateTime CreateData { get; set; }
        //public int MyProperty { get; set; }

        public virtual List<Article> Articles { get; set; }
        public virtual List<Model> Models { get; set; }

        public virtual List<Rating> Ratings { get; set; }
        public virtual List<RatingAstro> RatingsAstro { get; set; }
        public virtual List<ModelKoment> ModelKoments { get; set; }
        public virtual List<ArticleKoment> ArticleKoments { get; set; }
        public virtual List<Astro> Astros { get; set; }
        public virtual List<AstroKoment> AstroKoments { get; set; }
    }
}