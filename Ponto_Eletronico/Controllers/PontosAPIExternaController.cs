using System.Web.Http;
using System.Web.Http.Description;
using Ponto_Eletronico.Models;
using System.Collections.Generic;
using System.Web.Http.Results;
using System;

namespace Ponto_Eletronico.Controllers
{
    public class PontosAPIExternaController : ApiController
    {
        private PontosAPIInternaController apiInterna = new PontosAPIInternaController();

        public IEnumerable<Ponto> GetAllPontos()
        {
            return apiInterna.GetPonto();
        }

        public Ponto GetPontos(int id)
        {
            Ponto Ponto;
            IHttpActionResult result;
            result = apiInterna.GetPonto(id);

            try
            {
                Ponto = ((OkNegotiatedContentResult<Ponto>)result).Content;
            }
            catch(Exception e) {
                return null;
            }
            return Ponto;
        }

        public bool PutPonto(int id, int id_Funcionario, DateTime data_hora_entrada, DateTime data_hora_saida) {
            // TODO: Adicionar verificação para quando falhar retornar false.
            Ponto Ponto = new Ponto();
            Ponto.Id = id;
            Ponto.id_Funcionario = id_Funcionario;
            Ponto.data_hora_entrada = data_hora_entrada;
            Ponto.data_hora_saida = data_hora_saida;
            try {
                apiInterna.PutPonto(id, Ponto);
            }
            catch
            {
                return false;
            }
            
            return true;
        }

        public bool PostPonto(int id_Funcionario, DateTime data_hora_entrada, DateTime data_hora_saida)
        {
            // TODO: Adicionar verificação para quando falhar retornar false.
            Ponto Ponto = new Ponto();
            Ponto.id_Funcionario = id_Funcionario;
            Ponto.data_hora_entrada = data_hora_entrada;
            Ponto.data_hora_saida = data_hora_saida;
            try
            {
                apiInterna.PostPonto(Ponto);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool DeletePonto(int id) {
            try {
                apiInterna.DeletePonto(id);
            } catch {
                return false;
            }
            
            return true;
        }

    }
}