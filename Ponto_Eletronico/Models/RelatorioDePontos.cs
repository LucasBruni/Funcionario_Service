using System;

namespace Ponto_Eletronico.Models
{
    public class RelatorioDePontos
    {
        public int id_funcionario { get; set; }
        public String nome_funcionario { get; set; }
        public DateTime data { get; set; }
        public TimeSpan horas_trabalhadas { get; set; }
    }
}