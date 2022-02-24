using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Formula1DataApplication.Models.ViewModels
{
    public class DetailsDriver
    {
        public DriverDto SelectedDriver { get; set; }
        public IEnumerable<SponsorDto> ResponsibleSponsors { get; set; }

        public IEnumerable<SponsorDto> AvailableSponsors { get; set; }
    }
}