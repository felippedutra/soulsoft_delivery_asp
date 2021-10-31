using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using soulsoft_delivery_asp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
        public IActionResult Index()
        {
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
                    var usuarios = jObject.ToObject<List<UsuarioApi>>();

                    return View(usuarios);
                }
            }
            return View(new List<UsuarioApi>());
        }

        [HttpGet]
        public IActionResult CreateOrEdit(int id = 0)
        {
            //Teste
            id = 4;
            if (id != 0)
            {
                HttpResponseMessage response = _httpClient.GetAsync($"User/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    string conteudo = response.Content.ReadAsStringAsync().Result;
                    dynamic usuario = JsonConvert.DeserializeObject(conteudo);
                    ViewBag.nome = usuario.conteudo[0].nome;
                    ViewBag.telefone = usuario.conteudo[0].telefone;
                    ViewBag.email = usuario.conteudo[0].email;
                    ViewBag.senha = usuario.conteudo[0].senha;

                    //var usuario = await response.Content.ReadAsStringAsync<UsuarioApi>();

                    //var dados = await response.Content.ReadAsStringAsync();
                    //return JsonConvert.DeserializeObject<UsuarioApi>(dados.conteudo[0]);
                    return View();
                }
                else
                {
                    //Retorna usuário não encontrado
                    //return NotFound();
                    return View();
                }               
            }
            else
            {
                return View(new UsuarioApi());
            }    
        }

        [HttpPost]
        public IActionResult CreateOrEdit()
        {
            //if (ModelState.IsValid)
            //{
            //    if (EmployeeId == 0)
            //        _context.Add(employee);
            //    else
            //        _context.Update(employee);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}

            string nome = "";
            string telefone = "";
            string email = "";

            nome = Request.Form["nome"];
            email = Request.Form["email"];
            telefone = Request.Form["telefone"];

            return View();
        }
    }
}
