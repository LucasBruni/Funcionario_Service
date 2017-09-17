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
    public class FuncionariosAPIInternaController : ApiController
    {
        private Ponto_EletronicoEntities db = new Ponto_EletronicoEntities();

        public FuncionariosAPIInternaController()
        {
            //Solução para conseguir enviar uma entity
            db.Configuration.ProxyCreationEnabled = false;
        }

        // GET: api/FuncionariosAPIInterna
        public IQueryable<Funcionario> GetFuncionario()
        {
            return db.Funcionario;
        }

        // GET: api/FuncionariosAPIInterna/5
        [ResponseType(typeof(Funcionario))]
        public IHttpActionResult GetFuncionario(int id)
        {
            Funcionario funcionario = db.Funcionario.Find(id);
            Request = new System.Net.Http.HttpRequestMessage();
            Configuration = new HttpConfiguration();

            if (funcionario == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Funcionário não encontrado"));
            }
            return ResponseMessage(Request.CreateResponse<Funcionario>(HttpStatusCode.OK, funcionario));
        }

        public IHttpActionResult GetLogin(string user, string senha)
        {
            Funcionario funcionario = new Funcionario();
            Request = new System.Net.Http.HttpRequestMessage();
            Configuration = new HttpConfiguration();

            if (db.Funcionario.Where(i => i.usuario == user).Count() > 0)
            {
                if (db.Funcionario.Where(i => i.usuario == user && i.senha == senha).Count() > 0)
                {
                    funcionario = db.Funcionario.Where(i => i.usuario == user && i.senha == senha).FirstOrDefault();
                }
                else
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Senha incorreta."));
                }
            }
            else {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Usuário incorreto"));
            }
            return ResponseMessage(Request.CreateResponse<Funcionario>(HttpStatusCode.OK, funcionario));
        }

        // PUT: api/FuncionariosAPIInterna/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFuncionario(int id, Funcionario funcionario)
        {
            Request = new System.Net.Http.HttpRequestMessage();
            Configuration = new HttpConfiguration();

            if (!ModelState.IsValid)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Modelo inválido."));
            }

            if (id != funcionario.Id)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "O id enviado é diferente do id do Funcionário."));
            }

            db.Entry(funcionario).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FuncionarioExists(id))
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Funcionário não encontrado."));
                }
                else
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Erro ao salvar o Funcionário."));
                }
            }

            return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.OK, "Funcionário alterado com sucesso."));
        }

        // POST: api/FuncionariosAPIInterna
        [ResponseType(typeof(Funcionario))]
        public IHttpActionResult PostFuncionario(Funcionario funcionario)
        {
            Request = new System.Net.Http.HttpRequestMessage();
            Configuration = new HttpConfiguration();

            if (!ModelState.IsValid)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Modelo inválido."));
            }

            db.Funcionario.Add(funcionario);
            db.SaveChanges();

            return ResponseMessage(Request.CreateResponse<Funcionario>(HttpStatusCode.OK, funcionario));
        }

        // DELETE: api/FuncionariosAPIInterna/5
        [ResponseType(typeof(Funcionario))]
        public IHttpActionResult DeleteFuncionario(int id)
        {
            Funcionario funcionario = db.Funcionario.Find(id);
            Request = new System.Net.Http.HttpRequestMessage();
            Configuration = new HttpConfiguration();

            if (funcionario == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Funcionário não encontrado."));
            }

            db.Funcionario.Remove(funcionario);
            db.SaveChanges();

            return ResponseMessage(Request.CreateResponse<Funcionario>(HttpStatusCode.OK, funcionario));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FuncionarioExists(int id)
        {
            return db.Funcionario.Count(e => e.Id == id) > 0;
        }
    }
}