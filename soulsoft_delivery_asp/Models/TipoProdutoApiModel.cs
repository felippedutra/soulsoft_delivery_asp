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
        [StringLength(150)]
        [Required(ErrorMessage ="Informe o Nome")]
        public string Nome { get; set; }
        public DateTime Dt_cadastro { get; set; }
        [StringLength(100)]
        [Required(ErrorMessage = "Informe a Situação")]
        public string Situacao { get; set; }

    }
}
