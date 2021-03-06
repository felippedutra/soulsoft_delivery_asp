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
        public string Email { get; set; }
        public string Senha { get; set; }
        public DateTime DtCadastro { get; set; }
        public DateTime DtAtualizacao { get; set; }
        public bool Ativo { get; set; }
        public int EmpresaId { get; set; }
    }
}
