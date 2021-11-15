using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace soulsoft_delivery_asp.Models
{
    public class UsuarioApiModel
    {
        public int id { get; set; }
        [Required(ErrorMessage="É necessário informar o nome")]
        public string nome { get; set; }
        [Required(ErrorMessage = "É necessário informar o telefone")]
        public string telefone { get; set; }
        [Required(ErrorMessage = "É necessário informar o email")]
        public string email { get; set; }
        public DateTime dt_ultimo_acesso { get; set; }
        public DateTime dt_cadastro { get; set; }
        public bool ativo { get; set; }
        [Required(ErrorMessage = "É necessário informar a senha")]
        public string senha { get; set; }
        public int empresa_id { get; set; }
        public string empresa { get; set; }
        [Required(ErrorMessage = "É necessário informar o tipo de usuário")]
        public int tipo_usuario_id { get; set; }
        public virtual TipoUsuarioApiModel tipoUsuarioModel { get; set; }
    }
}
