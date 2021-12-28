using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace soulsoft_delivery_asp.Models
{
    public class UsuarioApiModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage="É necessário informar o nome")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "É necessário informar o telefone")]
        public string Telefone { get; set; }
        [Required(ErrorMessage = "É necessário informar o email")]
        public string Email { get; set; }
        public DateTime DtUltimoAcesso { get; set; }
        public DateTime DtCadastro { get; set; }
        public bool Ativo { get; set; }
        [Required(ErrorMessage = "É necessário informar a senha")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
        public int EmpresaId { get; set; }
        [Required(ErrorMessage = "É necessário informar o tipo de usuário")]
        public int TipoUsuarioId { get; set; }
        public virtual TipoUsuarioApiModel TipoUsuario { get; set; }
    }
}
