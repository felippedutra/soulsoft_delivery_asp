using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace soulsoft_delivery_asp.Services
{
    public class HttpClienteApi
    {
        public class ApiResponse
        {
            public string Status { get; set; }
            public Object Conteudo { get; set; }
        }

        const string URL = "https://www.soulsoft.tec.br/api";
        public static async Task<string> PostAsync(string UrlObject, object obj)
        {
            string response = null;

            try
            {
                var _json = JsonConvert.SerializeObject(obj);
                var _url = URL + UrlObject;

                HttpClient _client = new HttpClient();
                _client.Timeout = new TimeSpan(0, 0, 15);

                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("applcation/json"));

                HttpResponseMessage _result = null;
                if (!string.IsNullOrEmpty(_url) && _json != null)
                {
                    var _content = new StringContent(_json, Encoding.UTF8, "application/json");

                    _result = await _client.PostAsync(_url, _content);
                }
                response = _result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp);
            }

            return response;
        }

        public static async Task<string> PutAsync(string UrlObject, object obj)
        {
            string response = null;

            try
            {
                var _json = JsonConvert.SerializeObject(obj);
                var _url = URL + UrlObject;

                HttpClient _client = new HttpClient();
                _client.Timeout = new TimeSpan(0, 0, 15);

                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("applcation/json"));

                HttpResponseMessage _result = null;
                if (!string.IsNullOrEmpty(_url) && _json != null)
                {
                    var _content = new StringContent(_json, Encoding.UTF8, "application/json");

                    _result = await _client.PutAsync(_url, _content);
                }
                response = _result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp);
            }

            return response;
        }

        public static async Task<string> GetAsyncNew(string UrlObject)
        {
            string response = null;

            try
            {
                var _url = URL + UrlObject;

                HttpClient _client = new HttpClient();
                _client.Timeout = new TimeSpan(0, 0, 15);

                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("applcation/json"));

                HttpResponseMessage _result = null;
                if (!string.IsNullOrEmpty(_url))
                {
                    _result = await _client.GetAsync(_url);
                }
                response = _result.Content.ReadAsStringAsync().Result;

                Debug.WriteLine("--------fim-----------");
                Debug.WriteLine(response);
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp);
            }

            return response;
        }

        public static async Task<string> DeleteAsyncNew(string UrlObject)
        {
            string response = null;

            try
            {
                var _url = URL + UrlObject;

                HttpClient _client = new HttpClient();
                _client.Timeout = new TimeSpan(0, 0, 15);

                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("applcation/json"));

                HttpResponseMessage _result = null;
                if (!string.IsNullOrEmpty(_url))
                {
                    _result = await _client.DeleteAsync(_url);
                }
                response = _result.Content.ReadAsStringAsync().Result;

                Debug.WriteLine("--------fim-----------");
                Debug.WriteLine(response);
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp);
            }

            return response;
        }

        public static async Task<T> NewPostAsync<T>(string UrlObject, object obj) where T : new()
        {
            string _jsonResposta = await PostAsync(UrlObject, obj);
            ApiResponse _return = JsonConvert.DeserializeObject<ApiResponse>(_jsonResposta);
            if (_return != null)
            {
                if (_return.Status == "Sucesso")
                {
                    try
                    {
                        JArray jObject = _return.Conteudo as JArray;
                        var Lista = jObject.ToObject<T>();
                        return Lista;
                    }
                    catch (Exception ex)
                    {
                        return default(T);
                    }
                }
            }
            return default(T);
        }

        public static async Task<T> NewPutAsync<T>(string UrlObject, object obj) where T : new()
        {
            string _jsonResposta = await PutAsync(UrlObject, obj);
            ApiResponse _return = JsonConvert.DeserializeObject<ApiResponse>(_jsonResposta);
            if (_return != null)
            {
                if (_return.Status == "Sucesso")
                {
                    try
                    {
                        JArray jObject = _return.Conteudo as JArray;
                        var Lista = jObject.ToObject<T>();
                        return Lista;
                    }
                    catch (Exception ex)
                    {
                        return default(T);
                    }
                }
            }
            return default(T);
        }

        public static async Task<T> NewGetAsync<T>(string UrlObject) where T : new()
        {
            string _jsonResposta = await GetAsyncNew(UrlObject);
            ApiResponse _return = JsonConvert.DeserializeObject<ApiResponse>(_jsonResposta);
            if (_return != null)
            {
                if (_return.Status == "Sucesso")
                {
                    try
                    {
                        JArray jObject = _return.Conteudo as JArray;
                        var Lista = jObject.ToObject<T>();
                        return Lista;
                    }
                    catch (Exception ex)
                    {
                        return default(T);
                    }
                }
            }
            return default(T);
        }

        public static async Task<T> NewDeleteAsync<T>(string UrlObject) where T : new()
        {
            string _jsonResposta = await DeleteAsyncNew(UrlObject);
            ApiResponse _return = JsonConvert.DeserializeObject<ApiResponse>(_jsonResposta);
            if (_return != null)
            {
                if (_return.Status == "Sucesso")
                {
                    try
                    {
                        JArray jObject = _return.Conteudo as JArray;
                        var Lista = jObject.ToObject<T>();
                        return Lista;
                    }
                    catch (Exception ex)
                    {
                        return default(T);
                    }
                }
            }
            return default(T);
        }
    }
}
