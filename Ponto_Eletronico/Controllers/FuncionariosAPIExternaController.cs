using System.Web.Http;
using System.Web.Http.Description;
using Ponto_Eletronico.Models;
using System.Collections.Generic;
using System.Web.Http.Results;
using System;

namespace Ponto_Eletronico.Controllers
{
    public class FuncionariosAPIExternaController : ApiController
    {
        private FuncionariosAPIInternaController apiInterna = new FuncionariosAPIInternaController();

        public IEnumerable<Funcionario> GetAllFuncionarios()
        {
            return apiInterna.GetFuncionario();
        }

        public Funcionario GetFuncionarios(int id)
        {
            Funcionario funcionario = new Funcionario();
            IHttpActionResult result;
            result = apiInterna.GetFuncionario(id);

            try
            {
                funcionario = ((OkNegotiatedContentResult<Funcionario>)result).Content;
            }
            catch(Exception e) {
                return null;
            }
            return funcionario;
        }

        // TODO: melhorar verificações e enviar o motivo de não logar. Ex: Senha inválida. Ex2: Usuário não existe.
        public Funcionario GetLogin(string user, string senha) {
            return apiInterna.GetLogin(user, senha);
        }

        // TODO: Adicionar verificação para quando falhar retornar false.
        public bool PutFuncionario(int id, string nome, string cpf, string email, string usuario, string senha) {
            Funcionario funcionario = new Funcionario(id,nome,cpf,email,usuario,senha);
            try {
                apiInterna.PutFuncionario(id, funcionario);
            }
            catch
            {
                return false;
            }
            
            return true;
        }

        // TODO: Adicionar verificação para quando falhar retornar false.
        public bool PostFuncionario(string nome, string cpf, string email, string usuario, string senha)
        {
            Funcionario funcionario = new Funcionario(nome, cpf, email, usuario, senha);
            try
            {
                apiInterna.PostFuncionario(funcionario);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool DeleteFuncionario(int id) {
            try {
                apiInterna.DeleteFuncionario(id);
            } catch {
                return false;
            }
            
            return true;
        }

    }
}