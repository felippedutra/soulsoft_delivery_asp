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
    public class ClientesController : Controller
    {
        private readonly HttpClient _httpClient;
        public ClientesController()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.BaseAddress = new System.Uri("https://www.soulsoft.tec.br/api/");
        }

        [HttpGet]
        public IActionResult Index(string ClientesMessage = "")
        {
            if (ClientesMessage != "")
            {
                ViewData["ClientesMessage"] = ClientesMessage;
            }

            HttpResponseMessage response = _httpClient.GetAsync("Cliente/Listar").Result;
            if (response.IsSuccessStatusCode)
            {
                var responseString = response.Content.ReadAsStringAsync().Result;
                dynamic responseJson = JsonConvert.DeserializeObject(responseString);
                if (responseJson.status == "Sucesso")
                {
                    JArray jObject = responseJson.conteudo as JArray;
                    var clientes = jObject.ToObject<List<ClienteApi>>();

                    return View(clientes);
                }
            }
            return View(new List<ClienteApi>());
        }

        [HttpGet]
        public IActionResult CreateOrEdit(int id = 0)
        {
            if (id != 0)
            {
                HttpResponseMessage response = _httpClient.GetAsync($"Cliente/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    dynamic responseJson = JsonConvert.DeserializeObject(responseString);

                    JArray jObject = responseJson.conteudo as JArray;

                    ClienteApi cliente = new ClienteApi
                    {
                        id = responseJson.conteudo[0].id,
                        Nome = responseJson.conteudo[0].nome,
                        Telefone = responseJson.conteudo[0].telefone,
                        email = responseJson.conteudo[0].email,
                        senha = responseJson.conteudo[0].senha,
                        situacao = responseJson.conteudo[0].situacao
                    };

                    return View(cliente);
                }
                else
                {
                    //Retorna usuário não encontrado
                    //return NotFound();
                    return View(new ClienteApi());
                }
            }
            else
            {
                return View(new ClienteApi());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrEdit([Bind("id", "Nome", "Telefone", "email", "senha", "situacao")] ClienteApi ClienteApi)
        {
            if (ModelState.IsValid)
            {
                if (ClienteApi.id == 0)
                {
                    ClienteApi.dt_cadastro = DateTime.Today;

                    string json = JsonConvert.SerializeObject(ClienteApi);
                    var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = _httpClient.PostAsync("Cliente", httpContent).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        dynamic responseJson = JsonConvert.DeserializeObject(responseString);
                        if (responseJson.status == "Sucesso")
                        {
                            return Redirect("/Clientes/Index?ClientesMessage=Cliente cadastrado com Sucesso.");
                        }
                    }
                }
                else
                {
                    string json = JsonConvert.SerializeObject(ClienteApi);
                    var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = _httpClient.PutAsync("Cliente", httpContent).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        dynamic responseJson = JsonConvert.DeserializeObject(responseString);
                        if (responseJson.status == "Sucesso")
                        {
                            return Redirect("/Clientes/Index?ClientesMessage=Cliente alterado com Sucesso.");
                        }
                    }
                }
            }
            return View(ClienteApi);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {

            HttpResponseMessage response = _httpClient.DeleteAsync($"Cliente/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                var responseString = response.Content.ReadAsStringAsync().Result;
                dynamic responseJson = JsonConvert.DeserializeObject(responseString);
                if (responseJson.status == "Sucesso")
                {
                    return Redirect("/Clientes/Index?ClientesMessage=Cliente removido com Sucesso.");
                }
            }
            return RedirectToAction("Index");
        }
    }
}
