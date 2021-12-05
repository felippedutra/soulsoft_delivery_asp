using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace soulsoft_delivery_asp.Models
{
    public class TipoProdutoApiModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Informe o Nome")]
        [StringLength(150)]
        public string Nome { get; set; }
        public DateTime DtCadastro { get; set; }
        public bool Ativo { get; set; }
        public int EmpresaId { get; set; }
    }
}
