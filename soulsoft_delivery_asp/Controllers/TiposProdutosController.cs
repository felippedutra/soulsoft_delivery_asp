using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using soulsoft_delivery_asp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace soulsoft_delivery_asp.Controllers
{
    public class TiposProdutosController : Controller
    {
        private readonly HttpClient _httpClient;
        public TiposProdutosController()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.BaseAddress = new System.Uri("https://www.soulsoft.tec.br/api/");
        }

        [HttpGet]
        public IActionResult Index(string TiposProdutosMessage = "")
        {
            if (TiposProdutosMessage != "")
            {
                ViewData["TiposProdutosMessage"] = TiposProdutosMessage;
            }

            HttpResponseMessage response = _httpClient.GetAsync("TipoProduto/Listar").Result;
            if (response.IsSuccessStatusCode)
            {
                var responseString = response.Content.ReadAsStringAsync().Result;
                dynamic responseJson = JsonConvert.DeserializeObject(responseString);
                if (responseJson.status == "Sucesso")
                {
                    //Teste 1
                    //dynamic TiposProdutos = JsonConvert.DeserializeObject<List<UsuarioApi>>(responseString.conteudo);

                    //Teste 2
                    JArray jObject = responseJson.conteudo as JArray;
                    var TiposUsuarios = jObject.ToObject<List<TipoProdutoApiModel>>();

                    return View(TiposUsuarios);
                }
            }
            return View(new List<TipoProdutoApiModel>());
        }

        [HttpGet]
        public IActionResult CreateOrEdit(int id = 0)
        {
            if (id != 0)
            {
                return View();
            }
            else
            {
                return View();
            }
        }
    }
}
