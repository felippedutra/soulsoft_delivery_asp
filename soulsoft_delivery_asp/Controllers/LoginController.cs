using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using soulsoft_delivery_asp.Models;
using System.IO;
using System.Text;
using Converte_Object_Json;

namespace soulsoft_delivery_asp.Controllers
{

    public class LoginController : Controller
    {

        private readonly HttpClient _httpClient;
        public LoginController()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.BaseAddress = new System.Uri("http://147.182.192.85:8085/api/");
        }

        // GET
        [HttpGet]
        public IActionResult Index()
        {
            return View(new LoginApi());
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index([Bind("usuario", "senha")] LoginApi LoginApi)
        {
            if (ModelState.IsValid)
            {
                //JsonConversao jsonconv = new JsonConversao();
                //dynamic strAlunos = jsonconv.ConverteObjectParaJSon<LoginApi>(usuario);
                //usuario = jsonconv.ConverteJSonParaObject<LoginApi>(strAlunos);


                //string usuarioLogin = @"{ ""usuario"" : ""felipeviadinho@gmail.com"", ""senha"" : ""Teste"" }";
                //dynamic json = JsonConvert.DeserializeObject(usuarioLogin);

                //{
                //    "usuario":"felipeviadinho@gmail.com",
                //    "Senha":"Teste"
                //}


                //HttpResponseMessage response = _httpClient.PostAsync("Seguranca/Login", usuario).Result;

                //if (response.IsSuccessStatusCode)
                //{
                //    string resposta = response.Content.ReadAsStringAsync().Result;
                //    dynamic retorno = JsonConvert.DeserializeObject(resposta);
                //    token = retorno.token;
                //    if (token != "")
                //    {
                //        return Redirect("/Home/Index");
                //    }
                //}

                return Redirect("/Home/Index");
            }

            return View(LoginApi);
        }
    }
}
