using Money_Box.Models;
using Money_Box.IService;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Money_Box.Services
{
    public class BancosService : IBancosService
    {
        private readonly HttpClient _httpClient;

        public BancosService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Bancos>> GetBancos()
        {
            var url = ApiRest.sBaseUrlWebApi + "Bancos/Get";
            var json = await _httpClient.GetStringAsync(url);
            var bancos = JsonConvert.DeserializeObject<List<Bancos>>(json);
            return bancos; 
        }
    }
}
