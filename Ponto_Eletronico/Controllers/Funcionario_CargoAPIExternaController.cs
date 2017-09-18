using System.Web.Http;
using Ponto_Eletronico.Models;
using System.Collections.Generic;
using System.Web.Http.Results;
using System;

namespace Ponto_Eletronico.Controllers
{
    public class Funcionario_CargoAPIExternaController : ApiController
    {
        private Funcionario_CargoAPIInternaController apiInterna = new Funcionario_CargoAPIInternaController();
        private CargosAPIExternaController CargosApiExterna = new CargosAPIExternaController();

        public IEnumerable<Funcionario_Cargo> GetAllFuncionario_Cargos()
        {
            return apiInterna.GetFuncionario_Cargo();
        }

        public Funcionario_Cargo GetFuncionario_Cargos(int id)
        {
            Funcionario_Cargo Funcionario_Cargo = new Funcionario_Cargo();
            IHttpActionResult result;

            try
            {
                result = apiInterna.GetFuncionario_Cargo(id);
                if (((ResponseMessageResult)result).Response.IsSuccessStatusCode)
                {
                    Funcionario_Cargo = (Funcionario_Cargo)((System.Net.Http.ObjectContent)(((ResponseMessageResult)result).Response.Content)).Value;
                    return Funcionario_Cargo;
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

        // Retorna todos os Cargos de um funcionário
        public IEnumerable<Funcionario_Cargo> GetAllCargos(int id_funcionario)
        {
            IEnumerable<Funcionario_Cargo> lista;
            lista = apiInterna.GetAllCargos(id_funcionario);
            foreach (var item in lista)
            {
                Cargo cargo;
                cargo = CargosApiExterna.GetCargos(item.id_Cargo);
                if(cargo != null)
                {
                    item.Cargo = cargo;
                }
            }
            return apiInterna.GetAllCargos(id_funcionario);
        }

        // TODO: Adicionar verificação para quando falhar retornar false.
        public bool PutFuncionario_Cargo(int id, int id_Funcionario, int id_Cargo) {
            IHttpActionResult result;
            Funcionario_Cargo Funcionario_Cargo = new Funcionario_Cargo();
            Funcionario_Cargo.Id = id;
            Funcionario_Cargo.id_Funcionario = id_Funcionario;
            Funcionario_Cargo.id_Cargo = id_Cargo;
            try
            {
                result = apiInterna.PutFuncionario_Cargo(id, Funcionario_Cargo);
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

        // TODO: Adicionar verificação para quando falhar retornar false.
        public bool PostFuncionario_Cargo(int id_Funcionario, int id_Cargo)
        {
            IHttpActionResult result;
            Funcionario_Cargo Funcionario_Cargo = new Funcionario_Cargo();
            Funcionario_Cargo.id_Funcionario = id_Funcionario;
            Funcionario_Cargo.id_Cargo = id_Cargo;

            try
            {
                result = apiInterna.PostFuncionario_Cargo(Funcionario_Cargo);
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

        public bool DeleteFuncionario_Cargo(int id) {
            IHttpActionResult result;

            try
            {
                result = apiInterna.DeleteFuncionario_Cargo(id);
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

    }
}