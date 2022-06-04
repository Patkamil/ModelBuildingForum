using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatrykKamińskiProjektPZ.Models
{
    public class RatingAstro
    {
        public int ID { get; set; }
        public int Number { get; set; }
        public int ProfileID { get; set; }
        public int AstroID { get; set; }
        public virtual Astro Astro { get; set; }
        public virtual Profile Profile { get; set; }
    }
}