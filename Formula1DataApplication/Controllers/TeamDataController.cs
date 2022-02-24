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
    public class TeamDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Teams in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Teams in the database, including their associated team.
        /// </returns>
        /// <example>
        /// GET: api/TeamData/ListTeam
        /// </example>
        [HttpGet]
        [ResponseType(typeof(TeamDto))]
        public IHttpActionResult ListTeam()
        {
            List<Team> Team = db.Teams.ToList();
            List<TeamDto> TeamDtos = new List<TeamDto>();

            Team.ForEach(s => TeamDtos.Add(new TeamDto()
            {
                TeamID = s.TeamID,
                TeamName = s.TeamName,
                TeamCountry = s.TeamCountry,
                TeamYear = s. TeamYear
            }));

            return Ok(TeamDtos);
        }

        /// <summary>
        /// Returns all Teams in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An Team in the system matching up to the Team ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Team</param>
        /// <example>
        /// GET: api/TeamData/FindTeam/5
        /// </example>
        [ResponseType(typeof(TeamDto))]
        [HttpGet]
        public IHttpActionResult FindTeam(int id)
        {
            Team Team = db.Teams.Find(id);
            TeamDto TeamDto = new TeamDto()
            {
                TeamID = Team.TeamID,
                TeamName = Team.TeamName,
                TeamCountry = Team.TeamCountry,
                TeamYear = Team.TeamYear
            };
            if (Team == null)
            {
                return NotFound();
            }

            return Ok(TeamDto);
        }

        /// <summary>
        /// Updates a particular Team in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Team ID primary key</param>
        /// <param name="Team">JSON FORM DATA of an Team</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/TeamData/UpdateTeam/5
        /// FORM DATA: Team JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateTeam(int id, Team Team)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Team.TeamID)
            {

                return BadRequest();
            }

            db.Entry(Team).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamExists(id))
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
        /// Adds an Team to the system
        /// </summary>
        /// <param name="Team">JSON FORM DATA of an Team</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Team ID, Team Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/TeamData/AddTeam
        /// FORM DATA: Team JSON Object
        /// </example>
        [ResponseType(typeof(Team))]
        [HttpPost]
        public IHttpActionResult AddTeam(Team Team)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Teams.Add(Team);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Team.TeamID }, Team);
        }

        /// <summary>
        /// Deletes an Team from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Team</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/TeamData/DeleteTeam/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Team))]
        [HttpPost]
        public IHttpActionResult DeleteTeam(int id)
        {
            Team Team = db.Teams.Find(id);
            if (Team == null)
            {
                return NotFound();
            }

            db.Teams.Remove(Team);
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

        private bool TeamExists(int id)
        {
            return db.Teams.Count(e => e.TeamID == id) > 0;
        }
    }
}