//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ponto_Eletronico.Models
{
    using System.ComponentModel.DataAnnotations;
    using System;
    using System.Collections.Generic;
    
    public partial class Funcionario_Cargo
    {

        public Funcionario_Cargo() { }

        public int Id { get; set; }

        [Display(Name = "Funcionário")]
        public int id_Funcionario { get; set; }

        [Display(Name = "Cargo")]
        public int id_Cargo { get; set; }
    
        public virtual Cargo Cargo { get; set; }
        public virtual Funcionario Funcionario { get; set; }
    }
}
