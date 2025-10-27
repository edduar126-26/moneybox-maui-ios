using Money_Box.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using Money_Box.Views;
using Microsoft.Maui.Networking;
using Newtonsoft.Json.Linq;
using Money_Box.IService;

namespace Money_Box.Services
{
    public class ClienteService : IClienteService
    {
        private readonly HttpClient _httpClient;

        public ClienteService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<EntRespuesta> GetClientes()
        {
            var json = await _httpClient.GetStringAsync(ApiRest.sBaseUrlWebApi + "Clientes/Get");
            var obj = JObject.Parse(json.Replace(@"\n", ""));
            var lst = obj["Resultado"]?.ToObject<List<Clientes>>() ?? new();

            var respuesta = JsonConvert.DeserializeObject<EntRespuesta>(json);
            respuesta.Resultado = lst;
            return respuesta;
        }
    }
}
