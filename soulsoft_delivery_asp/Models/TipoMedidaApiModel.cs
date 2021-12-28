using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace soulsoft_delivery_asp.Models
{
    public class TipoMedidaApiModel
    {
        public int Id { get; set; }
        [StringLength(100)]
        [Required(ErrorMessage ="Informe o Nome")]
        public string Nome { get; set; }
        public DateTime DtCadastro { get; set; }
        public DateTime DtAtualizacao { get; set; }
        public bool Ativo { get; set; }
        public int EmpresaId { get; set; }
    }
}
