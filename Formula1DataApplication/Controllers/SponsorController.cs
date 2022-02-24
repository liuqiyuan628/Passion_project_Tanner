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
    public class SponsorController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static SponsorController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44359/api/");
        }

        // GET: Sponsor/List
        public ActionResult List()
        {
            //objective: communicate with our Sponsor data api to retrieve a list of Sponsors
            //curl https://localhost:44359/api/Sponsordata/listsponsors


            string url = "sponsordata/listsponsors";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<SponsorDto> Sponsors = response.Content.ReadAsAsync<IEnumerable<SponsorDto>>().Result;
            //Debug.WriteLine("Number of Sponsors received : ");
            //Debug.WriteLine(Sponsors.Count());


            return View(Sponsors);
        }

        // GET: Sponsor/Details/5
        public ActionResult Details(int id)
        {
            DetailsSponsor ViewModel = new DetailsSponsor();

            //objective: communicate with our Sponsor data api to retrieve one Sponsor
            //curl https://localhost:44359/api/Sponsordata/findsponsor/{id}

            string url = "sponsordata/findSponsor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            SponsorDto SelectedSponsor = response.Content.ReadAsAsync<SponsorDto>().Result;
            Debug.WriteLine("Sponsor received : ");
            Debug.WriteLine(SelectedSponsor.SponsorName);

            ViewModel.SelectedSponsor = SelectedSponsor;

            //show all drivers under the care of this sponsor
            url = "driverdata/listdriversforsponsor/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<DriverDto> KeptDrivers = response.Content.ReadAsAsync<IEnumerable<DriverDto>>().Result;

            ViewModel.KeptDrivers = KeptDrivers;


            return View(ViewModel);
        }

        public ActionResult Error()
        {

            return View();
        }

        // GET: Sponsor/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Sponsor/Create
        [HttpPost]
        public ActionResult Create(Sponsor Sponsor)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(Sponsor.SponsorName);
            //objective: add a new Sponsor into our system using the API
            //curl -H "Content-Type:application/json" -d @Sponsor.json https://localhost:44359/api/Sponsordata/addSponsor 
            string url = "sponsordata/addsponsor";


            string jsonpayload = jss.Serialize(Sponsor);
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

        // GET: Sponsor/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "sponsordata/findsponsor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            SponsorDto selectedSponsor = response.Content.ReadAsAsync<SponsorDto>().Result;
            return View(selectedSponsor);
        }

        // POST: Sponsor/Update/5
        [HttpPost]
        public ActionResult Update(int id, Sponsor Sponsor)
        {

            string url = "sponsordata/updatesponsor/" + id;
            string jsonpayload = jss.Serialize(Sponsor);
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

        // GET: Sponsor/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "sponsordata/findsponsor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            SponsorDto selectedSponsor = response.Content.ReadAsAsync<SponsorDto>().Result;
            return View(selectedSponsor);
        }

        // POST: Sponsor/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "sponsordata/deletesponsor/" + id;
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
