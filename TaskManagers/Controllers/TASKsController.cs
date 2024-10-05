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
using TaskManagers.Models;

namespace TaskManagers.Controllers
{
    public class TASKsController : ApiController
    {
        private MANAGEREntities db = new MANAGEREntities();

        // GET: api/TASKs
        public IQueryable<TASK> GetTASKs()
        {
            return db.TASKs;
        }

        // GET: api/TASKs/5
        [ResponseType(typeof(TASK))]
        public IHttpActionResult GetTASK(int id)
        {
            TASK tASK = db.TASKs.Find(id);
            if (tASK == null)
            {
                return NotFound();
            }

            return Ok(tASK);
        }

        // PUT: api/TASKs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTASK(int id, TASK tASK)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tASK.TaskID)
            {
                return BadRequest();
            }

            db.Entry(tASK).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TASKExists(id))
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

        // POST: api/TASKs
        [ResponseType(typeof(TASK))]
        public IHttpActionResult PostTASK(TASK tASK)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TASKs.Add(tASK);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (TASKExists(tASK.TaskID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tASK.TaskID }, tASK);
        }

        // DELETE: api/TASKs/5
        [ResponseType(typeof(TASK))]
        public IHttpActionResult DeleteTASK(int id)
        {
            TASK tASK = db.TASKs.Find(id);
            if (tASK == null)
            {
                return NotFound();
            }

            db.TASKs.Remove(tASK);
            db.SaveChanges();

            return Ok(tASK);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TASKExists(int id)
        {
            return db.TASKs.Count(e => e.TaskID == id) > 0;
        }
    }
}