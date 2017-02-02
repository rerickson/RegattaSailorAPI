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
using RegattaSailorAPI.DAL;
using RegattaSailorAPI.Models;

namespace RegattaSailorAPI.Controllers
{
    public class RaceController : ApiController
    {
        private RegattaSailorContext db = new RegattaSailorContext();

        // GET: api/Race
        public IQueryable<RaceSummaryModel> GetRaces()
        {
            return db.Races.Select(r => new RaceSummaryModel() { Id = r.Id, Name = r.Name, StartTime = r.StartTime });
        }

        // GET: api/Race/5
        [ResponseType(typeof(RaceModel))]
        public IHttpActionResult GetRaceModel(Guid id)
        {

            RaceModel raceModel = db.Races
                .Include(r => r.Divisions.Select(d => d.Yachts))
                .Include(r => r.Legs)
                .Where(r => r.Id == id)
                .SingleOrDefault(r => r.Id == id);

            if (raceModel == null)
            {
                return NotFound();
            }

            return Ok(raceModel);
        }

        // PUT: api/Race/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRaceModel(Guid id, RaceModel raceModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != raceModel.Id)
            {
                return BadRequest();
            }

            db.Entry(raceModel).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RaceModelExists(id))
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

        // POST: api/Race
        [ResponseType(typeof(RaceModel))]
        public IHttpActionResult PostRaceModel(RaceModel raceModel)
        {

            raceModel.Id = Guid.NewGuid();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Races.Add(raceModel);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (RaceModelExists(raceModel.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = raceModel.Id }, raceModel);
        }

        // DELETE: api/Race/5
        [ResponseType(typeof(RaceModel))]
        public IHttpActionResult DeleteRaceModel(Guid id)
        {
            RaceModel raceModel = db.Races.Find(id);
            if (raceModel == null)
            {
                return NotFound();
            }

            db.Races.Remove(raceModel);
            db.SaveChanges();

            return Ok(raceModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RaceModelExists(Guid id)
        {
            return db.Races.Count(e => e.Id == id) > 0;
        }
    }
}