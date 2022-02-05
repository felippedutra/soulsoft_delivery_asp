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
using System.Threading.Tasks;

namespace soulsoft_delivery_asp.Controllers
{
    public class TiposProdutosController : Controller
    {
        const string LocalUrl = "/TipoProduto/";

        [HttpGet]
        public async Task<IActionResult> Index(string TiposProdutosMessage = "")
        {
            if (TiposProdutosMessage != "")
            {
                ViewData["TiposProdutosMessage"] = TiposProdutosMessage;
            }

            //Obtendo o EmpresaId
            int EmpresaId = (int)HttpContext.Session.GetInt32("_empresaId");
            //Criando o objeto de lista
            List<TipoProdutoApiModel> TiposProdutos = new List<TipoProdutoApiModel>();

            var TiposProdutosApi = await HttpClienteApi.NewGetAsync<List<TipoProdutoApiModel>>(LocalUrl + "Listar/" + EmpresaId);

            if (TiposProdutosApi != null)
            {
                TiposProdutos = TiposProdutosApi;
            }

            return View(TiposProdutos);
        }

        [HttpGet]
        public async Task<IActionResult> CreateOrEdit(int Id = 0)
        {
            //Criando o objeto de retorno
            TipoProdutoApiModel TipoProduto = new TipoProdutoApiModel();

            if (Id != 0)
            {
                //Requisição de Tipo de Produto
                var TipoProdutoApi = await HttpClienteApi.NewGetAsync<List<TipoProdutoApiModel>>(LocalUrl + Id);

                if (TipoProdutoApi != null)
                {
                    TipoProduto.Id = TipoProdutoApi[0].Id;
                    TipoProduto.EmpresaId = TipoProdutoApi[0].EmpresaId;
                    TipoProduto.Nome = TipoProdutoApi[0].Nome;
                    TipoProduto.Ativo = TipoProdutoApi[0].Ativo;
                }
            }

            return View(TipoProduto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit([Bind("Id", "EmpresaId", "Nome", "Ativo")] TipoProdutoApiModel TipoProdutoApi)
        {
            if (ModelState.IsValid)
            {
                if (TipoProdutoApi.Id == 0)
                {
                    //Vinculando o Usuário a empresa
                    TipoProdutoApi.EmpresaId = (int)HttpContext.Session.GetInt32("_empresaId");
                    //Requisição para cadastro de Novo Tipo de Usuário
                    var Response = await HttpClienteApi.NewPostAsync<object>(LocalUrl, TipoProdutoApi);

                    if (Response != null)
                    {
                        return Redirect("/TiposProdutos/Index?TiposProdutosMessage=Tipo de Produto Cadastrado com Sucesso!");
                    }
                }
                else
                {
                    //Requisição para Alteração de Tipo de Usuário
                    var Response = await HttpClienteApi.NewPutAsync<object>(LocalUrl, TipoProdutoApi);
                    if (Response != null)
                    {
                        return Redirect("/TiposProdutos/Index?TiposProdutosMessage=Tipo de Produto Alterado com Sucesso!");
                    }
                }
            }
            return View(TipoProdutoApi);
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
                    return Redirect("/TiposProdutos/Index?TiposProdutosMessage=Tipo de Produto Removido com Sucesso!");
                }
            }
            return RedirectToAction("Index");
        }
    }
}
