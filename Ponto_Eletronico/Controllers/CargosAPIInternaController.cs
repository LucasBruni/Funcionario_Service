using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Ponto_Eletronico.Models;
using System.Net.Http;

namespace Ponto_Eletronico.Controllers
{
    public class CargosAPIInternaController : ApiController
    {
        private Ponto_EletronicoEntities db = new Ponto_EletronicoEntities();

        public CargosAPIInternaController()
        {
            //Solução para conseguir enviar uma entity
            db.Configuration.ProxyCreationEnabled = false;
        }

        // GET: api/CargosAPIInterna
        public IQueryable<Cargo> GetCargo()
        {
            return db.Cargo;
        }

        // GET: api/CargosAPIInterna/5
        [ResponseType(typeof(Cargo))]
        public IHttpActionResult GetCargo(int id)
        {
            Request = new System.Net.Http.HttpRequestMessage();
            Configuration = new HttpConfiguration();

            Cargo cargo = db.Cargo.Find(id);
            if (cargo == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Cargo não encontrado"));
            }

            return ResponseMessage(Request.CreateResponse<Cargo>(HttpStatusCode.OK, cargo));
        }

        // PUT: api/CargosAPIInterna/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCargo(int id, Cargo cargo)
        {
            Request = new System.Net.Http.HttpRequestMessage();
            Configuration = new HttpConfiguration();

            if (!ModelState.IsValid)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Modelo incorreto."));
            }

            if (id != cargo.Id)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Id enviada diferente da id do cargo."));
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
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Cargo não encontrado"));
                }
                else
                {
                    throw;
                }
            }

            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, "Cargo alterado com sucesso."));
        }

        // POST: api/CargosAPIInterna
        [ResponseType(typeof(Cargo))]
        public IHttpActionResult PostCargo(Cargo cargo)
        {
            Request = new System.Net.Http.HttpRequestMessage();
            Configuration = new HttpConfiguration();

            if (!ModelState.IsValid)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Modelo incorreto."));
            }

            db.Cargo.Add(cargo);
            db.SaveChanges();

            return ResponseMessage(Request.CreateResponse<Cargo>(HttpStatusCode.OK, cargo));
        }

        // DELETE: api/CargosAPIInterna/5
        [ResponseType(typeof(Cargo))]
        public IHttpActionResult DeleteCargo(int id)
        {
            Request = new System.Net.Http.HttpRequestMessage();
            Configuration = new HttpConfiguration();

            Cargo cargo = db.Cargo.Find(id);
            if (cargo == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Cargo não encontrado"));
            }

            db.Cargo.Remove(cargo);
            db.SaveChanges();

            return ResponseMessage(Request.CreateResponse<Cargo>(HttpStatusCode.OK, cargo));
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