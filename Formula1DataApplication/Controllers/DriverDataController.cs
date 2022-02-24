using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Formula1DataApplication.Models;
using System.Diagnostics;

namespace Formula1DataApplication.Controllers
{
    public class DriverDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/DriverData/ListDrivers
        [HttpGet]
        [ResponseType(typeof(DriverDto))]
        public IHttpActionResult ListDrivers()
        {
            List<Driver> Drivers = db.Drivers.ToList();
            List<DriverDto> DriverDtos = new List<DriverDto>();

            Drivers.ForEach(a => DriverDtos.Add(new DriverDto()
            {
                DriverID = a.DriverID,
                DriverFirstName = a.DriverFirstName,
                DriverLastName = a.DriverLastName,
                DriverNumber =a.DriverNumber,
                DriverPoints = a.DriverPoints,
                DriverCountry = a.DriverCountry,
                TeamID = a.Team.TeamID,
                TeamName = a.Team.TeamName
            }));

            return Ok(DriverDtos);
        }

        /// <summary>
        /// Gathers information about all drivers related to a particular team ID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Drivers in the database, including their associated team matched with a particular team ID
        /// </returns>
        /// <param name="id">Team ID.</param>
        /// <example>
        /// GET: api/DriverData/ListDriversForTeam/3
        /// </example>
        [HttpGet]
        [ResponseType(typeof(DriverDto))]
        public IHttpActionResult ListDriversForTeam(int id)
        {
            List<Driver> Drivers = db.Drivers.Where(a => a.TeamID == id).ToList();
            List<DriverDto> DriverDtos = new List<DriverDto>();

            Drivers.ForEach(a => DriverDtos.Add(new DriverDto()
            {
                DriverID = a.DriverID,
                DriverFirstName = a.DriverFirstName,
                DriverLastName = a.DriverLastName,
                DriverNumber = a.DriverNumber,
                DriverPoints = a.DriverPoints,
                DriverCountry = a.DriverCountry,
                TeamID = a.Team.TeamID,
                TeamName = a.Team.TeamName
            }));

            return Ok(DriverDtos);
        }

        /// <summary>
        /// Gathers information about Drivers related to a particular sponsor
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Drivers in the database, including their associated team that match to a particular sponsor id
        /// </returns>
        /// <param name="id">Sponsor ID.</param>
        /// <example>
        /// GET: api/DriverData/ListDriversForSponsor/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(DriverDto))]
        public IHttpActionResult ListDriversForSponsor(int id)
        {
            //all Drivers that have sponsors which match with our ID
            List<Driver> Drivers = db.Drivers.Where(
                a => a.Sponsors.Any(
                    k => k.SponsorID == id
                )).ToList();
            List<DriverDto> DriverDtos = new List<DriverDto>();

            Drivers.ForEach(a => DriverDtos.Add(new DriverDto()
            {
                DriverID = a.DriverID,
                DriverFirstName = a.DriverFirstName,
                DriverLastName = a.DriverLastName,
                DriverNumber = a.DriverNumber,
                DriverPoints = a.DriverPoints,
                DriverCountry = a.DriverCountry,
                TeamID = a.Team.TeamID,
                TeamName = a.Team.TeamName
            }));

            return Ok(DriverDtos);
        }


        /// <summary>
        /// Associates a particular sponsor with a particular Driver
        /// </summary>
        /// <param name="Driverid">The Driver ID primary key</param>
        /// <param name="sponsorid">The sponsor ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/DriverData/AssociateDriverWithSponsor/9/1
        /// </example>
        [HttpPost]
        [Route("api/DriverData/AssociateDriverWithSponsor/{driverid}/{sponsorid}")]
        public IHttpActionResult AssociateDriverWithSponsor(int driverid, int sponsorid)
        {

            Driver SelectedDriver = db.Drivers.Include(a => a.Sponsors).Where(a => a.DriverID == driverid).FirstOrDefault();
            Sponsor SelectedSponsor = db.Sponsors.Find(sponsorid);

            if (SelectedDriver == null || SelectedSponsor == null)
            {
                return NotFound();
            }

            Debug.WriteLine("input Driver id is: " + driverid);
            Debug.WriteLine("selected Driver name is: " + SelectedDriver.DriverFirstName);
            Debug.WriteLine("input sponsor id is: " + sponsorid);
            Debug.WriteLine("selected sponsor name is: " + SelectedSponsor.SponsorName);


            SelectedDriver.Sponsors.Add(SelectedSponsor);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Removes an association between a particular sponsor and a particular Driver
        /// </summary>
        /// <param name="Driverid">The Driver ID primary key</param>
        /// <param name="sponsorid">The sponsor ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/DriverData/AssociateDriverWithSponsor/9/1
        /// </example>
        [HttpPost]
        [Route("api/DriverData/UnAssociateDriverWithSponsor/{driverid}/{sponsorid}")]
        public IHttpActionResult UnAssociateDriverWithSponsor(int driverid, int sponsorid)
        {

            Driver SelectedDriver = db.Drivers.Include(a => a.Sponsors).Where(a => a.DriverID == driverid).FirstOrDefault();
            Sponsor SelectedSponsor = db.Sponsors.Find(sponsorid);

            if (SelectedDriver == null || SelectedSponsor == null)
            {
                return NotFound();
            }

            Debug.WriteLine("input driver id is: " + driverid);
            Debug.WriteLine("selected driver name is: " + SelectedDriver.DriverFirstName);
            Debug.WriteLine("input sponsor id is: " + sponsorid);
            Debug.WriteLine("selected sponsor name is: " + SelectedSponsor.SponsorName);


            SelectedDriver.Sponsors.Remove(SelectedSponsor);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Returns all drivers in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An driver in the system matching up to the driver ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the driver</param>
        /// <example>
        /// GET: api/DriverData/FindDriver/5
        /// </example>
        [ResponseType(typeof(DriverDto))]
        [HttpGet]
        public IHttpActionResult FindDriver(int id)
        {
            Driver Driver = db.Drivers.Find(id);
            DriverDto DriverDto = new DriverDto()
            {
                DriverID = Driver.DriverID,
                DriverFirstName = Driver.DriverFirstName,
                DriverLastName = Driver.DriverLastName,
                DriverNumber = Driver.DriverNumber,
                DriverPoints = Driver.DriverPoints,
                DriverCountry = Driver.DriverCountry,
                TeamID = Driver.Team.TeamID,
                TeamName = Driver.Team.TeamName
            };
            if (Driver == null)
            {
                return NotFound();
            }

            return Ok(DriverDto);
        }

        /// <summary>
        /// Updates a particular driver in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Driver ID primary key</param>
        /// <param name="driver">JSON FORM DATA of an driver</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/DriverData/UpdateDriver/5
        /// FORM DATA: Driver JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateDriver(int id, Driver driver)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != driver.DriverID)
            {

                return BadRequest();
            }

            db.Entry(driver).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriverExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds an driver to the system
        /// </summary>
        /// <param name="driver">JSON FORM DATA of an driver</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Driver ID, Driver Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/DriverData/AddDriver
        /// FORM DATA: Driver JSON Object
        /// </example>
        [ResponseType(typeof(Driver))]
        [HttpPost]
        public IHttpActionResult AddDriver(Driver driver)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Drivers.Add(driver);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = driver.DriverID }, driver);
        }

        /// <summary>
        /// Deletes an driver from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the driver</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/DriverData/DeleteDriver/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Driver))]
        [HttpPost]
        public IHttpActionResult DeleteDriver(int id)
        {
            Driver driver = db.Drivers.Find(id);
            if (driver == null)
            {
                return NotFound();
            }

            db.Drivers.Remove(driver);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DriverExists(int id)
        {
            return db.Drivers.Count(e => e.DriverID == id) > 0;
        }
    }
}