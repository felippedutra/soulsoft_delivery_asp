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
        public IActionResult CreateOrEdit(int Id = 0)
        {
            if (Id != 0)
            {
                //Instanciando e inicializando o retorno
                ClienteViewModel ClienteViewModel = new ClienteViewModel();
                ClienteViewModel.Cliente = new ClienteApiModel();
                ClienteViewModel.Endereco = new EnderecoApiModel();

                HttpResponseMessage responseCliente = _httpClient.GetAsync($"Cliente/{Id}").Result;
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

                        HttpResponseMessage responseEndereco = _httpClient.GetAsync($"Endereco/Listar/{Id}").Result;
                        if (responseEndereco.IsSuccessStatusCode)
                        {
                            var responseStringEndereco = responseEndereco.Content.ReadAsStringAsync().Result;
                            dynamic responseJsonEndereco = JsonConvert.DeserializeObject(responseStringEndereco);

                            JArray jObjectEndereco = responseJsonEndereco.conteudo as JArray;

                            if (responseJsonEndereco.status == "Sucesso")
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
                return View(ClienteViewModel);
            }
            else
            {
                //Retorna usuário não encontrado
                //return NotFound();
                return View(new ClienteViewModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrEdit(ClienteViewModel ClienteViewModel)
        {
            if (ModelState.IsValid)
            {
                ClienteApiModel Cliente = new ClienteApiModel();
                EnderecoApiModel Endereco = new EnderecoApiModel();

                int EmpresaId = (int)HttpContext.Session.GetInt32("_empresaId");

                Cliente = ClienteViewModel.Cliente;
                Cliente.EmpresaId = EmpresaId;
                Cliente.DtAtualizacao = DateTime.Today;
                Cliente.DtCadastro = DateTime.Today;

                Endereco = ClienteViewModel.Endereco;
                Endereco.DtAtualizacao = DateTime.Today;
                Endereco.DtCadastro = DateTime.Today;

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
                            int ClienteId =(int) responseJsonCliente.conteudo[0];

                            //Preparando Endereco
                            Endereco.ClienteId = ClienteId;
                            Endereco.TipoEnderecoId = 1;
                            Endereco.Ativo = true;
                            string jsonEndereco = JsonConvert.SerializeObject(Endereco);
                            var httpContentEndereco = new StringContent(jsonEndereco, Encoding.UTF8, "application/json");

                            HttpResponseMessage responseEndereco = _httpClient.PostAsync("Endereco", httpContentEndereco).Result;

                            if (responseEndereco.IsSuccessStatusCode)
                            {
                                string responseStringEndereco = responseEndereco.Content.ReadAsStringAsync().Result;
                                dynamic responseJsonEndereco = JsonConvert.DeserializeObject(responseStringEndereco);
                                if (responseJsonEndereco.status == "Sucesso")
                                {
                                    return Redirect($"/Clientes/Index/{EmpresaId}?ClientesMessage=Cliente cadastrado com Sucesso.");
                                }
                            }

                        }
                    }
                }
                else
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
                            int ClienteId = (int)responseJsonCliente.conteudo[0];

                            //Preparando Endereco
                            Endereco.ClienteId = ClienteId;
                            string jsonEndereco = JsonConvert.SerializeObject(Endereco);
                            var httpContentEndereco = new StringContent(jsonEndereco, Encoding.UTF8, "application/json");

                            HttpResponseMessage responseEndereco = _httpClient.PutAsync("Endereco", httpContentEndereco).Result;

                            if (responseEndereco.IsSuccessStatusCode)
                            {
                                string responseStringEndereco = responseEndereco.Content.ReadAsStringAsync().Result;
                                dynamic responseJsonEndereco = JsonConvert.DeserializeObject(responseStringEndereco);
                                if (responseJsonEndereco.status == "Sucesso")
                                {
                                    return Redirect($"/Clientes/Index/{EmpresaId}?ClientesMessage=Cliente alterado com Sucesso.");
                                }
                            }

                        }
                    }
                }
            }
            return View(ClienteViewModel);
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
                    return Redirect("/Clientes/Index?ClientesMessage=Cliente removido com Sucesso.");
                }
            }
            return RedirectToAction("Index");
        }
    }
}
