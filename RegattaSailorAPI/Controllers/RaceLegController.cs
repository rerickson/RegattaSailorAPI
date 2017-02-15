using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
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
                .Include(l => l.LegResults.Select(lr=>lr.Yacht))
                .Single(l => l.Id == id);
            if (raceLegModel == null)
            {
                return NotFound();
            }

            return Ok(raceLegModel.LegResults);
        }


        // PATCH: api/RaceLeg/5
        public IHttpActionResult PatchRaceLegModel(Guid id, RaceLegModel raceLegModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            raceLegModel.Id = id;
            db.RaceLegs.Attach(raceLegModel);
            var raceLeg = db.RaceLegs
                .Single(d => d.Id == raceLegModel.Id);

            DbEntityEntry entry = db.Entry(raceLegModel);
            foreach (var propertyName in entry.OriginalValues.PropertyNames)
            {
                if (propertyName != "LegResults" && propertyName != "Id")
                {
                    var original = entry.GetDatabaseValues().GetValue<object>(propertyName);
                    var current = entry.CurrentValues.GetValue<object>(propertyName);



                    if (!object.Equals(original, current))
                    {

                        entry.Property(propertyName).IsModified = true;
                    }
                }
            }

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
            raceLegModel.Id = Guid.NewGuid();
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

        // POST: api/RaceLeg/5/Results
        [ResponseType(typeof(LegResultModel))]
        [Route("api/RaceLeg/{id}/Results", Name ="Results")]
        [HttpPost]
        public IHttpActionResult PostLegResultBodyModel(Guid id, [FromBody] LegResultBodyModel legResultBodyModel)
        {
            var legResultModel = new LegResultModel();
            var raceLegModel = db.RaceLegs
                .Include(l => l.LegResults.Select(lr => lr.Yacht))
                .Single(l => l.Id == id);
            var YachtList = raceLegModel.LegResults.Select(lr => lr.Yacht);
            var YachtIdList = YachtList.Select(y => y.Id);

            if (YachtIdList.Contains(legResultBodyModel.YachtId))
            {
                return Conflict();
            }

            legResultModel.Id = Guid.NewGuid();
            legResultModel.Leg = db.RaceLegs.Find(id);
            legResultModel.Yacht = db.Yachts.Find(legResultBodyModel.YachtId);
            legResultModel.StartTime = legResultBodyModel.StartTime;
            legResultModel.EndTime = legResultBodyModel.EndTime;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            db.LegResults.Add(legResultModel);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (RaceLegModelExists(legResultModel.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("Results", new { id = legResultModel.Id }, legResultModel);
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

        // DELETE: api/RaceLeg/5/Results/5
        [ResponseType(typeof(LegResultModel))]
        [Route("api/RaceLeg/{id}/Results/{resultId}", Name = "DeleteResults")]
        [HttpDelete]
        public IHttpActionResult DeleteLegResultModel(Guid id, Guid resultId)
        {
            RaceLegModel raceLegModel = db.RaceLegs
                .Include(rl => rl.LegResults)
                .Single(rl => rl.Id == id);

            if (raceLegModel == null)
            {
                return NotFound();
            }

            LegResultModel legResultModel = db.LegResults
                .Include(lr => lr.Yacht)
                .Single(lr => lr.Id == resultId);
            raceLegModel.LegResults.Remove(legResultModel);
            db.SaveChanges();

            return Ok(legResultModel);
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