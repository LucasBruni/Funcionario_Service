﻿using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Ponto_Eletronico.Models;
using System.Net.Http;

namespace Ponto_Eletronico.Controllers
{
    public class Funcionario_CargoAPIInternaController : ApiController
    {
        private Ponto_EletronicoEntities db = new Ponto_EletronicoEntities();

        public Funcionario_CargoAPIInternaController()
        {
            //Solução para conseguir enviar uma entity
            db.Configuration.ProxyCreationEnabled = false;
        }

        // GET: api/Funcionario_CargoAPIInterna
        // Método para buscar todos os Funcionarios_cargos.
        public IQueryable<Funcionario_Cargo> GetFuncionario_Cargo()
        {
            return db.Funcionario_Cargo;
        }

        // GET: api/Funcionario_CargoAPIInterna/5
        // Método para retornar um Funcionario_Cargo pelo id.
        [ResponseType(typeof(Funcionario_Cargo))]
        public IHttpActionResult GetFuncionario_Cargo(int id)
        {
            Request = new System.Net.Http.HttpRequestMessage();
            Configuration = new HttpConfiguration();
            Funcionario_Cargo funcionario_Cargo = db.Funcionario_Cargo.Find(id);
            if (funcionario_Cargo == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Funcionário_Cargo não encontrado"));
            }

            return ResponseMessage(Request.CreateResponse<Funcionario_Cargo>(HttpStatusCode.OK, funcionario_Cargo));
        }

        // GET: api/Funcionario_CargoAPIInterna/5
        // Retorna todos os Cargos de um funcionário
        [ResponseType(typeof(Funcionario_Cargo))]
        public IQueryable<Funcionario_Cargo> GetTodosCargosDeFuncionario(int id_Funcionario)
        {
            return db.Funcionario_Cargo.Where(f => f.id_Funcionario == id_Funcionario);
        }

        // PUT: api/Funcionario_CargoAPIInterna/5
        // Método para alterar um Funcionario_cargo.
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFuncionario_Cargo(int id, Funcionario_Cargo funcionario_Cargo)
        {
            Request = new System.Net.Http.HttpRequestMessage();
            Configuration = new HttpConfiguration();

            if (!ModelState.IsValid)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Modelo incorreto."));
            }

            if (id != funcionario_Cargo.Id)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Id enviada diferente da id do funcionario_cargo."));
            }

            db.Entry(funcionario_Cargo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Funcionario_CargoExists(id))
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Funcionario_Cargo não encontrado"));
                }
                else
                {
                    throw;
                }
            }

            return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.OK, "Funconario_Cargo alterado com sucesso."));
        }

        // POST: api/Funcionario_CargoAPIInterna
        // Método para criar um Funcionario_Cargo.
        [ResponseType(typeof(Funcionario_Cargo))]
        public IHttpActionResult PostFuncionario_Cargo(Funcionario_Cargo funcionario_Cargo)
        {
            Request = new System.Net.Http.HttpRequestMessage();
            Configuration = new HttpConfiguration();

            if (!ModelState.IsValid)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Modelo incorreto."));
            }

            db.Funcionario_Cargo.Add(funcionario_Cargo);
            db.SaveChanges();

            return ResponseMessage(Request.CreateResponse<Funcionario_Cargo>(HttpStatusCode.OK, funcionario_Cargo));
        }

        // DELETE: api/Funcionario_CargoAPIInterna/5
        // Método para deletar um Funcionario_Cargo.
        [ResponseType(typeof(Funcionario_Cargo))]
        public IHttpActionResult DeleteFuncionario_Cargo(int id)
        {
            Request = new System.Net.Http.HttpRequestMessage();
            Configuration = new HttpConfiguration();
            Funcionario_Cargo funcionario_Cargo = db.Funcionario_Cargo.Find(id);

            if (funcionario_Cargo == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Funcionário_Cargo não encontrado"));
            }

            db.Funcionario_Cargo.Remove(funcionario_Cargo);
            db.SaveChanges();

            return ResponseMessage(Request.CreateResponse<Funcionario_Cargo>(HttpStatusCode.OK, funcionario_Cargo));
        }

        // Método para deletar um Funcionario_Cargo atráves do id_funcionario e id_cargo.
        [ResponseType(typeof(Funcionario_Cargo))]
        public IHttpActionResult DeleteFuncionario_Cargo(int id_funcionario, int id_cargo)
        {
            Request = new System.Net.Http.HttpRequestMessage();
            Configuration = new HttpConfiguration();
            Funcionario_Cargo funcionario_Cargo = db.Funcionario_Cargo.Where(f => f.id_Funcionario == id_funcionario && f.id_Cargo == id_cargo).FirstOrDefault();

            if (funcionario_Cargo == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Funcionário_Cargo não encontrado"));
            }

            db.Funcionario_Cargo.Remove(funcionario_Cargo);
            db.SaveChanges();

            return ResponseMessage(Request.CreateResponse<Funcionario_Cargo>(HttpStatusCode.OK, funcionario_Cargo));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Funcionario_CargoExists(int id)
        {
            return db.Funcionario_Cargo.Count(e => e.Id == id) > 0;
        }
    }
}