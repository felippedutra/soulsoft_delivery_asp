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
    public class EnderecosController : Controller
    {
        private readonly HttpClient _httpClient;
        public EnderecosController()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.BaseAddress = new System.Uri("https://www.soulsoft.tec.br/api/");
        }

        [HttpGet]
        public IActionResult CreateOrEdit(int EnderecoId = 0, int ClienteId = 0, string ClientesMessage = "")
        {
            if (ClientesMessage != "")
            {
                ViewData["ClientesMessage"] = ClientesMessage;
            }

            //Instanciando e inicializando o retorno
            EnderecoViewModel EnderecoViewModel = new EnderecoViewModel();
            EnderecoViewModel.TiposEnderecos = new List<TipoEnderecoApiModel>();

            EnderecoApiModel Endereco = new EnderecoApiModel();

            Endereco.Id = EnderecoId;
            Endereco.ClienteId = ClienteId;

            int EmpresaId = (int)HttpContext.Session.GetInt32("_empresaId");

            HttpResponseMessage responseTiposEnderecos = _httpClient.GetAsync($"TipoEndereco/Listar/{EmpresaId}").Result;
            if (responseTiposEnderecos.IsSuccessStatusCode)
            {
                var responseStringTiposEnderecos = responseTiposEnderecos.Content.ReadAsStringAsync().Result;
                dynamic responseJsonTiposEnderecos = JsonConvert.DeserializeObject(responseStringTiposEnderecos);

                JArray jObjectEndereco = responseJsonTiposEnderecos.conteudo as JArray;

                if (responseJsonTiposEnderecos.status == "Sucesso")
                {
                    //var TipoEndereco = new TipoEnderecoApiModel
                    //{
                    //    Id = responseJsonTiposEnderecos.conteudo[0].id,
                    //    Nome = responseJsonTiposEnderecos.conteudo[0].bairro,
                    //    DtCadastro = responseJsonTiposEnderecos.conteudo[0].quadra,
                    //    DtAtualizacao = responseJsonTiposEnderecos.conteudo[0].numero,
                    //    Ativo = responseJsonTiposEnderecos.conteudo[0].lote,
                    //    EmpresaId = responseJsonTiposEnderecos.conteudo[0].cep,
                    //};

                    var TiposEnderecos = jObjectEndereco.ToObject<List<TipoEnderecoApiModel>>();

                    //ViewData["TiposEnderecos"] = TiposEnderecos;
                    EnderecoViewModel.TiposEnderecos = TiposEnderecos;
                }
            }

            if (EnderecoId != 0)
            {
                HttpResponseMessage responseEndereco = _httpClient.GetAsync($"Endereco/Listar/{ClienteId}").Result;
                if (responseEndereco.IsSuccessStatusCode)
                {
                    var responseStringEndereco = responseEndereco.Content.ReadAsStringAsync().Result;
                    dynamic responseJsonEndereco = JsonConvert.DeserializeObject(responseStringEndereco);

                    JArray jObjectEndereco = responseJsonEndereco.conteudo as JArray;

                    if (responseJsonEndereco.status == "Sucesso")
                    {
                        Endereco.Id = responseJsonEndereco.conteudo[0].id;
                        Endereco.Bairro = responseJsonEndereco.conteudo[0].bairro;
                        Endereco.Quadra = responseJsonEndereco.conteudo[0].quadra;
                        Endereco.Numero = responseJsonEndereco.conteudo[0].numero;
                        Endereco.Lote = responseJsonEndereco.conteudo[0].lote;
                        Endereco.Cep = responseJsonEndereco.conteudo[0].cep;
                        Endereco.Rua = responseJsonEndereco.conteudo[0].rua;
                        Endereco.Complemento = responseJsonEndereco.conteudo[0].complemento;
                        Endereco.ClienteId = responseJsonEndereco.conteudo[0].clienteId;
                        Endereco.TipoEnderecoId = responseJsonEndereco.conteudo[0].tipoEnderecoId;
                    }
                }
            }

            EnderecoViewModel.Endereco = Endereco;

            return View(EnderecoViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrEdit(EnderecoViewModel EnderecoViewModel)
        {
            int EmpresaId = (int)HttpContext.Session.GetInt32("_empresaId");

            if (ModelState.IsValid)
            {
                EnderecoApiModel Endereco = new EnderecoApiModel();
                Endereco = EnderecoViewModel.Endereco;

                Endereco.DtAtualizacao = DateTime.Today;
                Endereco.DtCadastro = DateTime.Today;
                Endereco.Ativo = true;

                if (Endereco.Id == 0)
                {
                    //Preparando Endereco
                    string jsonEndereco = JsonConvert.SerializeObject(Endereco);
                    var httpContentEndereco = new StringContent(jsonEndereco, Encoding.UTF8, "application/json");

                    HttpResponseMessage responseEndereco = _httpClient.PostAsync("Endereco", httpContentEndereco).Result;

                    if (responseEndereco.IsSuccessStatusCode)
                    {
                        string responseStringEndereco = responseEndereco.Content.ReadAsStringAsync().Result;
                        dynamic responseJsonEndereco = JsonConvert.DeserializeObject(responseStringEndereco);
                        if (responseJsonEndereco.status == "Sucesso")
                        {
                            return Redirect($"/Clientes/Index/{EmpresaId}?ClientesMessage=Cadastro Realizado com Sucesso!");
                        }
                    }
                }
                else
                {
                    //Preparando Endereco
                    string jsonEndereco = JsonConvert.SerializeObject(Endereco);
                    var httpContentEndereco = new StringContent(jsonEndereco, Encoding.UTF8, "application/json");

                    HttpResponseMessage responseEndereco = _httpClient.PutAsync("Endereco", httpContentEndereco).Result;

                    if (responseEndereco.IsSuccessStatusCode)
                    {
                        string responseStringEndereco = responseEndereco.Content.ReadAsStringAsync().Result;
                        dynamic responseJsonEndereco = JsonConvert.DeserializeObject(responseStringEndereco);
                        if (responseJsonEndereco.status == "Sucesso")
                        {
                            return Redirect($"/Clientes/CreateOrEdit?ClienteId={Endereco.ClienteId}&ClientesMessage=Endereço Alterado com Sucesso!");
                        }
                    }
                }
            }

            return View(EnderecoViewModel);
        }

        //[HttpGet]
        //public IActionResult Delete(int EnderecoId)
        //{
        //    HttpResponseMessage response = _httpClient.DeleteAsync($"Endereco/{EnderecoId}").Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var responseString = response.Content.ReadAsStringAsync().Result;
        //        dynamic responseJson = JsonConvert.DeserializeObject(responseString);
        //        if (responseJson.status == "Sucesso")
        //        {
        //            return Redirect("/Clientes/Index?ClientesMessage=Cliente removido com Sucesso.");
        //        }
        //    }
        //    return RedirectToAction("Index");
        //}
    }
}
