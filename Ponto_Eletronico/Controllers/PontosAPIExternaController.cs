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
        private FuncionariosAPIExternaController FuncionarioapiExterna = new FuncionariosAPIExternaController();

        // Busca todos os pontos.
        public IEnumerable<Ponto> GetAllPontos()
        {
            return apiInterna.GetPonto();
        }

        // Busca todos os pontos de um funcionário.
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

        // Método para buscar os pontos de um funcionário e montar um relatório.
        // id_Funcionario = busca os pontos do funcionário desejado.
        // usuario = nome de usuário para verificar se o usuário tem permissão(Administrador).
        public IEnumerable<RelatorioDePontos> GetTodosPontosPorFuncionario(int id_funcionario, String usuario)
        {
            if (!(FuncionarioapiExterna.VerificaCargoAdmin(usuario) == null))
            {
                List<RelatorioDePontos> lista = new List<RelatorioDePontos>();
                IEnumerable<Ponto> tpontos;
                tpontos = apiInterna.GetPontoPorFuncionario(id_funcionario);
                foreach (var item in tpontos)
                {
                    RelatorioDePontos rel = new RelatorioDePontos();
                    rel = lista.Find(x => x.id_funcionario == item.id_Funcionario && x.data == item.data_hora_entrada.Date);
                    if (rel == null)
                    {
                        rel = new RelatorioDePontos();
                        rel.id_funcionario = item.id_Funcionario;
                        rel.nome_funcionario = FuncionarioapiExterna.GetFuncionarios(item.id_Funcionario).nome;
                        rel.data = item.data_hora_entrada.Date;
                        if (!(item.data_hora_saida == null))
                        {
                            DateTime saida = (DateTime)item.data_hora_saida;
                            rel.horas_trabalhadas = saida.Subtract(item.data_hora_entrada);
                        }
                        else
                        {
                            // Não tem saída registrada, busca o tempo atual. (Pode causar uma diferença muito grande caso não registre a saída)
                            //rel.horas_trabalhadas = item.data_hora_entrada.Subtract(DateTime.Now);
                        }
                        lista.Add(rel);
                    }
                    else
                    {
                        DateTime saida = (DateTime)item.data_hora_saida;
                        rel.horas_trabalhadas = rel.horas_trabalhadas.Add(saida.Subtract(item.data_hora_entrada));
                    }
                }
                return lista;
            }
            else
            {
                // Apenas usuário admin pode consultar as horas trabalhadas.
                return null;
            }
        }

        // Método para buscar um relatório de todos os funcionários.
        public IEnumerable<RelatorioDePontos> GetRelatorioDePontos(String usuario)
        {
            if (!(FuncionarioapiExterna.VerificaCargoAdmin(usuario) == null))
            {
                List<RelatorioDePontos> lista = new List<RelatorioDePontos>();
                IEnumerable<Ponto> tpontos;
                tpontos = apiInterna.GetPonto();
                foreach (var item in tpontos)
                {
                    RelatorioDePontos rel = new RelatorioDePontos();
                    rel = lista.Find(x => x.id_funcionario == item.id_Funcionario && x.data == item.data_hora_entrada.Date);
                    if (rel == null)
                    {
                        rel = new RelatorioDePontos();
                        rel.id_funcionario = item.id_Funcionario;
                        rel.nome_funcionario = FuncionarioapiExterna.GetFuncionarios(item.id_Funcionario).nome;
                        rel.data = item.data_hora_entrada.Date;
                        if (!(item.data_hora_saida == null))
                        {
                            DateTime saida = (DateTime)item.data_hora_saida;
                            rel.horas_trabalhadas = saida.Subtract(item.data_hora_entrada);
                        }
                        else
                        {
                            // Não tem saída registrada, busca o tempo atual. (Pode causar uma diferença muito grande caso não registre a saída)
                            //rel.horas_trabalhadas = item.data_hora_entrada.Subtract(DateTime.Now);
                        }
                        lista.Add(rel);
                    }
                    else
                    {
                        DateTime saida = (DateTime)item.data_hora_saida;
                        rel.horas_trabalhadas = rel.horas_trabalhadas.Add(saida.Subtract(item.data_hora_entrada));
                    }
                }
                return lista;
            }
            else
            { 
                // Apenas usuário admin pode consultar as horas trabalhadas.
                return null;
            }
        }

        // Método para o administrador alterar um registro.
        // TODO: Verificar se deveria retorna mensagem de erro ou null.
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
        // TODO: Verificar se deveria retorna mensagem de erro ou null.
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
        // TODO: Verificar se deveria retorna mensagem de erro ou null.
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
        // TODO: Verificar se deveria retorna mensagem de erro ou null.
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