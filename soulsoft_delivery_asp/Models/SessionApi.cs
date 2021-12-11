using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace soulsoft_delivery_asp.Models
{
    public class SessionApi
    {
        public string token { get; set; }
        public string dataTokenGerado { get; set; }
        public int usuarioId { get; set; }
        public string usuarioNome { get; set; }
        public int tipoUsuarioId { get; set; }
        public string tipoUsuarioNome { get; set; }
        public int empresaId { get; set; }
        public string empresaNome { get; set; }
    }
}
