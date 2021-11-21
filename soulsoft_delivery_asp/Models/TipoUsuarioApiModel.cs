﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace soulsoft_delivery_asp.Models
{
    public class TipoUsuarioApiModel
    {
        public int Id { get; set; }
        [StringLength(100)]
        [Required(ErrorMessage = "Informe o nome")]
        public string Nome { get; set; }
        public bool ativo { get; set; }
        public DateTime DataCadastro { get; set; }
        public List<UsuarioApiModel> Usuarios { get; set; }
    }
}
