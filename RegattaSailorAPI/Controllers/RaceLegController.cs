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
    public class RaceLegController : ApiController
    {
        private RegattaSailorContext db = new RegattaSailorContext();

        // GET: api/RaceLeg/5
        [ResponseType(typeof(RaceLegModel))]
        public IHttpActionResult GetRaceLegModel(Guid id)
        {
            RaceLegModel raceLegModel = db.RaceLegs
                .Include(l => l.LegResults.Select(lr => lr.Yacht))
                .Single(l => l.Id == id);
            if (raceLegModel == null)
            {
                return NotFound();
            } 

            return Ok(raceLegModel);
        }

        // GET: api/RaceLeg/5/Results
        [ResponseType(typeof(List<LegResultModel>))]
        [Route("api/RaceLeg/{id}/Results")]
        [HttpGet]
        public IHttpActionResult GetRaceLegModelResults(Guid id)
        {
            RaceLegModel raceLegModel = db.RaceLegs
                .Include(l => l.LegResults)
                .Single(l => l.Id == id);
            if (raceLegModel == null)
            {
                return NotFound();
            }

            return Ok(raceLegModel.LegResults);
        }

        // PUT: api/RaceLeg/5
        [ResponseType(typeof(void))]

        public IHttpActionResult PutRaceLegModel(Guid id, RaceLegModel raceLegModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != raceLegModel.Id)
            {
                return BadRequest();
            }

            db.Entry(raceLegModel).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RaceLegModelExists(id))
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

        // POST: api/RaceLeg
        [ResponseType(typeof(RaceLegModel))]
        public IHttpActionResult PostRaceLegModel(RaceLegModel raceLegModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.RaceLegs.Add(raceLegModel);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (RaceLegModelExists(raceLegModel.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = raceLegModel.Id }, raceLegModel);
        }

        // DELETE: api/RaceLeg/5
        [ResponseType(typeof(RaceLegModel))]
        public IHttpActionResult DeleteRaceLegModel(Guid id)
        {
            RaceLegModel raceLegModel = db.RaceLegs.Find(id);
            if (raceLegModel == null)
            {
                return NotFound();
            }

            db.RaceLegs.Remove(raceLegModel);
            db.SaveChanges();

            return Ok(raceLegModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RaceLegModelExists(Guid id)
        {
            return db.RaceLegs.Count(e => e.Id == id) > 0;
        }
    }
}