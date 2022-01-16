using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace soulsoft_delivery_asp.Models
{
    public class TipoEnderecoApiModel
    {
        public int Id { get; set; }
        [StringLength(100)]
        [Required]
        public string Nome { get; set; }
        [Required]
        public DateTime DtCadastro { get; set; }
        [Required]
        public DateTime DtAtualizacao { get; set; }
        [Required]
        public bool Ativo { get; set; }
        [Required]
        public int EmpresaId { get; set; }
    }
}
