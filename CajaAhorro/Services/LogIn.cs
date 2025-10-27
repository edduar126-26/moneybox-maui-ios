using Microsoft.Maui.Networking;
using Money_Box.IService;
using Money_Box.Models;
using Money_Box.Models.Local;
using Money_Box.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Money_Box.Services
{

    public class LogInService : ILogInService
    {
        private readonly HttpClient _httpClient;
        private const string DevHostHeader = "localhost:6463";

        public LogInService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<EntRespuesta> Login(string idEmpresa, string usuario, string contrasenia)
        {
            var url = $"{ApiRest.sBaseUrlWebApi}Acceso/ValidaLogin?idEmpresa={idEmpresa}" +
                      $"&clave={Uri.EscapeDataString(usuario)}&contrasena={Uri.EscapeDataString(contrasenia)}";

            var json = await _httpClient.GetStringAsync(url);
            var obj = JObject.Parse(json);
            var socio = obj["Estado"].Value<bool>() ? obj["Resultado"].ToObject<caSocio>() : new caSocio { Mensaje = "No existe" };

            var respuesta = JsonConvert.DeserializeObject<EntRespuesta>(json);
            respuesta.Resultado = socio;
            return respuesta;
        }

        public async Task<EntRespuesta> Registro(int idEmpresa, int noEmpleado)
        {
            var url = $"{ApiRest.sBaseUrlWebApi}Acceso/ObtenDatosEmpleado?idEmpresa={idEmpresa}&noEmpleado={noEmpleado}";
            var json = await _httpClient.GetStringAsync(url);
            var obj = JObject.Parse(json);
            var socio = obj["Estado"].Value<bool>() ? obj["Resultado"].ToObject<caSocio>() : new caSocio { Mensaje = "No existe" };

            var respuesta = JsonConvert.DeserializeObject<EntRespuesta>(json);
            respuesta.Resultado = socio;
            return respuesta;
        }

        public async Task<RespuestaBoolCompat> ValidaEmailCambioContraseña(string email)
        {
            var url = $"{ApiRest.sBaseUrlWebApi}Acceso/RequestPasswordReset";
            var payload = Newtonsoft.Json.JsonConvert.SerializeObject(new { Email = email?.Trim() });
            var content = new StringContent(payload, System.Text.Encoding.UTF8, "application/json");

            var resp = await SendAsyncWithHost(HttpMethod.Post, url, content);
            resp.EnsureSuccessStatusCode();

            var json = await resp.Content.ReadAsStringAsync();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<RespuestaBoolCompat>(json);
        }


        public async Task<RespuestaBoolCompat> ReiniciarPassword(string token, int uid, string newPassword)
        {
            var url = $"{ApiRest.sBaseUrlWebApi}Acceso/ResetPassword";
            var payload = JsonConvert.SerializeObject(new { Token = token?.Trim(), Uid = uid, NewPassword = newPassword });
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var resp = await SendAsyncWithHost(HttpMethod.Post, url, content);
            resp.EnsureSuccessStatusCode();

            var json = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<RespuestaBoolCompat>(json);
        }



        private async Task<HttpResponseMessage> SendAsyncWithHost(HttpMethod method, string url, HttpContent? content = null)
        {
            var req = new HttpRequestMessage(method, url) { Content = content };
            //req.Headers.Host = DevHostHeader; 

            return await _httpClient.SendAsync(req);
        }
    }

}
