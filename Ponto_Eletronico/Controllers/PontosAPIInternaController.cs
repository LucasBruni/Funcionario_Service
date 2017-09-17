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
using Ponto_Eletronico.Models;

namespace Ponto_Eletronico.Controllers
{
    public class PontosAPIInternaController : ApiController
    {
        private Ponto_EletronicoEntities db = new Ponto_EletronicoEntities();

        public PontosAPIInternaController()
        {
            //Solução para conseguir enviar uma entity
            db.Configuration.ProxyCreationEnabled = false;
        }

        // GET: api/PontosAPIInterna
        public IQueryable<Ponto> GetPonto()
        {
            return db.Ponto;
        }

        // GET: api/PontosAPIInterna/5
        [ResponseType(typeof(Ponto))]
        public IHttpActionResult GetPonto(int id)
        {
            Ponto ponto = db.Ponto.Find(id);
            if (ponto == null)
            {
                return NotFound();
            }

            return Ok(ponto);
        }

        // PUT: api/PontosAPIInterna/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPonto(int id, Ponto ponto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ponto.Id)
            {
                return BadRequest();
            }

            db.Entry(ponto).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PontoExists(id))
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

        // POST: api/PontosAPIInterna
        [ResponseType(typeof(Ponto))]
        public IHttpActionResult PostPonto(Ponto ponto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Ponto.Add(ponto);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = ponto.Id }, ponto);
        }

        // DELETE: api/PontosAPIInterna/5
        [ResponseType(typeof(Ponto))]
        public IHttpActionResult DeletePonto(int id)
        {
            Ponto ponto = db.Ponto.Find(id);
            if (ponto == null)
            {
                return NotFound();
            }

            db.Ponto.Remove(ponto);
            db.SaveChanges();

            return Ok(ponto);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PontoExists(int id)
        {
            return db.Ponto.Count(e => e.Id == id) > 0;
        }
    }
}