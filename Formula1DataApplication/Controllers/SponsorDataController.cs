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
    public class SponsorDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Sponsors in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Sponsors in the database, including their associated team.
        /// </returns>
        /// <example>
        /// GET: api/SponsorData/ListSponsors
        /// </example>
        [HttpGet]
        [ResponseType(typeof(SponsorDto))]
        public IHttpActionResult ListSponsors()
        {
            List<Sponsor> Sponsors = db.Sponsors.ToList();
            List<SponsorDto> SponsorDtos = new List<SponsorDto>();

            Sponsors.ForEach(k => SponsorDtos.Add(new SponsorDto()
            {
                SponsorID = k.SponsorID,
                SponsorName = k.SponsorName,
            }));

            return Ok(SponsorDtos);
        }

        /// <summary>
        /// Returns all Sponsors in the system associated with a particular driver.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Sponsors in the database taking care of a particular driver
        /// </returns>
        /// <param name="id">Driver Primary Key</param>
        /// <example>
        /// GET: api/SponsorData/ListSponsorsForDriver/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(SponsorDto))]
        public IHttpActionResult ListSponsorsForDriver(int id)
        {
            List<Sponsor> Sponsors = db.Sponsors.Where(
                k => k.Drivers.Any(
                    a => a.DriverID == id)
                ).ToList();
            List<SponsorDto> SponsorDtos = new List<SponsorDto>();

            Sponsors.ForEach(k => SponsorDtos.Add(new SponsorDto()
            {
                SponsorID = k.SponsorID,
                SponsorName = k.SponsorName,
            }));

            return Ok(SponsorDtos);
        }


        /// <summary>
        /// Returns Sponsors in the system not caring for a particular driver.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Sponsors in the database not taking care of a particular driver
        /// </returns>
        /// <param name="id">Driver Primary Key</param>
        /// <example>
        /// GET: api/SponsorData/ListSponsorsNotCaringForDriver/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(SponsorDto))]
        public IHttpActionResult ListSponsorsNotCaringForDriver(int id)
        {
            List<Sponsor> Sponsors = db.Sponsors.Where(
                k => !k.Drivers.Any(
                    a => a.DriverID == id)
                ).ToList();
            List<SponsorDto> SponsorDtos = new List<SponsorDto>();

            Sponsors.ForEach(k => SponsorDtos.Add(new SponsorDto()
            {
                SponsorID = k.SponsorID,
                SponsorName = k.SponsorName,
            }));

            return Ok(SponsorDtos);
        }

        /// <summary>
        /// Returns all Sponsors in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An Sponsor in the system matching up to the Sponsor ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Sponsor</param>
        /// <example>
        /// GET: api/SponsorData/FindSponsor/5
        /// </example>
        [ResponseType(typeof(SponsorDto))]
        [HttpGet]
        public IHttpActionResult FindSponsor(int id)
        {
            Sponsor Sponsor = db.Sponsors.Find(id);
            SponsorDto SponsorDto = new SponsorDto()
            {
                SponsorID = Sponsor.SponsorID,
                SponsorName = Sponsor.SponsorName,
            };
            if (Sponsor == null)
            {
                return NotFound();
            }

            return Ok(SponsorDto);
        }

        /// <summary>
        /// Updates a particular Sponsor in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Sponsor ID primary key</param>
        /// <param name="Sponsor">JSON FORM DATA of an Sponsor</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/SponsorData/UpdateSponsor/5
        /// FORM DATA: Sponsor JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateSponsor(int id, Sponsor Sponsor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Sponsor.SponsorID)
            {

                return BadRequest();
            }

            db.Entry(Sponsor).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SponsorExists(id))
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
        /// Adds an Sponsor to the system
        /// </summary>
        /// <param name="Sponsor">JSON FORM DATA of an Sponsor</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Sponsor ID, Sponsor Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/SponsorData/AddSponsor
        /// FORM DATA: Sponsor JSON Object
        /// </example>
        [ResponseType(typeof(Sponsor))]
        [HttpPost]
        public IHttpActionResult AddSponsor(Sponsor Sponsor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Sponsors.Add(Sponsor);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Sponsor.SponsorID }, Sponsor);
        }

        /// <summary>
        /// Deletes an Sponsor from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Sponsor</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/SponsorData/DeleteSponsor/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Sponsor))]
        [HttpPost]
        public IHttpActionResult DeleteSponsor(int id)
        {
            Sponsor Sponsor = db.Sponsors.Find(id);
            if (Sponsor == null)
            {
                return NotFound();
            }

            db.Sponsors.Remove(Sponsor);
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

        private bool SponsorExists(int id)
        {
            return db.Sponsors.Count(e => e.SponsorID == id) > 0;
        }
    }
}