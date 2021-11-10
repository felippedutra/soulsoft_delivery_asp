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
    public class UsuariosController : Controller
    {
        private readonly HttpClient _httpClient;
        public UsuariosController()
        {
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
        public IActionResult CreateOrEdit(int id = 0)
        {
            if (id != 0)
            {
                HttpResponseMessage response = _httpClient.GetAsync($"User/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    dynamic responseJson = JsonConvert.DeserializeObject(responseString);

                    JArray jObject = responseJson.conteudo as JArray;

                    UsuarioApiModel usuario = new UsuarioApiModel{
                        id = responseJson.conteudo[0].id,
                        nome = responseJson.conteudo[0].nome,
                        telefone = responseJson.conteudo[0].telefone,
                        email = responseJson.conteudo[0].email,
                        senha = responseJson.conteudo[0].senha,
                        tipo_usuario_id = responseJson.conteudo[0].tipo_usuario_id,
                        situacao = responseJson.conteudo[0].situacao
                    };

                    return View(usuario);
                }
                else
                {
                    //Retorna usuário não encontrado
                    //return NotFound();
                    return View(new UsuarioApiModel());
                }               
            }
            else
            {
                return View(new UsuarioApiModel());
            }    
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrEdit([Bind("id", "nome", "telefone", "email", "senha", "tipo_usuario_id", "situacao")] UsuarioApiModel UsuarioApi)
        {
            if (ModelState.IsValid)
            {
                if (UsuarioApi.id == 0)
                {
                    UsuarioApi.tipoUsuarioModel = null;
                    UsuarioApi.dt_cadastro = DateTime.Today;
                    UsuarioApi.dt_ultimo_acesso = DateTime.Today;

                    string json = JsonConvert.SerializeObject(UsuarioApi);
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
                    string json = JsonConvert.SerializeObject(UsuarioApi);
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

            return View(UsuarioApi);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {

            HttpResponseMessage response = _httpClient.DeleteAsync($"User/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                var responseString = response.Content.ReadAsStringAsync().Result;
                dynamic responseJson = JsonConvert.DeserializeObject(responseString);
                if (responseJson.status == "Sucesso")
                {
                    return Redirect("/Usuarios/Index?UsuariosMessage=Usuário removido com Sucesso.");
                }
            }
            return RedirectToAction("Index");
        }
    }

}
