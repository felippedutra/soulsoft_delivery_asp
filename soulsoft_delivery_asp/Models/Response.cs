using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace soulsoft_delivery_asp.Models
{
    public class Response
    {
        public string status { get; set; }
        public IEnumerable<object> conteudo { get; set; }
    }
}
