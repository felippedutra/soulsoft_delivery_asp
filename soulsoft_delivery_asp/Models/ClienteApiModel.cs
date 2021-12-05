using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace soulsoft_delivery_asp.Models
{
    public class ClienteApiModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Informe o Nome")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Informe o Telefone")]
        public string Telefone { get; set; }
        [Required(ErrorMessage = "Informe o Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Informe o Senha")]
        public string Senha { get; set; }
        public DateTime DtCadastro { get; set; }
        [Required(ErrorMessage = "Informe o Situação")]
        public string Ativo { get; set; }
        public int EmpresaId { get; set; }
    }
}
