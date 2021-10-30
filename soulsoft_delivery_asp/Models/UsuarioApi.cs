using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace soulsoft_delivery_asp.Models
{
    public class UsuarioApi
    {
        public int id { get; set; }
        [Required(ErrorMessage="É necessário informar o nome")]
        public string nome { get; set; }
        [Required(ErrorMessage = "É necessário informar o telefone")]
        public string telefone { get; set; }
        [Required(ErrorMessage = "É necessário informar o email")]
        public EmailAddressAttribute email { get; set; }
        public DateTime dt_ultimo_acesso { get; set; }
        public DateTime dt_cadastro { get; set; }
        [Required(ErrorMessage = "É necessário informar a situação")]
        public string situacao { get; set; }
        [Required(ErrorMessage = "É necessário informar a senha")]
        public string senha { get; set; }
        [Required(ErrorMessage = "É necessário informar o tipo de usuário")]
        public int tipo_usuario_id { get; set; }
    }
}
