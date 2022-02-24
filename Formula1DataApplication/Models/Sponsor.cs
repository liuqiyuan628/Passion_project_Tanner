using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Formula1DataApplication.Models
{
    public class Sponsor
    {
        [Key]
        public int SponsorID { get; set; }
        public string SponsorName { get; set; }


        //A Sponsor can support many drivers
        public ICollection<Driver> Drivers { get; set; }

    }

    public class SponsorDto
    {
        public int SponsorID { get; set; }
        public string SponsorName { get; set; }
    }
}