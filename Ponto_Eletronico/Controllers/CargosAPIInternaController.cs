using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Ponto_Eletronico.Models;

namespace Ponto_Eletronico.Controllers
{
    public class CargosAPIInternaController : ApiController
    {
        private Ponto_EletronicoEntities db = new Ponto_EletronicoEntities();

        // GET: api/CargosAPIInterna
        public IQueryable<Cargo> GetCargo()
        {
            return db.Cargo;
        }

        // GET: api/CargosAPIInterna/5
        [ResponseType(typeof(Cargo))]
        public IHttpActionResult GetCargo(int id)
        {
            Cargo cargo = db.Cargo.Find(id);
            if (cargo == null)
            {
                return NotFound();
            }

            return Ok(cargo);
        }

        // PUT: api/CargosAPIInterna/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCargo(int id, Cargo cargo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cargo.Id)
            {
                return BadRequest();
            }

            db.Entry(cargo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CargoExists(id))
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

        // POST: api/CargosAPIInterna
        [ResponseType(typeof(Cargo))]
        public IHttpActionResult PostCargo(Cargo cargo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Cargo.Add(cargo);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = cargo.Id }, cargo);
        }

        // DELETE: api/CargosAPIInterna/5
        [ResponseType(typeof(Cargo))]
        public IHttpActionResult DeleteCargo(int id)
        {
            Cargo cargo = db.Cargo.Find(id);
            if (cargo == null)
            {
                return NotFound();
            }

            db.Cargo.Remove(cargo);
            db.SaveChanges();

            return Ok(cargo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CargoExists(int id)
        {
            return db.Cargo.Count(e => e.Id == id) > 0;
        }
    }
}