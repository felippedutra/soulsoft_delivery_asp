using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using soulsoft_delivery_asp.Models;
using soulsoft_delivery_asp.Services;
using soulsoft_delivery_asp.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace soulsoft_delivery_asp.Controllers
{
    public class UsuariosController : Controller
    {
        const string LocalUrl = "/Usuario/";

        [HttpGet]
        public async Task<IActionResult> Index(string UsuariosMessage = "")
        {
            if (UsuariosMessage != "")
            {
                ViewData["UsuariosMessage"] = UsuariosMessage;
            }

            //Definindo p retorno padrão
            List<UsuarioApiModel> Usuarios = new List<UsuarioApiModel>();

            //Obtendo o EmpresaId
            int EmpresaId = (int)HttpContext.Session.GetInt32("_empresaId");

            //Requisição da lista de usuários da empresa
            var UsuariosApi = await HttpClienteApi.NewGetAsync<List<UsuarioApiModel>>(LocalUrl + "Listar/" + EmpresaId);

            if (UsuariosApi != null)
            {
                Usuarios = UsuariosApi;
            }

            return View(Usuarios);
        }

        [HttpGet]
        public async Task<IActionResult> CreateOrEdit(int Id = 0)
        {
            //Instancia da View Model de Usuário
            var UsuarioViewModel = new UsuarioViewModel();
            //Iniciando a classe UsuarioViewModel
            UsuarioViewModel.Usuario = new UsuarioApiModel();
            UsuarioViewModel.TiposUsuarios = new List<TipoUsuarioApiModel>();
            //Obtendo o EmpresaId
            int EmpresaId = (int)HttpContext.Session.GetInt32("_empresaId");
            //Capturando a lista de Tipos de Usuários
            var TiposUsuarios = await HttpClienteApi.NewGetAsync<List<TipoUsuarioApiModel>>("/TipoUsuario/Listar/" + EmpresaId);

            UsuarioViewModel.TiposUsuarios = TiposUsuarios;

            if (Id != 0)
            {
                //Requisição do Usuario
                var UsuarioList = await HttpClienteApi.NewGetAsync<List<UsuarioApiModel>>(LocalUrl + Id);
                var Usuario = new UsuarioApiModel
                {
                    Id = UsuarioList[0].Id,
                    Nome = UsuarioList[0].Nome,
                    Telefone = UsuarioList[0].Telefone,
                    Email = UsuarioList[0].Email,
                    Senha = UsuarioList[0].Senha,
                    Ativo = UsuarioList[0].Ativo,
                    EmpresaId = UsuarioList[0].EmpresaId,
                    TipoUsuarioId = UsuarioList[0].TipoUsuarioId
                };
                UsuarioViewModel.Usuario = Usuario;
            }

            return View(UsuarioViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit(UsuarioViewModel UsuarioViewModel)
        {
            if (ModelState.IsValid)
            {
                //Capturando o Usuário
                //UsuarioApiModel Usuario = new UsuarioApiModel();
                var Usuario = UsuarioViewModel.Usuario;

                if (Usuario.Id == 0)
                {
                    //Vinculando o Usuário a empresa
                    Usuario.EmpresaId = (int)HttpContext.Session.GetInt32("_empresaId");
                    //Requisição para cadastro de Novo Usuário
                    var Response = await HttpClienteApi.NewPostAsync<object>(LocalUrl, Usuario);

                    if (Response != null)
                    {
                        return Redirect("/Usuarios/Index?UsuariosMessage=Usuário Cadastrado com Sucesso!");
                    }   
                }
                else
                {
                    //Rquisição de Update de Usuário
                    var Response = await HttpClienteApi.NewPutAsync<object>(LocalUrl, Usuario);

                    if(Response != null)
                    {
                        return Redirect("/Usuarios/Index?UsuariosMessage=Usuário Alterado com Sucesso!");
                    } 
                }
            }

            //Capturando a lista de Tipos de Usuários
            var TiposUsuarios = await HttpClienteApi.NewGetAsync<List<TipoUsuarioApiModel>>("/TipoUsuario/Listar/" + UsuarioViewModel.Usuario.EmpresaId);
            UsuarioViewModel.TiposUsuarios = TiposUsuarios;

            return View(UsuarioViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            //Requisição para Exlusão
            var Response = await HttpClienteApi.NewDeleteAsync<object>(LocalUrl + Id);

            if (Response != null)
            {
                return Redirect("/Usuarios/Index?UsuariosMessage=Usuário removido com Sucesso.");
            }

            return RedirectToAction("Index");
        }
    }

}
