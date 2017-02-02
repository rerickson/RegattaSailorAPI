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
    public class DivisionYachtController : ApiController
    {
        private RegattaSailorContext db = new RegattaSailorContext();

        // GET: api/DivisionYacht/5
        [ResponseType(typeof(YachtModel[]))]
        public IHttpActionResult GetDivisionModel(Guid id)
        {
            DivisionModel division = db.Divisions.Include("Yachts")
                .Single(d => d.Id == id);

            if (division == null)
            {
                return NotFound();
            }

            return Ok(division.Yachts);
        }

        // PUT: api/DivisionYacht/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDivisionModel(Guid id, YachtModel[] yachtModelList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DivisionModel division = db.Divisions.Include("Yachts")
                .Single(d => d.Id == id);

            var guidList = new List<Guid>();

            foreach (var yacht in yachtModelList)
            {
                guidList.Add(yacht.Id);
            }

            var yachts = db.Yachts
                .Where(y => guidList.Contains(y.Id))
                .ToList();

            division.Yachts.RemoveAll(y => !guidList.Contains(y.Id));
            foreach (var yacht in yachts)
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

        // Patch: api/DivisionYacht/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PatchDivisionModel(Guid id, YachtModel[] yachtModelList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DivisionModel division = db.Divisions.Include("Yachts")
                .Single(d => d.Id == id);

            var guidList = new List<Guid>();

            foreach (var yacht in division.Yachts)
            {
                guidList.Add(yacht.Id);
            }

            foreach (var yachtModel in yachtModelList)
            {
                if (!guidList.Contains(yachtModel.Id))
                {
                    var yacht = db.Yachts
                        .Where(y => y.Id == yachtModel.Id)
                        .FirstOrDefault();
                    division.Yachts.Add(yacht);
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

        // DELETE: api/DivisionYacht/5
        [HttpDelete]
        [ResponseType(typeof(DivisionModel))]
        public IHttpActionResult DeleteDivisionModel(Guid id, [FromUri]Guid yachtId)
        {
            DivisionModel divisionModel = db.Divisions.Include("Yachts")
                .Single(d => d.Id == id);

            var yacht = db.Yachts
                .Where(y => y.Id == yachtId)
                .FirstOrDefault();
            divisionModel.Yachts.Remove(yacht);
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