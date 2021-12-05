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
                    var clientes = jObject.ToObject<List<ClienteApiModel>>();

                    return View(clientes);
                }
            }
            return View(new List<ClienteApiModel>());
        }

        [HttpGet]
        public IActionResult CreateOrEdit(int Id = 0)
        {
            if (Id != 0)
            {
                HttpResponseMessage response = _httpClient.GetAsync($"Cliente/{Id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    dynamic responseJson = JsonConvert.DeserializeObject(responseString);

                    JArray jObject = responseJson.conteudo as JArray;

                    ClienteApiModel cliente = new ClienteApiModel
                    {
                        Id = responseJson.conteudo[0].Id,
                        Nome = responseJson.conteudo[0].Nome,
                        Telefone = responseJson.conteudo[0].telefone,
                        Email = responseJson.conteudo[0].email,
                        Senha = responseJson.conteudo[0].Senha,
                        Ativo = responseJson.conteudo[0].Ativo
                    };

                    return View(cliente);
                }
                else
                {
                    //Retorna usuário não encontrado
                    //return NotFound();
                    return View(new ClienteApiModel());
                }
            }
            else
            {
                return View(new ClienteApiModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrEdit([Bind("Id", "Nome", "Telefone", "email", "Senhaa", "Situacao")] ClienteApiModel ClienteApi)
        {
            if (ModelState.IsValid)
            {
                if (ClienteApi.Id == 0)
                {
                    ClienteApi.DtCadastro = DateTime.Today;

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
        public IActionResult Delete(int Id)
        {

            HttpResponseMessage response = _httpClient.DeleteAsync($"Cliente/{Id}").Result;
            if (response.IsSuccessStatusCode)
            {
                var responseString = response.Content.ReadAsStringAsync().Result;
                dynamic responseJson = JsonConvert.DeserializeObject(responseString);
                if (responseJson.status == "Sucesso")
                {
                    return Redirect("/Clientes/Index?ClientesMessage=Cliente removIdo com Sucesso.");
                }
            }
            return RedirectToAction("Index");
        }
    }
}
