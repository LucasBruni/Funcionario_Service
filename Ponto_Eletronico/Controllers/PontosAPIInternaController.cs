using System;
using System.Data;
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
            Request = new System.Net.Http.HttpRequestMessage();
            Configuration = new HttpConfiguration();
            if (ponto == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Ponto não encontrado"));
            }

            return ResponseMessage(Request.CreateResponse<Ponto>(HttpStatusCode.OK, ponto));
        }

        // PUT: api/PontosAPIInterna/5
        // TODO: Verificar forma de retornar false para camada externa.
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPonto(int id, Ponto ponto)
        {
            Request = new System.Net.Http.HttpRequestMessage();
            Configuration = new HttpConfiguration();

            if (!ModelState.IsValid)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Modelo incorreto."));
            }

            if (id != ponto.Id)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Id enviada diferente da id do ponto."));
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
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Ponto não encontrado"));
                }
                else
                {
                    throw;
                }
            }

            return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.OK, "Ponto alterado com sucesso."));
        }

        // Método para efetuar o lançamento apenas de saída.
        // TODO: Verificar forma de retornar false para camada externa.
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPontoSaida(int id_Funcionario, DateTime? data_hora_saida)
        {
            Request = new System.Net.Http.HttpRequestMessage();
            Configuration = new HttpConfiguration();

            if (!ModelState.IsValid)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Modelo incorreto."));
            }                

            try
            {
                // Verifica se é o mesmo funcionário, se existe uma entrada sem saída e se a saída é após a entrada.
                if (db.Ponto.Where(p => p.id_Funcionario == id_Funcionario && p.data_hora_saida == null && p.data_hora_entrada < data_hora_saida).Count() > 0)
                {
                    Ponto ponto = new Ponto();
                    // Busca o registro para adicionar a saída.
                    ponto = db.Ponto.Where(p => p.id_Funcionario == id_Funcionario && p.data_hora_saida == null && p.data_hora_entrada < data_hora_saida).FirstOrDefault();
                    ponto.data_hora_saida = (DateTime)data_hora_saida;
                    db.Entry(ponto).State = EntityState.Modified;
                    db.SaveChanges();

                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.OK, "Ponto alterado com sucesso."));
                }
                else {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Não foi encontrada nenhuma entrada com saída pendente."));
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        // POST: api/PontosAPIInterna
        // TODO: Verificar forma de retornar false para camada externa.
        [ResponseType(typeof(Ponto))]
        public IHttpActionResult PostPonto(Ponto ponto)
        {
            Request = new System.Net.Http.HttpRequestMessage();
            Configuration = new HttpConfiguration();

            if (!ModelState.IsValid)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Modelo incorreto."));
            }

            db.Ponto.Add(ponto);
            db.SaveChanges();

            return ResponseMessage(Request.CreateResponse<Ponto>(HttpStatusCode.OK, ponto));
        }

        // Método para registar apenas a entrada.
        // TODO: Verificar forma de retornar false para camada externa.
        [ResponseType(typeof(Ponto))]
        public IHttpActionResult PostPontoEntrada(int id_Funcionario, DateTime? data_hora_entrada)
        {
            Ponto ponto = new Ponto();
            Request = new System.Net.Http.HttpRequestMessage();
            Configuration = new HttpConfiguration();
            
            if (!ModelState.IsValid)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Modelo incorreto."));
            }
            // Verifica se é o mesmo funcionário e que não exista uma saída pendente.
            if (db.Ponto.Where(p => p.id_Funcionario == id_Funcionario && p.data_hora_saida == null).Count() == 0)
            {
                ponto.id_Funcionario = id_Funcionario;
                ponto.data_hora_entrada = (DateTime)data_hora_entrada;

                db.Ponto.Add(ponto);
                db.SaveChanges();

                return ResponseMessage(Request.CreateResponse<Ponto>(HttpStatusCode.OK, ponto));
            }
            else {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Existe uma entrada com saída pendente."));
            }
        }

        // DELETE: api/PontosAPIInterna/5
        // TODO: Verificar forma de retornar false para camada externa.
        [ResponseType(typeof(Ponto))]
        public IHttpActionResult DeletePonto(int id)
        {
            Ponto ponto = db.Ponto.Find(id);
            Request = new System.Net.Http.HttpRequestMessage();
            Configuration = new HttpConfiguration();

            if (ponto == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Ponto não encontrado."));
            }

            db.Ponto.Remove(ponto);
            db.SaveChanges();

            return ResponseMessage(Request.CreateResponse<Ponto>(HttpStatusCode.OK, ponto));
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