using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace soulsoft_delivery_asp.Models
{
    public class LoginApi
    {
        [Required(ErrorMessage ="Informe o Usuário")]
        public string usuario { get; set; }
        [Required(ErrorMessage = "Informe a Senha")]
        public string senha { get; set; }
    }
}
