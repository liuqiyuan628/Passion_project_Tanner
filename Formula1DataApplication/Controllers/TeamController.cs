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
    public class TeamController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static TeamController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44359/api/");
        }

        // GET: Team/List
        public ActionResult List()
        {
            //objective: communicate with our Team data api to retrieve a list of Teams
            //curl https://localhost:44359/api/Teamdata/listTeams


            string url = "teamdata/listteam";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<TeamDto> Team = response.Content.ReadAsAsync<IEnumerable<TeamDto>>().Result;
            //Debug.WriteLine("Number of Teams received : ");
            //Debug.WriteLine(Teams.Count());


            return View(Team);
        }

        // GET: Team/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with our Team data api to retrieve one Team
            //curl https://localhost:44359/api/Teamdata/findteam/{id}

            DetailsTeam ViewModel = new DetailsTeam();

            string url = "teamdata/findteam/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            TeamDto SelectedTeam = response.Content.ReadAsAsync<TeamDto>().Result;
            Debug.WriteLine("Team received : ");
            Debug.WriteLine(SelectedTeam.TeamName);

            ViewModel.SelectedTeam = SelectedTeam;

            //showcase information about drivers related to this team
            //send a request to gather information about drivers related to a particular team ID
            url = "driverdata/listdriversforteam/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<DriverDto> RelatedDrivers = response.Content.ReadAsAsync<IEnumerable<DriverDto>>().Result;

            ViewModel.RelatedDrivers = RelatedDrivers;


            return View(ViewModel);
        }

        public ActionResult Error()
        {

            return View();
        }

        // GET: Team/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Team/Create
        [HttpPost]
        public ActionResult Create(Team Team)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(Team.TeamName);
            //objective: add a new Team into our system using the API
            //curl -H "Content-Type:application/json" -d @Team.json https://localhost:44359/api/Teamdata/addTeam 
            string url = "teamdata/addteam";


            string jsonpayload = jss.Serialize(Team);
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

        // GET: Team/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "teamdata/findteam/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TeamDto selectedTeam = response.Content.ReadAsAsync<TeamDto>().Result;
            return View(selectedTeam);
        }

        // POST: Team/Update/5
        [HttpPost]
        public ActionResult Update(int id, Team Team)
        {

            string url = "teamdata/updateteam/" + id;
            string jsonpayload = jss.Serialize(Team);
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

        // GET: Team/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "teamdata/findteam/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TeamDto selectedTeam = response.Content.ReadAsAsync<TeamDto>().Result;
            return View(selectedTeam);
        }

        // POST: Team/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "teamdata/deleteteam/" + id;
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
