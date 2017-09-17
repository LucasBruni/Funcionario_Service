using System.Web.Http;
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

        // Método para o administrador alterar um registro.
        // TODO: Adicionar verificação para quando falhar retornar false.
        public bool PutPonto(int id, int id_Funcionario, DateTime data_hora_entrada, DateTime data_hora_saida) {
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

        // Método para alterar um registro e adicionar o ponto de saída.
        // TODO: Adicionar verificação para quando falhar retornar false.
        public bool PutPontoSaida(int id_Funcionario, DateTime? data_hora_saida = null)
        {
            // Caso não envie a hora de saida, busca a hora
            if (data_hora_saida == null)
            {
                data_hora_saida = DateTime.Now;
            }
            try
            {
                apiInterna.PutPontoSaida(id_Funcionario , data_hora_saida);
            }
            catch
            {
                return false;
            }

            return true;
        }

        // Método para adicionar um registro com entrada e saída.
        // TODO: Adicionar verificação para quando falhar retornar false.
        public bool PostPonto(int id_Funcionario, DateTime data_hora_entrada, DateTime data_hora_saida)
        {
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

        // Método para registrar um ponto de entrada.
        // TODO: Adicionar verificação para quando falhar retornar false.
        public bool PostPontoEntrada(int id_Funcionario, DateTime? data_hora_entrada = null)
        {
            // Caso não envie a hora de entrada, busca a hora
            if (data_hora_entrada == null)
            {
                data_hora_entrada = DateTime.Now;
            }
            try
            {
                apiInterna.PostPontoEntrada(id_Funcionario,data_hora_entrada);
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