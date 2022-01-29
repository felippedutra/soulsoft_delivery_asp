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
    public class TiposUsuariosController : Controller
    {
        const string LocalUrl = "/TipoUsuario/";

        [HttpGet]
        public async Task<IActionResult> Index(string TiposUsuariosMessage = "")
        {
            if (TiposUsuariosMessage != "")
            {
                ViewData["TiposUsuariosMessage"] = TiposUsuariosMessage;
            }

            //Obtendo o EmpresaId
            int EmpresaId = (int)HttpContext.Session.GetInt32("_empresaId");
            //Criando o objeto de lista
            List<TipoUsuarioApiModel> TiposUsuarios = new List<TipoUsuarioApiModel>();

            var UsuariosApi = await HttpClienteApi.NewGetAsync<List<TipoUsuarioApiModel>>(LocalUrl + "Listar/" + EmpresaId);

            if(UsuariosApi != null)
            {
                TiposUsuarios = UsuariosApi;
            }

            return View(TiposUsuarios);
        }

        [HttpGet]
        public async Task<IActionResult> CreateOrEdit(int Id = 0)
        {
            //Criando o objeto de retorno
            TipoUsuarioApiModel TipoUsuario = new TipoUsuarioApiModel();

            if (Id != 0)
            {
                //Requisição de Tipo de Usuario
                var TipoUsuarioApi = await HttpClienteApi.NewGetAsync<List<TipoUsuarioApiModel>>(LocalUrl + Id);

                if (TipoUsuarioApi != null)
                {
                    TipoUsuario.Id = TipoUsuarioApi[0].Id;
                    TipoUsuario.EmpresaId = TipoUsuarioApi[0].EmpresaId;
                    TipoUsuario.Nome = TipoUsuarioApi[0].Nome;
                    TipoUsuario.Ativo = TipoUsuarioApi[0].Ativo;
                }
            }

            return View(TipoUsuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit([Bind("Id", "EmpresaId", "Nome", "Ativo")] TipoUsuarioApiModel TipoUsuarioApi)
        {
            if (ModelState.IsValid)
            {
                if (TipoUsuarioApi.Id == 0)
                {
                    //Vinculando o Usuário a empresa
                    TipoUsuarioApi.EmpresaId = (int)HttpContext.Session.GetInt32("_empresaId");
                    //Requisição para cadastro de Novo Tipo de Usuário
                    var Response = await HttpClienteApi.NewPostAsync<object>(LocalUrl, TipoUsuarioApi);

                    if(Response != null)
                    {
                        return Redirect("/TiposUsuarios/Index?TiposUsuariosMessage=Tipo de Usuário Cadastrado com Sucesso!");
                    }
                }
                else
                {
                    //Requisição para Alteração de Tipo de Usuário
                    var Response = await HttpClienteApi.NewPutAsync<object>(LocalUrl, TipoUsuarioApi);
                    if (Response != null)
                    {
                        return Redirect("/TiposUsuarios/Index?TiposUsuariosMessage=Tipo de Usuário Alterado com Sucesso!");
                    } 
                }
            }
            return View(TipoUsuarioApi);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id = 0)
        {
            if(Id != 0)
            {
                //Requisição para Exclusão do Tipo de Usuário
                var Response = await HttpClienteApi.NewDeleteAsync<object>(LocalUrl + Id);

                if(Response != null)
                {
                    return Redirect("/TiposUsuarios/Index?TiposUsuariosMessage=Tipo de Usuário Removido com Sucesso!");
                }
            }
            return RedirectToAction("Index");
        }
    }
}
