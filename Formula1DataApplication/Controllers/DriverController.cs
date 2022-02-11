using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using Formula1DataApplication.Models;
using System.Web.Script.Serialization;

namespace Formula1DataApplication.Controllers
{
    public class DriverController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DriverController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44359/api/Driverdata/");
        }

        // GET: Driver/List
        public ActionResult List()
        {
            //objective: communicate with our Driver data api to retrieve a list of Drivers
            //curl https://localhost:44359/api/Driverdata/listDrivers


            string url = "listDrivers";
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
            //objective: communicate with our Driver data api to retrieve one Driver
            //curl https://localhost:44359/api/Driverdata/findDriver/{id}

            string url = "findDriver/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            DriverDto selecteddriver = response.Content.ReadAsAsync<DriverDto>().Result;
            Debug.WriteLine("driver received : ");
            Debug.WriteLine(selecteddriver.DriverFirstName);


            return View(selecteddriver);
        }

        public ActionResult Error()
        {

            return View();
        }

        // GET: Driver/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Driver/Create
        [HttpPost]
        public ActionResult Create(Driver driver)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(driver.driverName);
            //objective: add a new Driver into our system using the API
            //curl -H "Content-Type:application/json" -d @Driver.json https://localhost:44359/api/Driverdata/addDriver 
            string url = "adddriver";


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
            return View();
        }

        // POST: Driver/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Driver/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Driver/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
