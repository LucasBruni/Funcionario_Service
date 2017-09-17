using System.Web.Http;
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

        public Cargo GetCargos(int id)
        {
            Cargo Cargo = new Cargo();
            IHttpActionResult result;
            result = apiInterna.GetCargo(id);

            try
            {
                Cargo = ((OkNegotiatedContentResult<Cargo>)result).Content;
            }
            catch(Exception e) {
                return null;
            }
            return Cargo;
        }

        // TODO: Adicionar verificação para quando falhar retornar false.
        public bool PutCargo(int id, string descricao) {
            Cargo Cargo = new Cargo(id, descricao);
            try {
                apiInterna.PutCargo(id, Cargo);
            }
            catch
            {
                return false;
            }
            
            return true;
        }

        // TODO: Adicionar verificação para quando falhar retornar false.
        public bool PostCargo(string descricao)
        {
            Cargo Cargo = new Cargo(descricao);
            try
            {
                apiInterna.PostCargo(Cargo);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool DeleteCargo(int id) {
            try {
                apiInterna.DeleteCargo(id);
            } catch {
                return false;
            }
            
            return true;
        }

    }
}