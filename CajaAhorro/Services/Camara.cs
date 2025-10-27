using Microsoft.Maui.Networking;
using Money_Box.IService;
using Money_Box.Models;
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
    public class CamaraService : ICamaraService
    {
        private readonly HttpClient _httpClient;

        public CamaraService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> InsertExpedienteAsync(byte[] base64Bytes)
        {
            var body = new StringContent(JsonConvert.SerializeObject(base64Bytes), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(ApiRest.sBaseUrlWebApi + "ExpedienteSocio/InsExpedienteSocio", body);

            return response.IsSuccessStatusCode ? "Ok" : "Error";
        }
    }
}

