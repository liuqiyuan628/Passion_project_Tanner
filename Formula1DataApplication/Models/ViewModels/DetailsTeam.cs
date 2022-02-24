using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Formula1DataApplication.Models.ViewModels
{
    public class DetailsTeam
    {
        //the team itself that we want to display
        public TeamDto SelectedTeam { get; set; }

        //all of the related drivers to that particular team
        public IEnumerable<DriverDto> RelatedDrivers { get; set; }
    }
}