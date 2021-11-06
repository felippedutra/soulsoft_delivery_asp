using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace soulsoft_delivery_asp.Models
{
    public class TipoUsuarioApi
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Informe a Descrição")]
        public string descricao { get; set; }
        [Required(ErrorMessage = "Informe a Situação")]
        public string situacao { get; set; }
    }
}
