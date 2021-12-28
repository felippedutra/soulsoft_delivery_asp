using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using soulsoft_delivery_asp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace soulsoft_delivery_asp.Controllers
{
    public class TiposMedidasController : Controller
    {
        private readonly HttpClient _httpClient;
        public TiposMedidasController()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.BaseAddress = new System.Uri("https://www.soulsoft.tec.br/api/");
        }

        [HttpGet]
        public IActionResult Index(string TiposMedidasMessage = "")
        {
            if (TiposMedidasMessage != "")
            {
                ViewData["TiposMedidasMessage"] = TiposMedidasMessage;
            }

            //Obtendo o EmpresaId
            int EmpresaId = (int)HttpContext.Session.GetInt32("_empresaId");

            HttpResponseMessage response = _httpClient.GetAsync($"TipoMedida/Listar/{EmpresaId}").Result;
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
                    var TiposMedidas = jObject.ToObject<List<TipoMedidaApiModel>>();

                    return View(TiposMedidas);
                }
            }
            return View(new List<TipoMedidaApiModel>());
        }

        [HttpGet]
        public IActionResult CreateOrEdit(int TipoMedidaId = 0)
        {
            if (TipoMedidaId != 0)
            {
                HttpResponseMessage response = _httpClient.GetAsync($"TipoMedida/{TipoMedidaId}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    dynamic responseJson = JsonConvert.DeserializeObject(responseString);

                    JArray jObject = responseJson.conteudo as JArray;

                    TipoMedidaApiModel TipoMedida = new TipoMedidaApiModel
                    {
                        Id = responseJson.conteudo[0].id,
                        Nome = responseJson.conteudo[0].nome,
                        Ativo = responseJson.conteudo[0].ativo
                    };
                    return View(TipoMedida);
                }
                else
                {
                    //Retorna usuário não encontrado
                    //return NotFound();
                    return View(new TipoMedidaApiModel());
                }
            }
            else
            {
                return View(new TipoMedidaApiModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrEdit([Bind("Id", "Nome", "Ativo")] TipoMedidaApiModel TipoMedidaApi)
        {
            if (ModelState.IsValid)
            {
                //Vinculando o Usuário a empresa
                TipoMedidaApi.EmpresaId = (int)HttpContext.Session.GetInt32("_empresaId");

                if (TipoMedidaApi.Id == 0)
                {
                    TipoMedidaApi.DtCadastro = DateTime.Today;

                    string json = JsonConvert.SerializeObject(TipoMedidaApi);
                    var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = _httpClient.PostAsync("TipoMedida", httpContent).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        dynamic responseJson = JsonConvert.DeserializeObject(responseString);
                        if (responseJson.status == "Sucesso")
                        {
                            return Redirect("/TiposMedidas/Index?TiposMedidasMessage=Tipo de Medida cadastrado com Sucesso.");
                        }
                    }
                }
                else
                {
                    string json = JsonConvert.SerializeObject(TipoMedidaApi);
                    var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = _httpClient.PutAsync("TipoMedida", httpContent).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        dynamic responseJson = JsonConvert.DeserializeObject(responseString);
                        if (responseJson.status == "Sucesso")
                        {
                            return Redirect("/TiposMedidas/Index?TiposMedidasMessage=Tipo de Medida alterado com Sucesso.");
                        }
                    }
                }
            }
            return View(TipoMedidaApi);
        }

        [HttpGet]
        public IActionResult Delete(int TipoMedidaId)
        {
            HttpResponseMessage response = _httpClient.DeleteAsync($"TipoMedida/{TipoMedidaId}").Result;
            if (response.IsSuccessStatusCode)
            {
                var responseString = response.Content.ReadAsStringAsync().Result;
                dynamic responseJson = JsonConvert.DeserializeObject(responseString);
                if (responseJson.status == "Sucesso")
                {
                    return Redirect("/TiposMedidas/Index?TiposMedidasMessage=Tipo de Medida removido com Sucesso.");
                }
            }
            return RedirectToAction("Index");
        }
    }
}
