using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using soulsoft_delivery_asp.Models;
using soulsoft_delivery_asp.Services;
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
        const string LocalUrl = "/TipoMedida/";

        [HttpGet]
        public async Task<IActionResult> Index(string TiposMedidasMessage = "")
        {
            if (TiposMedidasMessage != "")
            {
                ViewData["TiposMedidasMessage"] = TiposMedidasMessage;
            }

            //Obtendo o EmpresaId
            int EmpresaId = (int)HttpContext.Session.GetInt32("_empresaId");
            //Criando o objeto de lista
            List<TipoMedidaApiModel> TiposMedidas = new List<TipoMedidaApiModel>();

            var TiposMedidasApi = await HttpClienteApi.NewGetAsync<List<TipoMedidaApiModel>>(LocalUrl + "Listar/" + EmpresaId);

            if (TiposMedidasApi != null)
            {
                TiposMedidas = TiposMedidasApi;
            }

            return View(TiposMedidas);
        }

        [HttpGet]
        public async Task<IActionResult> CreateOrEdit(int Id = 0)
        {
            //Criando o objeto de retorno
            TipoMedidaApiModel TipoMedida = new TipoMedidaApiModel();

            if (Id != 0)
            {
                //Requisição de Tipo de Usuario
                var TipoMedidaApi = await HttpClienteApi.NewGetAsync<List<TipoMedidaApiModel>>(LocalUrl + Id);

                if (TipoMedidaApi != null)
                {
                    TipoMedida.Id = TipoMedidaApi[0].Id;
                    TipoMedida.EmpresaId = TipoMedidaApi[0].EmpresaId;
                    TipoMedida.Nome = TipoMedidaApi[0].Nome;
                    TipoMedida.Ativo = TipoMedidaApi[0].Ativo;
                }
            }

            return View(TipoMedida);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit([Bind("Id", "EmpresaId", "Nome", "Ativo")] TipoMedidaApiModel TipoMedidaApi)
        {
            if (ModelState.IsValid)
            {
                if (TipoMedidaApi.Id == 0)
                {
                    //Vinculando o Usuário a empresa
                    TipoMedidaApi.EmpresaId = (int)HttpContext.Session.GetInt32("_empresaId");
                    //Requisição para cadastro de Novo Tipo de Usuário
                    var Response = await HttpClienteApi.NewPostAsync<object>(LocalUrl, TipoMedidaApi);

                    if (Response != null)
                    {
                        return Redirect("/TiposMedidas/Index?TiposMedidasMessage=Tipo de Medida Cadastrado com Sucesso!");
                    }
                }
                else
                {
                    //Requisição para Alteração de Tipo de Usuário
                    var Response = await HttpClienteApi.NewPutAsync<object>(LocalUrl, TipoMedidaApi);
                    if (Response != null)
                    {
                        return Redirect("/TiposMedidas/Index?TiposMedidasMessage=Tipo de Medida Alterado com Sucesso!");
                    }
                }
            }
            return View(TipoMedidaApi);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id = 0)
        {
            if (Id != 0)
            {
                //Requisição para Exclusão do Tipo de Usuário
                var Response = await HttpClienteApi.NewDeleteAsync<object>(LocalUrl + Id);

                if (Response != null)
                {
                    return Redirect("/TiposMedidas/Index?TiposUsuariosMessage=Tipo de Medida Removido com Sucesso!");
                }
            }
            return RedirectToAction("Index");
        }
    }
}
