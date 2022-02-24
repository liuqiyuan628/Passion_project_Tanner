using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using Formula1DataApplication.Models;
using System.Web.Script.Serialization;
using Formula1DataApplication.Models.ViewModels;

namespace Formula1DataApplication.Controllers
{
    public class DriverController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DriverController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44359/api/");
        }

        // GET: Driver/List
        public ActionResult List()
        {
            //objective: communicate with our Driver data api to retrieve a list of Drivers
            //curl https://localhost:44359/api/Driverdata/listDrivers


            string url = "Driverdata/listDrivers";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<DriverDto> drivers = response.Content.ReadAsAsync<IEnumerable<DriverDto>>().Result;
            //Debug.WriteLine("Number of Drivers received : ");
            //Debug.WriteLine(drivers.Count());


            return View(drivers);
        }

        // GET: Driver/Details/5
        public ActionResult Details(int id)
        {
            DetailsDriver ViewModel = new DetailsDriver();

            //objective: communicate with our Driver data api to retrieve one Driver
            //curl https://localhost:44359/api/Driverdata/findDriver/{id}

            string url = "Driverdata/findDriver/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            DriverDto SelectedDriver = response.Content.ReadAsAsync<DriverDto>().Result;
            Debug.WriteLine("driver received : ");
            Debug.WriteLine(SelectedDriver.DriverFirstName);

            ViewModel.SelectedDriver = SelectedDriver;

            //show associated apsonsors with this driver
            url = "sponsordata/listsponsorsfordriver/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<SponsorDto> ResponsibleSponsors = response.Content.ReadAsAsync<IEnumerable<SponsorDto>>().Result;

            ViewModel.ResponsibleSponsors = ResponsibleSponsors;

            url = "sponsordata/listsponsorsnotcaringfordriver/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<SponsorDto> AvailableSponsors = response.Content.ReadAsAsync<IEnumerable<SponsorDto>>().Result;

            ViewModel.AvailableSponsors = AvailableSponsors;


            return View(ViewModel);
        }


        //POST: driver/Associate/{driverid}
        [HttpPost]
        public ActionResult Associate(int id, int SponsorID)
        {
            Debug.WriteLine("Attempting to associate driver :" + id + " with sponsor " + SponsorID);

            //call our api to associate driver with Sponsor
            string url = "driverdata/associatedriverwithsponsor/" + id + "/" + SponsorID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }


        //Get: driver/UnAssociate/{id}?SponsorID={SponsorID}
        [HttpGet]
        public ActionResult UnAssociate(int id, int SponsorID)
        {
            Debug.WriteLine("Attempting to unassociate driver :" + id + " with sponsor: " + SponsorID);

            //call our api to associate driver with Sponsor
            string url = "driverdata/unassociatedriverwithsponsor/" + id + "/" + SponsorID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }



        public ActionResult Error()
        {

            return View();
        }

        // GET: Driver/New
        public ActionResult New()
        {
            //information about all Team in the system.
            //GET api/Teamdata/listTeam

            string url = "teamdata/listteam";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<TeamDto> TeamOptions = response.Content.ReadAsAsync<IEnumerable<TeamDto>>().Result;

            return View(TeamOptions);
        }

        // POST: Driver/Create
        [HttpPost]
        public ActionResult Create(Driver driver)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(driver.DriverFirstName);
            //objective: add a new Driver into our system using the API
            //curl -H "Content-Type:application/json" -d @driver.json https://localhost:44324/api/Driverdata/addDriver 
            string url = "driverdata/adddriver";


            string jsonpayload = jss.Serialize(driver);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }


        }

        // GET: Driver/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateDriver ViewModel = new UpdateDriver();

            //the existing Driver information
            string url = "driverdata/finddriver/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DriverDto SelectedDriver = response.Content.ReadAsAsync<DriverDto>().Result;
            ViewModel.SelectedDriver = SelectedDriver;

            // all Team to choose from when updating this Driver
            //the existing Driver information
            url = "teamdata/listteam/";
            response = client.GetAsync(url).Result;
            IEnumerable<TeamDto> TeamOptions = response.Content.ReadAsAsync<IEnumerable<TeamDto>>().Result;

            ViewModel.TeamOptions = TeamOptions;

            return View(ViewModel);
        }

        // POST: Driver/Update/5
        [HttpPost]
        public ActionResult Update(int id, Driver driver)
        {

            string url = "driverdata/updatedriver/" + id;
            string jsonpayload = jss.Serialize(driver);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Driver/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "driverdata/finddriver/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DriverDto selecteddriver = response.Content.ReadAsAsync<DriverDto>().Result;
            return View(selecteddriver);
        }

        // POST: Driver/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "driverdata/deletedriver/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }
    }
}
