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
        public IEnumerable<DriverDto> ListDrivers()
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
                TeamName = a.Team.TeamName
            }));

            return DriverDtos;
        }

        // GET: api/DriverData/FindDriver/5
        [ResponseType(typeof(Driver))]
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
                TeamName = Driver.Team.TeamName
            };
            if (Driver == null)
            {
                return NotFound();
            }

            return Ok(DriverDto);
        }

        // POST: api/DriverData/UpdateDriver/5
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

        // POST: api/DriverData/AddDriver
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

        // POST: api/DriverData/DeleteDriver/5
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