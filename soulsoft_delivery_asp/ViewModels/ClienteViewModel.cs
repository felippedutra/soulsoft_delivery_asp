using soulsoft_delivery_asp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace soulsoft_delivery_asp.ViewModels
{
    public class ClienteViewModel
    {
        public ClienteApiModel Cliente { get; set; }
        public EnderecoApiModel Endereco { get; set; }
    }
}
