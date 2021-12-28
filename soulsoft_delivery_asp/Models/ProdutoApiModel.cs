using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace soulsoft_delivery_asp.Models
{
    public class ProdutoApiModel
    {
        public int Id { get; set; }
        [StringLength(100)]
        [Required]
        public string Nome { get; set; }
        [StringLength(200)]
        public string Descricao { get; set; }
        public int Quantidade { get; set; }
        [Required]
        public double Valor { get; set; }
        [Required]
        public DateTime DtCadastro { get; set; }
        [Required]
        public DateTime DtAtualizacao { get; set; }
        [Required]
        public bool Ativo { get; set; }
        [Required]
        public int TipoProdutoId { get; set; }
        [Required]
        public int TipoMedidaId { get; set; }
        public int EmpresaId { get; set; }
        public virtual TipoProdutoApiModel TipoProduto { get; set; }
        public virtual TipoMedidaApiModel TipoMedida { get; set; }
    }
}
