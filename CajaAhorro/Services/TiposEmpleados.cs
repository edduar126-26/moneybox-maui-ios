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
    public class TiposEmpleadosService : ITiposEmpleadosService
    {
        private readonly HttpClient _httpClient;

        public TiposEmpleadosService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<caTiposEmpleado>> GetTiposEmpleado()
        {
            var json = await _httpClient.GetStringAsync(ApiRest.sBaseUrlWebApi + "TipoEmpleado/Get");
            return JsonConvert.DeserializeObject<List<caTiposEmpleado>>(json);
        }
    }
}

