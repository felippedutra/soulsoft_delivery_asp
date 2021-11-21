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
    public class TiposUsuariosController : Controller
    {
        private readonly HttpClient _httpClient;
        public TiposUsuariosController()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.BaseAddress = new System.Uri("https://www.soulsoft.tec.br/api/");
        }

        [HttpGet]
        public IActionResult Index(string TiposUsuariosMessage = "")
        {
            if (TiposUsuariosMessage != "")
            {
                ViewData["TiposUsuariosMessage"] = TiposUsuariosMessage;
            }

            HttpResponseMessage response = _httpClient.GetAsync("TipoUsuario/Listar").Result;
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
                    var TiposUsuarios = jObject.ToObject<List<TipoUsuarioApiModel>>();

                    return View(TiposUsuarios);
                }
            }
            return View(new List<TipoUsuarioApiModel>());
        }

        [HttpGet]
        public IActionResult CreateOrEdit(int id = 0)
        {
            if (id != 0)
            {
                HttpResponseMessage response = _httpClient.GetAsync($"TipoUsuario/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    dynamic responseJson = JsonConvert.DeserializeObject(responseString);

                    JArray jObject = responseJson.conteudo as JArray;

                    TipoUsuarioApiModel TipoUsuario = new TipoUsuarioApiModel
                    {
                        Id = responseJson.conteudo[0].id,
                        Nome = responseJson.conteudo[0].nome,
                        ativo = responseJson.conteudo[0].ativo
                    };
                    return View(TipoUsuario);
                }
                else
                {
                    //Retorna usuário não encontrado
                    //return NotFound();
                    return View(new TipoUsuarioApiModel());
                }
            }
            else
            {
                return View(new TipoUsuarioApiModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrEdit([Bind("Id", "Nome", "ativo")] TipoUsuarioApiModel TipoUsuarioApi)
        {
            if (ModelState.IsValid)
            {
                if (TipoUsuarioApi.Id == 0)
                {
                    TipoUsuarioApi.DataCadastro = DateTime.Today;

                    string json = JsonConvert.SerializeObject(TipoUsuarioApi);
                    var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = _httpClient.PostAsync("TipoUsuario", httpContent).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        dynamic responseJson = JsonConvert.DeserializeObject(responseString);
                        if (responseJson.status == "Sucesso")
                        {
                            return Redirect("/TiposUsuarios/Index?TiposUsuariosMessage=Tipo de Usuário cadastrado com Sucesso.");
                        }
                    }
                }
                else
                {
                    string json = JsonConvert.SerializeObject(TipoUsuarioApi);
                    var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = _httpClient.PutAsync("TipoUsuario", httpContent).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        dynamic responseJson = JsonConvert.DeserializeObject(responseString);
                        if (responseJson.status == "Sucesso")
                        {
                            return Redirect("/TiposUsuarios/Index?TiposUsuariosMessage=Tipo de Usuário alterado com Sucesso.");
                        }
                    }
                }
            }
            return View(TipoUsuarioApi);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            HttpResponseMessage response = _httpClient.DeleteAsync($"TipoUsuario/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                var responseString = response.Content.ReadAsStringAsync().Result;
                dynamic responseJson = JsonConvert.DeserializeObject(responseString);
                if (responseJson.status == "Sucesso")
                {
                    return Redirect("/TiposUsuarios/Index?TiposUsuariosMessage=Tipo de Usuário removido com Sucesso.");
                }
            }
            return RedirectToAction("Index");
        }
    }
}
