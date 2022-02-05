using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using soulsoft_delivery_asp.Models;
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
        const string SessionDataTokenGerado = "_dataTokenGerado";
        const string SessionUsuarioId = "_usuarioId";
        const string SessionUsuarioNome = "_usuarioNome";
        const string SessionTipoUsuarioId = "_tipoUsuarioId";
        const string SessionTipoUsuarioNome = "_tipoUsuarioNome";
        const string SessionEmpresaId = "_empresaId";
        const string SessionEmpresaNome = "_empresaNome";

        public LoginController()
        {
            //Preparando contexto para consumir API
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.BaseAddress = new System.Uri("https://www.soulsoft.tec.br/api/");
        }

        // GET
        [HttpGet]
        public IActionResult Index()
        {
            return View(new LoginApiModel());
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index([Bind("Email", "Senha")] LoginApiModel LoginApi)
        {
            if (ModelState.IsValid)
            {
                string json = JsonConvert.SerializeObject(LoginApi);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync("Seguranca/Login", httpContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    dynamic responseJson = JsonConvert.DeserializeObject(responseString);
                    if (responseJson.status == "Sucesso")
                    {
                        string token = responseJson.conteudo[0].token;
                        string dataTokenGerado = responseJson.conteudo[0].dataTokenGerado;
                        int usuarioId = responseJson.conteudo[0].usuarioId;
                        string usuarioNome = responseJson.conteudo[0].usuarioNome;
                        int tipoUsuarioId = responseJson.conteudo[0].tipoUsuarioId;
                        string tipoUsuarioNome = responseJson.conteudo[0].tipoUsuarioNome;
                        int empresaId = responseJson.conteudo[0].empresaId;
                        string empresaNome = responseJson.conteudo[0].empresaNome;

                        HttpContext.Session.SetString(SessionToken, token);
                        HttpContext.Session.SetString(SessionDataTokenGerado, dataTokenGerado);
                        HttpContext.Session.SetInt32(SessionUsuarioId, usuarioId);
                        HttpContext.Session.SetString(SessionUsuarioNome, usuarioNome);
                        HttpContext.Session.SetInt32(SessionTipoUsuarioId, tipoUsuarioId);
                        HttpContext.Session.SetString(SessionTipoUsuarioNome, tipoUsuarioNome);
                        HttpContext.Session.SetInt32(SessionEmpresaId, empresaId);
                        HttpContext.Session.SetString(SessionEmpresaNome, empresaNome);

                        //Capturando as variaveis de sessão
                        //var token = HttpContext.Session.GetString("_token");

                        return Redirect("/Home/Index");
                    }
                    ViewData["LoginMessage"] = "Email ou Senha incorreta.";
                }
                else
                {
                    ViewData["LoginMessage"] = "Não foi possível autenticar, tente novamente.";
                }
            }
            return View(LoginApi);
        }
    }
}
