using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
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
    public class UsuariosController : Controller
    {
        private readonly HttpClient _httpClient;

        public UsuariosController()
        {
            //Metodo Client para consumir Api's
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.BaseAddress = new System.Uri("https://www.soulsoft.tec.br/api/");
        }

        [HttpGet]
        public IActionResult Index(string UsuariosMessage = "")
        {
            if (UsuariosMessage != "")
            {
                ViewData["UsuariosMessage"] = UsuariosMessage;
            }

            HttpResponseMessage response = _httpClient.GetAsync("User/Listar").Result;
            if (response.IsSuccessStatusCode)
            {
                var responseString = response.Content.ReadAsStringAsync().Result;
                dynamic responseJson = JsonConvert.DeserializeObject(responseString);
                if (responseJson.status == "Sucesso")
                {
                    //Teste 1
                    //dynamic usuarios = JsonConvert.DeserializeObject<List<UsuarioApi>>(responseString.conteudo);

                    //Teste 2
                    JArray jObject = responseJson.conteudo as JArray;
                    var usuarios = jObject.ToObject<List<UsuarioApiModel>>();

                    return View(usuarios);
                }
            }
            return View(new List<UsuarioApiModel>());
        }

        [HttpGet]
        public IActionResult CreateOrEdit(int Id = 0)
        {
            //Instancia da View Model de Usuário
            var UsuarioViewModel = new UsuarioViewModel();

            if (Id != 0)
            {
                HttpResponseMessage response = _httpClient.GetAsync($"User/{Id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    //Capturando o Usuário
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    dynamic responseJson = JsonConvert.DeserializeObject(responseString);

                    JArray jObject = responseJson.conteudo as JArray;

                    UsuarioApiModel Usuario = new UsuarioApiModel
                    {
                        Id = responseJson.conteudo[0].Id,
                        Nome = responseJson.conteudo[0].Nome,
                        Telefone = responseJson.conteudo[0].Telefone,
                        Email = responseJson.conteudo[0].Email,
                        Senha = responseJson.conteudo[0].Senha,
                        TipoUsuarioId = responseJson.conteudo[0].TipoUsuarioId,
                        Ativo = responseJson.conteudo[0].Ativo
                    };

                    UsuarioViewModel.Usuario = Usuario;

                    //Capturando a lista de Tipos de Usuários
                    HttpResponseMessage responseTipoUsuario = _httpClient.GetAsync("TipoUsuario/Listar").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var responseTipoUsuarioString = responseTipoUsuario.Content.ReadAsStringAsync().Result;
                        dynamic responseTipoUsuarioJson = JsonConvert.DeserializeObject(responseTipoUsuarioString);
                        if (responseTipoUsuarioJson.status == "Sucesso")
                        {
                            JArray jObjectTipoUsuario = responseTipoUsuarioJson.conteudo as JArray;
                            var TiposUsuarios = jObjectTipoUsuario.ToObject<List<TipoUsuarioApiModel>>();

                            UsuarioViewModel.TiposUsuarios = TiposUsuarios;
                        }
                    }

                    return View(UsuarioViewModel);
                }
                else
                {
                    //Retorna usuário não encontrado
                    //return NotFound();
                    return RedirectToAction("Index");
                }               
            }
            else
            {
                UsuarioViewModel.Usuario = new UsuarioApiModel();

                //Capturando a lista de Tipos de Usuários
                HttpResponseMessage responseTipoUsuario = _httpClient.GetAsync("TipoUsuario/Listar").Result;
                if (responseTipoUsuario.IsSuccessStatusCode)
                {
                    var responseTipoUsuarioString = responseTipoUsuario.Content.ReadAsStringAsync().Result;
                    dynamic responseTipoUsuarioJson = JsonConvert.DeserializeObject(responseTipoUsuarioString);
                    if (responseTipoUsuarioJson.status == "Sucesso")
                    {
                        JArray jObjectTipoUsuario = responseTipoUsuarioJson.conteudo as JArray;
                        var TiposUsuarios = jObjectTipoUsuario.ToObject<List<TipoUsuarioApiModel>>();

                        UsuarioViewModel.TiposUsuarios = TiposUsuarios;
                    }
                }

                return View(UsuarioViewModel);
            }    
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrEdit(UsuarioViewModel UsuarioViewModel)
        {
            if (ModelState.IsValid)
            {

                //Capturando o Usuário
                UsuarioApiModel Usuario = new UsuarioApiModel();
                Usuario = UsuarioViewModel.Usuario;

                //Vinculando o Usuário a empresa
                Usuario.EmpresaId =(int) HttpContext.Session.GetInt32("_empresaId");

                if (Usuario.Id == 0)
                {
                    Usuario.DtCadastro = DateTime.Today;
                    Usuario.DtUltimoAcesso = DateTime.Today;

                    string json = JsonConvert.SerializeObject(Usuario);
                    var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = _httpClient.PostAsync("User", httpContent).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        dynamic responseJson = JsonConvert.DeserializeObject(responseString);
                        if (responseJson.status == "Sucesso")
                        {
                            return Redirect("/Usuarios/Index?UsuariosMessage=Usuário cadastrado com Sucesso.");
                        } 
                    }
                }
                else
                {
                    string json = JsonConvert.SerializeObject(Usuario);
                    var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = _httpClient.PutAsync("User", httpContent).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        dynamic responseJson = JsonConvert.DeserializeObject(responseString);
                        if (responseJson.status == "Sucesso")
                        {
                            return Redirect("/Usuarios/Index?UsuariosMessage=Usuário alterado com Sucesso.");
                        }
                    }
                }
            }

            return View(UsuarioViewModel);
        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {

            HttpResponseMessage response = _httpClient.DeleteAsync($"User/{Id}").Result;
            if (response.IsSuccessStatusCode)
            {
                var responseString = response.Content.ReadAsStringAsync().Result;
                dynamic responseJson = JsonConvert.DeserializeObject(responseString);
                if (responseJson.status == "Sucesso")
                {
                    return Redirect("/Usuarios/Index?UsuariosMessage=Usuário removIdo com Sucesso.");
                }
            }
            return RedirectToAction("Index");
        }
    }

}
