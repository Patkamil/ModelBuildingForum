using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PatrykKamińskiProjektPZ.Models
{
    public class ModelKoment
    {
        public int ID { get; set; }
        public int ModelID { get; set; }
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
        public virtual Model Model { get; set; }
    }
}