using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Formula1DataApplication.Models
{
    public class Driver
    {
        // create BD colums here
        [Key]
        public int DriverID { get; set; }
        public string DriverFirstName { get; set; }
        public string DriverLastName { get; set; }
        public int DriverNumber { get; set; }
        public decimal DriverPoints { get; set; }
        public string DriverCountry { get; set; }

        //An driver belongs to one team
        //A team can have many drivers
        [ForeignKey("Team")]
        public int TeamID { get; set; }
        public virtual Team Team { get; set; }

        //an driver can be supported by many sponsors
        public ICollection<Sponsor> Sponsors { get; set; }

    }

    public class DriverDto
    {
        public int DriverID { get; set; }
        public string DriverFirstName { get; set; }
        public string DriverLastName { get; set; }
        public int DriverNumber { get; set; }
        public decimal DriverPoints { get; set; }
        public string DriverCountry { get; set; }

        public string TeamName { get; set; }



    }
}