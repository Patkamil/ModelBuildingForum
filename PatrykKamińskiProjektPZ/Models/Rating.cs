using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatrykKamińskiProjektPZ.Models
{
    public class Rating
    {
        public int ID { get; set; }
        public int Number { get; set; }
        public int ProfileID { get; set; }
        public int ModelID { get; set; }
        public virtual Model Model { get; set; }
        public virtual Profile Profile { get; set; }
    }
}