using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using soulsoft_delivery_asp.Models;
using soulsoft_delivery_asp.Services;
using soulsoft_delivery_asp.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace soulsoft_delivery_asp.Controllers
{
    public class ClientesController : Controller
    {
        const string LocalUrl = "/Cliente/";

        [HttpGet]
        public async Task<IActionResult> Index(string ClientesMessage = "")
        {
            if (ClientesMessage != "")
            {
                ViewData["ClientesMessage"] = ClientesMessage;
            }

            //Definindo p retorno padrão
            List<ClienteApiModel> Clientes = new List<ClienteApiModel>();

            //Obtendo o EmpresaId
            int EmpresaId = (int)HttpContext.Session.GetInt32("_empresaId");

            //Requisição da lista de usuários da empresa
            var ClienteApi = await HttpClienteApi.NewGetAsync<List<ClienteApiModel>>(LocalUrl + "Listar/" + EmpresaId);

            if (ClienteApi != null)
            {
                Clientes = ClienteApi;
            }

            return View(Clientes);
        }

        [HttpGet]
        public async Task<IActionResult> CreateOrEdit(int Id = 0, string ClientesMessage = "")
        {
            if (ClientesMessage != "")
            {
                ViewData["ClientesMessage"] = ClientesMessage;
            }

            //Instanciando e inicializando o retorno
            ClienteViewModel ClienteViewModel = new ClienteViewModel();
            ClienteViewModel.Cliente = new ClienteApiModel();
            ClienteViewModel.Endereco = new EnderecoApiModel();

            if (Id != 0)
            {
                var ClienteApi = await HttpClienteApi.NewGetAsync<List<ClienteApiModel>>(LocalUrl + Id);

                if(ClienteApi != null)
                {
                    ClienteViewModel.Cliente.Id = ClienteApi[0].Id;
                    ClienteViewModel.Cliente.EmpresaId = ClienteApi[0].EmpresaId;
                    ClienteViewModel.Cliente.Nome = ClienteApi[0].Nome;
                    ClienteViewModel.Cliente.Telefone = ClienteApi[0].Telefone;
                    ClienteViewModel.Cliente.Email = ClienteApi[0].Email;
                    ClienteViewModel.Cliente.Senha = ClienteApi[0].Senha;
                    ClienteViewModel.Cliente.Ativo = ClienteApi[0].Ativo;
                }

                var EnderecoApi = await HttpClienteApi.NewGetAsync<List<EnderecoApiModel>>($"/Endereco/Listar/{Id}");

                if(EnderecoApi.Count != 0)
                {
                    ClienteViewModel.Endereco.Id = EnderecoApi[0].Id;
                    ClienteViewModel.Endereco.Bairro = EnderecoApi[0].Bairro;
                    ClienteViewModel.Endereco.Quadra = EnderecoApi[0].Quadra;
                    ClienteViewModel.Endereco.Lote = EnderecoApi[0].Lote;
                    ClienteViewModel.Endereco.Numero = EnderecoApi[0].Numero;
                    ClienteViewModel.Endereco.Rua = EnderecoApi[0].Rua;
                    ClienteViewModel.Endereco.Cep = EnderecoApi[0].Cep;
                    ClienteViewModel.Endereco.Complemento = EnderecoApi[0].Complemento;
                    ClienteViewModel.Endereco.Ativo = EnderecoApi[0].Ativo;
                }
            }
            return View(ClienteViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit(ClienteApiModel Cliente)
        {
            if (ModelState.IsValid)
            {
                //Obtendo o Id da Empresa
                int EmpresaId = (int)HttpContext.Session.GetInt32("_empresaId");
                if (Cliente.Id == 0)
                {
                    //Vinculando o Cliente a Empresa
                    Cliente.EmpresaId = EmpresaId;
                    //Preparando Cliente
                    var Response = await HttpClienteApi.NewPostAsync<object>(LocalUrl, Cliente);

                    if(Response != null)
                    {
                        return Redirect($"/Clientes/Index/{EmpresaId}?ClientesMessage=Cliente Cadastrado com Sucesso!");
                    }

                    //Redireciona para o cadastro de endereco
                    //return Redirect($"/Enderecos/CreateOrEdit?EnderecoId={0}&ClienteId={ClienteId}&ClientesMessage=Cliente Cadastrado com Sucesso! Preencha o Endereço.");
                }
                else
                {
                    //Verifica Endereco
                    //var Response = await HttpClienteApi.NewGetAsync<Object>($"Endereco/Listar/{Cliente.Id}");

                    //if (Response != null)
                    //{
                    //    if (responseJsonEndereco.conteudo.Count == 0)
                    //    {
                    //        return Redirect($"/Enderecos/CreateOrEdit?EnderecoId={0}&ClienteId={Cliente.Id}&ClientesMessage=Cliente Alterado com Sucesso! Preencha o Endereço.");
                    //    }
                    //}

                    var Response = await HttpClienteApi.NewPutAsync<object>(LocalUrl, Cliente);
                    if (Response != null)
                    {
                        return Redirect($"/Clientes/Index/{EmpresaId}?ClientesMessage=Cliente Alterado com Sucesso!");
                    }
                }
            }
            return View(Cliente);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            //Requisição para Exlusão
            var Response = await HttpClienteApi.NewDeleteAsync<object>(LocalUrl + Id);

            if (Response != null)
            {
                return Redirect("/Clientes/Index?ClientesMessage=Cliente Removido com Sucesso!");
            }

            return RedirectToAction("Index");
        }
    }
}
