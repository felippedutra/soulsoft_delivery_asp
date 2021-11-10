using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace soulsoft_delivery_asp.Models
{
    public class TipoUsuarioApiModel
    {
        public int Id { get; set; }
        [StringLength(100)]
        [Required(ErrorMessage = "Informe o Nome")]
        public string Nome { get; set; }
        [StringLength(100)]
        [Required(ErrorMessage = "Informe a Situação")]
        public string Stuacao { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
