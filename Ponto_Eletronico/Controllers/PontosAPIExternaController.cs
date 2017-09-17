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

            try
            {
                result = apiInterna.GetPonto(id);
                if (((ResponseMessageResult)result).Response.IsSuccessStatusCode)
                {
                    Ponto = (Ponto)((System.Net.Http.ObjectContent)(((ResponseMessageResult)result).Response.Content)).Value;
                    return Ponto;
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

        // Método para o administrador alterar um registro.
        // TODO: Adicionar verificação para quando falhar retornar false.
        public bool PutPonto(int id, int id_Funcionario, DateTime data_hora_entrada, DateTime data_hora_saida) {
            IHttpActionResult result;
            Ponto Ponto = new Ponto();
            Ponto.Id = id;
            Ponto.id_Funcionario = id_Funcionario;
            Ponto.data_hora_entrada = data_hora_entrada;
            Ponto.data_hora_saida = data_hora_saida;
            
            try
            {
                result = apiInterna.PutPonto(id, Ponto);
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

        // Método para alterar um registro e adicionar o ponto de saída.
        // TODO: Adicionar verificação para quando falhar retornar false.
        public bool PutPontoSaida(int id_Funcionario, DateTime? data_hora_saida = null)
        {
            IHttpActionResult result;
            // Caso não envie a hora de saida, busca a hora
            if (data_hora_saida == null)
            {
                data_hora_saida = DateTime.Now;
            }

            try
            {
                result = apiInterna.PutPontoSaida(id_Funcionario, data_hora_saida);
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

        // Método para adicionar um registro com entrada e saída.
        // TODO: Adicionar verificação para quando falhar retornar false.
        public bool PostPonto(int id_Funcionario, DateTime data_hora_entrada, DateTime data_hora_saida)
        {
            IHttpActionResult result;
            Ponto Ponto = new Ponto();
            Ponto.id_Funcionario = id_Funcionario;
            Ponto.data_hora_entrada = data_hora_entrada;
            Ponto.data_hora_saida = data_hora_saida;
            try
            {
                result = apiInterna.PostPonto(Ponto);
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

        // Método para registrar um ponto de entrada.
        // TODO: Adicionar verificação para quando falhar retornar false.
        public bool PostPontoEntrada(int id_Funcionario, DateTime? data_hora_entrada = null)
        {
            IHttpActionResult result;
            // Caso não envie a hora de entrada, busca a hora
            if (data_hora_entrada == null)
            {
                data_hora_entrada = DateTime.Now;
            }
            try
            {
                result = apiInterna.PostPontoEntrada(id_Funcionario, data_hora_entrada);
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

        public bool DeletePonto(int id)
        {
            IHttpActionResult result;
            try
            {
                result = apiInterna.DeletePonto(id);
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