using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PatrykKamińskiProjektPZ.Models
{
    public class Album
    {
        public int ID { get; set; }
        public int ModelID { get; set; }
        [Display(Name = "Zdjęcia")]
        public string Picture { get; set; }

        public virtual Model Model { get; set; }
    }
}