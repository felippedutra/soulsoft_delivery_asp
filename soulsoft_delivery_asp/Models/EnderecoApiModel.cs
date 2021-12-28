using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace soulsoft_delivery_asp.Models
{
    public class EnderecoApiModel
    {
        public int Id { get; set; }
        [StringLength(100)]
        [Required(ErrorMessage = "Informe a Rua")]
        public string Rua { get; set; }
        [StringLength(100)]
        public string Quadra { get; set; }
        [StringLength(100)]
        public string Lote { get; set; }
        [StringLength(100)]
        [Required(ErrorMessage = "Informe o Bairro")]
        public string Bairro { get; set; }
        [StringLength(100)]
        public string Numero { get; set; }
        [StringLength(100)]
        public string Cep { get; set; }
        public string Complemento { get; set; }
        public DateTime DtCadastro { get; set; }
        public DateTime DtAtualizacao { get; set; }
        public bool Ativo { get; set; }
        public int ClienteId { get; set; }
        public int TipoEnderecoId { get; set; }
    }
}
