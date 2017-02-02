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
    public class DivisionController : ApiController
    {
        private RegattaSailorContext db = new RegattaSailorContext();

        // GET: api/Division
        public IQueryable<DivisionModel> GetDivisions()
        {
            return db.Divisions
                .Include(d => d.Yachts);
        }

        // GET: api/Division/5
        [ResponseType(typeof(DivisionModel))]
        public IHttpActionResult GetDivisionModel(Guid id)
        {
            DivisionModel divisionModel = db.Divisions.Find(id);
            if (divisionModel == null)
            {
                return NotFound();
            }

            return Ok(divisionModel);
        }

        // PUT: api/Division/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDivisionModel(Guid id, DivisionModel divisionModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != divisionModel.Id)
            {
                return BadRequest();
            }
            var division = db.Divisions.Include("Yachts")
                .Single(d => d.Id == divisionModel.Id);

            var guidList = new List<Guid>();

            foreach(var yacht in divisionModel.Yachts)
            {
                guidList.Add(yacht.Id);
            }

            var yachts = db.Yachts
                .Where(y => guidList.Contains(y.Id))
                .ToList();

            division.Yachts.RemoveAll(y => !guidList.Contains(y.Id));
            foreach(var yacht in yachts)
            {
                division.Yachts.Add(yacht);
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DivisionModelExists(id))
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

        public IHttpActionResult PatchDivisionModel(Guid id, DivisionModel divisionModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            divisionModel.Id = id;
            db.Divisions.Attach(divisionModel);
            var division = db.Divisions.Include("Yachts")
                .Single(d => d.Id == divisionModel.Id);

            DbEntityEntry entry = db.Entry(divisionModel);
            foreach( var propertyName in entry.OriginalValues.PropertyNames)
            {
                if (propertyName != "RaceId")
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
                if (!DivisionModelExists(id))
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

        // POST: api/Division
        [ResponseType(typeof(DivisionModel))]
        public IHttpActionResult PostDivisionModel(DivisionModel divisionModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Divisions.Add(divisionModel);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (DivisionModelExists(divisionModel.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = divisionModel.Id }, divisionModel);
        }

        // DELETE: api/Division/5
        [ResponseType(typeof(DivisionModel))]
        public IHttpActionResult DeleteDivisionModel(Guid id)
        {
            DivisionModel divisionModel = db.Divisions.Find(id);
            if (divisionModel == null)
            {
                return NotFound();
            }

            db.Divisions.Remove(divisionModel);
            db.SaveChanges();

            return Ok(divisionModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DivisionModelExists(Guid id)
        {
            return db.Divisions.Count(e => e.Id == id) > 0;
        }
    }
}