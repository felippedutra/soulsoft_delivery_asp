﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace soulsoft_delivery_asp.Models
{
    public class LoginApiModel
    {
        [Required(ErrorMessage ="Informe o Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Informe a Senha")]
        public string Senha { get; set; }
    }
}
