using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PatrykKamińskiProjektPZ.Models
{
    public class Model
    {
        public int ID { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "The name cannot be longer than 50 characters")]
        [Display(Name = "Name")]
        public string Name { get; set; }
        public int ProfileID { get; set; }
        //[Required]
        [Display(Name = "Picture")]
        public string Picture { get; set; }
        [StringLength(5000, ErrorMessage = "The description cannot be longer than 5000 characters")]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Send date")]
        public DateTime SendDate { get; set; }
        //[Required]
        
        public virtual Profile Profile { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<ModelKoment> ModelKoments { get; set; }
        public virtual ICollection<Album> Pictures { get; set; }
    }
}