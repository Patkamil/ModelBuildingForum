using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PatrykKamińskiProjektPZ.ViewModels
{
    public class ForumDateGroup
    {
        public string Name { get; set; }

        public int Count { get; set; }

        //[DataType(DataType.Date)]
        //public DateTime? RegistrationDate2 { get; set; }

        //public int Count2 { get; set; }
    }
}