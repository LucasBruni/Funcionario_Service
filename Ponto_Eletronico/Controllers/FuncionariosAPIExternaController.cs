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
        private Funcionario_CargoAPIExternaController func_cargoExterna = new Funcionario_CargoAPIExternaController();

        public IEnumerable<Funcionario> GetAllFuncionarios()
        {
            return apiInterna.GetFuncionario();
        }

        // TODO: Verificar se deveria retorna mensagem de erro ou null.
        public Funcionario GetFuncionarios(int id)
        {
            Funcionario funcionario;
            IHttpActionResult result;
            
            try
            {
                result = apiInterna.GetFuncionario(id);
                if (((ResponseMessageResult)result).Response.IsSuccessStatusCode)
                {
                    funcionario = (Funcionario) ((System.Net.Http.ObjectContent)(((ResponseMessageResult)result).Response.Content)).Value;
                    return funcionario;
                }
                else {
                    return null;
                }
            }
            catch(Exception e) {
                return null;
            }
        }

        // TODO: Verificar se deveria retorna mensagem de erro ou null.
        [HttpGet]
        public Funcionario GetLogin(string user, string senha) {
            Funcionario funcionario;
            IHttpActionResult result;
            
            try
            {
                result = apiInterna.GetLogin(user, senha);
                if (((ResponseMessageResult)result).Response.IsSuccessStatusCode)
                {
                    funcionario = (Funcionario)((System.Net.Http.ObjectContent)(((ResponseMessageResult)result).Response.Content)).Value;
                    return funcionario;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        // TODO: Verificar se deveria retorna mensagem de erro ou null.
        [HttpPut]
        public bool PutFuncionario(int id, string nome, string cpf, string email, string usuario, string senha) {
            Funcionario funcionario = new Funcionario(id, nome, cpf, email, usuario, senha);
            IHttpActionResult result;

            try
            {
                result = apiInterna.PutFuncionario(id, funcionario);
                if (((ResponseMessageResult)result).Response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        // TODO: Verificar se deveria retorna mensagem de erro ou null.
        public bool PostFuncionario(string nome, string cpf, string email, string usuario, string senha)
        {
            Funcionario funcionario = new Funcionario(nome, cpf, email, usuario, senha);
            IHttpActionResult result;
            
            try
            {
                result = apiInterna.PostFuncionario(funcionario);
                if (((ResponseMessageResult)result).Response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        // TODO: Verificar se deveria retorna mensagem de erro ou null.
        // TODO: Receber lista de cargos por parâmetro.
        // TODO: Apagar os cargos e recriar.
        //public bool PostFuncionario(string nome, string cpf, string email, string usuario, string senha, List<int> listaCargos_id)
        //{
        //    Funcionario funcionario = new Funcionario(nome, cpf, email, usuario, senha);
        //    Funcionario id_Funcionario = new Funcionario();
        //    IHttpActionResult result;
        //    try
        //    {
        //        result = apiInterna.PostFuncionario(funcionario);
        //        if (((ResponseMessageResult)result).Response.IsSuccessStatusCode)
        //        {
        //            id_Funcionario = (Funcionario)((System.Net.Http.ObjectContent)(((ResponseMessageResult)result).Response.Content)).Value;
        //            foreach (int cargo_id in listaCargos_id)
        //            {
        //                func_cargoExterna.PostFuncionario_Cargo(id_Funcionario.Id, cargo_id);
        //            }
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        public bool DeleteFuncionario(int id) {
            IHttpActionResult result;
            
            try {
                result = apiInterna.DeleteFuncionario(id);

                if (((ResponseMessageResult)result).Response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            } catch {
                return false;
            }
        }

    }
}