using System.Web.Http;
using System.Web.Http.Description;
using Ponto_Eletronico.Models;
using System.Collections.Generic;
using System.Web.Http.Results;
using System.Linq;
using System;

namespace Ponto_Eletronico.Controllers
{
    public class FuncionariosAPIExternaController : ApiController
    {
        private FuncionariosAPIInternaController apiInterna = new FuncionariosAPIInternaController();
        private Funcionario_CargoAPIExternaController func_cargoExterna = new Funcionario_CargoAPIExternaController();

        // Método para buscar todos os funcionários.
        public IEnumerable<Funcionario> GetAllFuncionarios()
        {
            return apiInterna.GetFuncionario();
        }

        // Método para buscar o funcionários pelo id.
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

        // Método para efetuar o login no sistema (Apenas Administrador pode logar)
        // TODO: Verificar se deveria retorna mensagem de erro ou null.
        [HttpGet]
        public Funcionario GetLogin(string user, string senha) {
            Funcionario funcionario = new Funcionario();
            IHttpActionResult result;
            
            try
            {
                result = apiInterna.GetLogin(user, senha);
                if (((ResponseMessageResult)result).Response.IsSuccessStatusCode)
                {
                    funcionario = (Funcionario)((System.Net.Http.ObjectContent)(((ResponseMessageResult)result).Response.Content)).Value;
                    // Busca todos os cargos do funcionário.
                    IEnumerable<Funcionario_Cargo> lista = func_cargoExterna.GetTodosCargosDeFuncionario(funcionario.Id);
                    foreach (var item in lista)
                    {
                        // Verifica se ele tem o cargo de Administrador. (Apenas Administrador pode logar)
                        if (item.Cargo.Descricao.ToUpper() == "ADMINISTRADOR")
                        {
                            return funcionario;
                        }
                    }

                    return null;
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

        // Método para verificar se o funcionário possui cargo de Administrador.
        // TODO: Verificar se deveria retorna mensagem de erro ou null.
        public Funcionario VerificaCargoAdmin(string user)
        {
            Funcionario funcionario = new Funcionario();
            IHttpActionResult result;

            try
            {
                result = apiInterna.BuscaFuncionarioPorUsuario(user);
                if (((ResponseMessageResult)result).Response.IsSuccessStatusCode)
                {
                    funcionario = (Funcionario)((System.Net.Http.ObjectContent)(((ResponseMessageResult)result).Response.Content)).Value;
                    // Busca todos os cargos do funcionário.
                    IEnumerable<Funcionario_Cargo> lista = func_cargoExterna.GetTodosCargosDeFuncionario(funcionario.Id);
                    foreach (var item in lista)
                    {
                        // Verifica se ele tem o cargo de Administrador. (Apenas Administrador pode logar)
                        if (item.Cargo.Descricao.ToUpper() == "ADMINISTRADOR")
                        {
                            return funcionario;
                        }
                    }

                    return null;
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

        // Método para alterar um funcionário.
        // TODO: Verificar se deveria retorna mensagem de erro ou null.
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

        // Método para criar um funcionário.
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

        // Método para criar um funcionário com cargos.
        // TODO: Verificar se deveria retorna mensagem de erro ou null.
        // TODO: Receber lista de cargos por parâmetro.
        // TODO: Apagar os cargos e recriar.
        public bool PostFuncionarioComCargos(string nome, string cpf, string email, string usuario, string senha, List<int> lista_nova)
        {
            Funcionario funcionario = new Funcionario(nome, cpf, email, usuario, senha);
            IHttpActionResult result;
            try
            {
                result = apiInterna.PostFuncionario(funcionario);
                if (((ResponseMessageResult)result).Response.IsSuccessStatusCode)
                {
                    // Busca o funcionário que foi inserido
                    funcionario = (Funcionario)((System.Net.Http.ObjectContent)(((ResponseMessageResult)result).Response.Content)).Value;
                    // Busca todos os cargos desse funcionário.
                    IEnumerable<Funcionario_Cargo> listaCargos_antiga;
                    listaCargos_antiga = func_cargoExterna.GetTodosCargosDeFuncionario(funcionario.Id);
                    // Cria uma lista apenas com os ids dos Cargos.
                    List<int> lista_antiga;
                    lista_antiga = (List<int>) listaCargos_antiga.Select(x => x.id_Cargo);

                    List<int> lista_remover;
                    List<int> lista_adicionar;
                    // Os ids dos cargos antigos que não tem na lista nova. (Deletar)
                    lista_remover = lista_antiga.Except(lista_nova).ToList();
                    // Os ids dos cargos novas que não tem na lista antiga (Adicionar)
                    lista_adicionar = lista_nova.Except(lista_antiga).ToList();

                    // Remove os cargos do funcionário
                    foreach (int func_cargo in lista_remover)
                    {
                        func_cargoExterna.DeleteFuncionario_Cargo(funcionario.Id, func_cargo);
                    }

                    // Adiciona os cargos ao funcionário.
                    foreach (int func_cargo in lista_adicionar)
                    {
                        func_cargoExterna.PostFuncionario_Cargo(funcionario.Id, func_cargo);
                    }

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

        // Método para deleter um funcionário.
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