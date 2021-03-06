﻿using System.Web.Http;
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

        // Método retorna todos os Funcionario_Cargo.
        public IEnumerable<Funcionario_Cargo> GetAllFuncionario_Cargos()
        {
            return apiInterna.GetFuncionario_Cargo();
        }

        // Método retorna um Funcionario_Cargo pelo id.
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
        public IEnumerable<Funcionario_Cargo> GetTodosCargosDeFuncionario(int id_funcionario)
        {
            IEnumerable<Funcionario_Cargo> lista;
            lista = apiInterna.GetTodosCargosDeFuncionario(id_funcionario);
            // Busca e atribui o cargo
            foreach (var item in lista)
            {
                Cargo cargo;
                cargo = CargosApiExterna.GetCargos(item.id_Cargo);
                if(cargo != null)
                {
                    item.Cargo = cargo;
                }
            }
            return lista;
            //return apiInterna.GetTodosCargosDeFuncionario(id_funcionario);
        }

        // Método para alterar um Funcionario_Cargo.
        // TODO: Verificar se deveria retorna mensagem de erro ou null.
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

        // Método para criar um Funcionario_Cargo.
        // TODO: Verificar se deveria retorna mensagem de erro ou null.
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

        // Método para deletar um Funcionario_Cargo.
        // TODO: Verificar se deveria retorna mensagem de erro ou null.
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

        // Método para deletar um Funcionario_Cargo.
        // TODO: Verificar se deveria retorna mensagem de erro ou null.
        public bool DeleteFuncionario_Cargo(int id_funcionario, int id_cargo)
        {
            IHttpActionResult result;

            try
            {
                result = apiInterna.DeleteFuncionario_Cargo(id_funcionario, id_cargo);
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