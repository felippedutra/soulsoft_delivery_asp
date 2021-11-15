using soulsoft_delivery_asp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace soulsoft_delivery_asp.ViewModels
{
    public class UsuarioViewModel
    {
        public UsuarioApiModel Usuario { get; set; }
        public IEnumerable<TipoUsuarioApiModel> TiposUsuarios { get; set; }
    }
}
