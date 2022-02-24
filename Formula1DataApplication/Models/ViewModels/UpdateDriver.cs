using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Formula1DataApplication.Models.ViewModels
{
    public class UpdateDriver
    {
        //This viewmodel is a class which stores information that we need to present to /Driver/Update/{}

        //the existing animal information

        public DriverDto SelectedDriver { get; set; }

        // all team to choose from when updating this animal

        public IEnumerable<TeamDto> TeamOptions { get; set; }

    }
}