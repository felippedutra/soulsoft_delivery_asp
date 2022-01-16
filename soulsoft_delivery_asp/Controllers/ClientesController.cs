using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using soulsoft_delivery_asp.Models;
using soulsoft_delivery_asp.ViewModels;
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

            int EmpresaId = (int)HttpContext.Session.GetInt32("_empresaId");

            HttpResponseMessage response = _httpClient.GetAsync($"Cliente/Listar/{EmpresaId}").Result;
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
        public IActionResult CreateOrEdit(int ClienteId = 0, string ClientesMessage = "")
        {
            if (ClientesMessage != "")
            {
                ViewData["ClientesMessage"] = ClientesMessage;
            }

            //Instanciando e inicializando o retorno
            ClienteViewModel ClienteViewModel = new ClienteViewModel();
            ClienteViewModel.Cliente = new ClienteApiModel();
            ClienteViewModel.Endereco = new EnderecoApiModel();

            if (ClienteId != 0)
            {
                HttpResponseMessage responseCliente = _httpClient.GetAsync($"Cliente/{ClienteId}").Result;
                if (responseCliente.IsSuccessStatusCode)
                {
                    var responseStringCliente = responseCliente.Content.ReadAsStringAsync().Result;
                    dynamic responseJsonCliente = JsonConvert.DeserializeObject(responseStringCliente);

                    JArray jObjectCliente = responseJsonCliente.conteudo as JArray;

                    if (responseJsonCliente.status == "Sucesso")
                    {
                        var ClienteApi = new ClienteApiModel
                        {
                            Id = responseJsonCliente.conteudo[0].id,
                            Nome = responseJsonCliente.conteudo[0].nome,
                            Telefone = responseJsonCliente.conteudo[0].telefone,
                            Email = responseJsonCliente.conteudo[0].email,
                            Senha = responseJsonCliente.conteudo[0].senha,
                            Ativo = responseJsonCliente.conteudo[0].ativo
                        };

                        ClienteViewModel.Cliente = ClienteApi;

                        HttpResponseMessage responseEndereco = _httpClient.GetAsync($"Endereco/Listar/{ClienteId}").Result;
                        if (responseEndereco.IsSuccessStatusCode)
                        {
                            var responseStringEndereco = responseEndereco.Content.ReadAsStringAsync().Result;
                            dynamic responseJsonEndereco = JsonConvert.DeserializeObject(responseStringEndereco);

                            JArray jObjectEndereco = responseJsonEndereco.conteudo as JArray;

                            if (responseJsonEndereco.status == "Sucesso")
                            {
                                if(responseJsonEndereco.conteudo.Count != 0)
                                {
                                    var EnderecoApi = new EnderecoApiModel
                                    {
                                        Id = responseJsonEndereco.conteudo[0].id,
                                        Bairro = responseJsonEndereco.conteudo[0].bairro,
                                        Quadra = responseJsonEndereco.conteudo[0].quadra,
                                        Numero = responseJsonEndereco.conteudo[0].numero,
                                        Lote = responseJsonEndereco.conteudo[0].lote,
                                        Cep = responseJsonEndereco.conteudo[0].cep,
                                        Rua = responseJsonEndereco.conteudo[0].rua,
                                        Complemento = responseJsonEndereco.conteudo[0].complemento
                                    };

                                    ClienteViewModel.Endereco = EnderecoApi;
                                }

                            }
                        }
                    }
                }
            }
            return View(ClienteViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrEdit(ClienteApiModel Cliente)
        {
            if (ModelState.IsValid)
            {
                int EmpresaId = (int)HttpContext.Session.GetInt32("_empresaId");

                Cliente.EmpresaId = EmpresaId;
                Cliente.DtAtualizacao = DateTime.Today;
                Cliente.DtCadastro = DateTime.Today;

                if (Cliente.Id == 0)
                {
                    //Preparando Cliente
                    string jsonCliente = JsonConvert.SerializeObject(Cliente);
                    var httpContentCliente = new StringContent(jsonCliente, Encoding.UTF8, "application/json");

                    HttpResponseMessage responseCliente = _httpClient.PostAsync("Cliente", httpContentCliente).Result;

                    if (responseCliente.IsSuccessStatusCode)
                    {
                        string responseStringCliente = responseCliente.Content.ReadAsStringAsync().Result;
                        dynamic responseJsonCliente = JsonConvert.DeserializeObject(responseStringCliente);
                        if (responseJsonCliente.status == "Sucesso")
                        {
                            int ClienteId = (int)responseJsonCliente.conteudo[0];

                            //Preparando Endereco
                            return Redirect($"/Enderecos/CreateOrEdit?EnderecoId={0}&ClienteId={ClienteId}&ClientesMessage=Cliente Cadastrado com Sucesso! Preencha o Endereço.");
                        }
                    }
                }
                else
                {
                    HttpResponseMessage responseEndereco = _httpClient.GetAsync($"Endereco/Listar/{Cliente.Id}").Result;
                    if (responseEndereco.IsSuccessStatusCode)
                    {
                        var responseStringEndereco = responseEndereco.Content.ReadAsStringAsync().Result;
                        dynamic responseJsonEndereco = JsonConvert.DeserializeObject(responseStringEndereco);

                        JArray jObjectEndereco = responseJsonEndereco.conteudo as JArray;

                        if (responseJsonEndereco.status == "Sucesso")
                        {
                            //Preparando Cliente
                            string jsonCliente = JsonConvert.SerializeObject(Cliente);
                            var httpContentCliente = new StringContent(jsonCliente, Encoding.UTF8, "application/json");

                            HttpResponseMessage responseCliente = _httpClient.PutAsync("Cliente", httpContentCliente).Result;

                            if (responseCliente.IsSuccessStatusCode)
                            {
                                string responseStringCliente = responseCliente.Content.ReadAsStringAsync().Result;
                                dynamic responseJsonCliente = JsonConvert.DeserializeObject(responseStringCliente);
                                if (responseJsonCliente.status == "Sucesso")
                                {
                                    if (responseJsonEndereco.conteudo.Count == 0)
                                    {
                                        return Redirect($"/Enderecos/CreateOrEdit?EnderecoId={0}&ClienteId={Cliente.Id}&ClientesMessage=Cliente Alterado com Sucesso! Preencha o Endereço.");
                                    }

                                    return Redirect($"/Clientes/Index/{EmpresaId}?ClientesMessage=Cliente Alterado com Sucesso!");
                                }
                            }
                        }
                    }
                }
            }
            return View(Cliente);
        }

        [HttpGet]
        public IActionResult Delete(int ClienteId)
        {
            HttpResponseMessage response = _httpClient.DeleteAsync($"Cliente/{ClienteId}").Result;
            if (response.IsSuccessStatusCode)
            {
                var responseString = response.Content.ReadAsStringAsync().Result;
                dynamic responseJson = JsonConvert.DeserializeObject(responseString);
                if (responseJson.status == "Sucesso")
                {
                    return Redirect("/Clientes/Index?ClientesMessage=Cliente Removido com Sucesso!");
                }
            }
            return RedirectToAction("Index");
        }
    }
}
