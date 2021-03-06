﻿using System.Web.Http;
using Ponto_Eletronico.Models;
using System.Collections.Generic;
using System.Web.Http.Results;
using System;

namespace Ponto_Eletronico.Controllers
{
    public class CargosAPIExternaController : ApiController
    {
        private CargosAPIInternaController apiInterna = new CargosAPIInternaController();

        public IEnumerable<Cargo> GetAllCargos()
        {
            return apiInterna.GetCargo();
        }

        // TODO: Verificar se deveria retorna mensagem de erro ou null.
        public Cargo GetCargos(int id)
        {
            Cargo Cargo = new Cargo();
            IHttpActionResult result;

            try
            {
                result = apiInterna.GetCargo(id);
                if (((ResponseMessageResult)result).Response.IsSuccessStatusCode)
                {
                    Cargo = (Cargo)((System.Net.Http.ObjectContent)(((ResponseMessageResult)result).Response.Content)).Value;
                    return Cargo;
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
        public bool PutCargo(int id, [FromBody]string descricao) {
            IHttpActionResult result;
            Cargo Cargo = new Cargo(id, descricao);

            try
            {
                result = apiInterna.PutCargo(id, Cargo);
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
        [HttpPost]
        public bool PostCargo([FromBody]string descricao)
        {
            IHttpActionResult result;
            Cargo Cargo = new Cargo(descricao);

            try
            {
                result = apiInterna.PostCargo(Cargo);
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
        public bool DeleteCargo(int id) {
            IHttpActionResult result;

            try
            {
                result = apiInterna.DeleteCargo(id);
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