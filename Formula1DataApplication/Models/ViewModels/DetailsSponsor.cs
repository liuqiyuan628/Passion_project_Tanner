using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Formula1DataApplication.Models.ViewModels
{
    public class DetailsSponsor
    {
        public SponsorDto SelectedSponsor { get; set; }
        public IEnumerable<DriverDto> KeptDrivers { get; set; }
    }
}