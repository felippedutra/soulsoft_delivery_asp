using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace soulsoft_delivery_asp.Models
{
    public class ClienteApiModel
    {
        public int id { get; set; }
        [Required(ErrorMessage ="Informe o Nome")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Informe o Telefone")]
        public string Telefone { get; set; }
        [Required(ErrorMessage = "Informe o Email")]
        public string email { get; set; }
        [Required(ErrorMessage = "Informe o Senha")]
        public string senha { get; set; }
        public DateTime dt_cadastro { get; set; }
        [Required(ErrorMessage = "Informe o Situação")]
        public string situacao { get; set; }
    }
}
