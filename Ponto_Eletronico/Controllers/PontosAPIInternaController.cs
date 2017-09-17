using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
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
        // TODO: Verificar forma de retornar false para camada externa.
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

        // Método para efetuar o lançamento apenas de saída.
        // TODO: Verificar forma de retornar false para camada externa.
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPontoSaida(int id_Funcionario, DateTime? data_hora_saida)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }                

            try
            {
                // Verifica se é o mesmo funcionário, se existe uma entrada sem saída e se a saída é após a entrada.
                if (db.Ponto.Where(p => p.id_Funcionario == id_Funcionario && p.data_hora_saida == null && p.data_hora_entrada < data_hora_saida).Count() > 0)
                {
                    Ponto ponto = new Ponto();
                    // Busca o registro para adicionar a saída.
                    ponto = db.Ponto.Where(p => p.id_Funcionario == id_Funcionario && p.data_hora_saida == null && p.data_hora_entrada < data_hora_saida).FirstOrDefault();
                    ponto.data_hora_saida = (DateTime) data_hora_saida;
                    db.Entry(ponto).State = EntityState.Modified;
                    db.SaveChanges();

                    return StatusCode(HttpStatusCode.NoContent);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return BadRequest();
        }

        // POST: api/PontosAPIInterna
        // TODO: Verificar forma de retornar false para camada externa.
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

        // Método para registar apenas a entrada.
        // TODO: Verificar forma de retornar false para camada externa.
        [ResponseType(typeof(Ponto))]
        public IHttpActionResult PostPontoEntrada(int id_Funcionario, DateTime? data_hora_entrada)
        {
            Ponto ponto = new Ponto();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Verifica se é o mesmo funcionário e que não exista uma saída pendente.
            if (db.Ponto.Where(p => p.id_Funcionario == id_Funcionario && p.data_hora_saida == null).Count() == 0)
            {
                ponto.id_Funcionario = id_Funcionario;
                ponto.data_hora_entrada = (DateTime) data_hora_entrada;

                db.Ponto.Add(ponto);
                db.SaveChanges();
            }

            return CreatedAtRoute("DefaultApi", new { id = ponto.Id }, ponto);
        }

        // DELETE: api/PontosAPIInterna/5
        // TODO: Verificar forma de retornar false para camada externa.
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