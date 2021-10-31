﻿using Microsoft.AspNetCore.Mvc;
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
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace soulsoft_delivery_asp.Controllers
{

    public class LoginController : Controller
    {
        //Constante contexto API
        private readonly HttpClient _httpClient;
        //Definindo o nome das variaveis de sessão
        const string SessionToken = "_token";
        const string SessionNome = "_nome";
        const string SessionTipoAcessoId = "_tipoAcessoId";
        const string SessionEmpresaId = "_empresaId";

        public LoginController()
        {
            //Preparando contexto para consumir API
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
        public IActionResult Index([Bind("usuario", "Senha")] LoginApi LoginApi)
        {
            if (ModelState.IsValid)
            {
                string json = JsonConvert.SerializeObject(LoginApi);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync("Seguranca/Login", httpContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    string resposta = response.Content.ReadAsStringAsync().Result;
                    dynamic retorno = JsonConvert.DeserializeObject(resposta);
                    string token = retorno.token;
                    if (token != "")
                    {
                        //Criando as variaveis de sessão
                        HttpContext.Session.SetString(SessionToken, token);
                        //HttpContext.Session.SetString(SessionNome, "");
                        //HttpContext.Session.SetString(SessionTipoAcessoId, "");
                        //HttpContext.Session.SetString(SessionEmpresaId, "");

                        //Capturando as variaveis de sessão
                        //HttpContextAccessor.HttpContext.Session.GetString("_token")

                        return Redirect("/Home/Index");
                    }
                    ViewData["LoginMessage"] = "Não foi possível autenticar, tente novamente.";
                }
                ViewData["LoginMessage"] = "Email ou senha incorreta.";
            }
            return View(LoginApi);
        }
    }
}
