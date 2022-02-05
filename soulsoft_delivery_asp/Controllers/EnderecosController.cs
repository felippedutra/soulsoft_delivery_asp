using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using soulsoft_delivery_asp.Models;
using soulsoft_delivery_asp.Services;
using soulsoft_delivery_asp.ViewModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace soulsoft_delivery_asp.Controllers
{
    public class EnderecosController : Controller
    {
        const string LocalUrl = "/Endereco/";

        [HttpGet]
        public async Task<IActionResult> CreateOrEdit(int EnderecoId = 0, int ClienteId = 0, string ClientesMessage = "")
        {
            if (ClientesMessage != "")
            {
                ViewData["ClientesMessage"] = ClientesMessage;
            }

            //Instanciando e inicializando o retorno
            EnderecoViewModel EnderecoViewModel = new EnderecoViewModel();
            //Obtendo o Id da Empresa
            int EmpresaId = (int)HttpContext.Session.GetInt32("_empresaId");

            var TiposEnderecosApi = await HttpClienteApi.NewGetAsync<List<TipoEnderecoApiModel>>("/TipoEndereco/Listar/" + EmpresaId);

            if(TiposEnderecosApi != null)
            {
                EnderecoViewModel.TiposEnderecos = TiposEnderecosApi;
            }

            EnderecoApiModel Endereco = new EnderecoApiModel();

            Endereco.Id = EnderecoId;
            Endereco.ClienteId = ClienteId;

            if (EnderecoId != 0)
            {
                var EnderecoApi = await HttpClienteApi.NewGetAsync<List<EnderecoApiModel>>(LocalUrl + "Listar/" + ClienteId);

                if (EnderecoApi != null)
                {
                    Endereco.Id = EnderecoApi[0].Id;
                    Endereco.Bairro = EnderecoApi[0].Bairro;
                    Endereco.Quadra = EnderecoApi[0].Quadra;
                    Endereco.Numero = EnderecoApi[0].Numero;
                    Endereco.Lote = EnderecoApi[0].Lote;
                    Endereco.Cep = EnderecoApi[0].Cep;
                    Endereco.Rua = EnderecoApi[0].Rua;
                    Endereco.Complemento = EnderecoApi[0].Complemento;
                    Endereco.ClienteId = EnderecoApi[0].ClienteId;
                    Endereco.TipoEnderecoId = EnderecoApi[0].TipoEnderecoId;
                }
            }

            EnderecoViewModel.Endereco = Endereco;

            return View(EnderecoViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit(EnderecoViewModel EnderecoViewModel)
        {
            int EmpresaId = (int)HttpContext.Session.GetInt32("_empresaId");

            if (ModelState.IsValid)
            {
                EnderecoApiModel Endereco = new EnderecoApiModel();
                Endereco = EnderecoViewModel.Endereco;
                Endereco.Ativo = true;

                if (Endereco.Id == 0)
                {
                    var Response = await HttpClienteApi.NewPostAsync<object>(LocalUrl, Endereco);
                    if(Response != null)
                    {
                        return Redirect($"/Clientes/CreateOrEdit?Id={Endereco.ClienteId}&ClientesMessage=Endereço Cadastrado com Sucesso!");
                    }

                }
                else
                {
                    var Response = await HttpClienteApi.NewPutAsync<object>(LocalUrl, Endereco);
                    if (Response != null)
                    {
                        return Redirect($"/Clientes/CreateOrEdit?Id={Endereco.ClienteId}&ClientesMessage=Endereço Alterado com Sucesso!");
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
