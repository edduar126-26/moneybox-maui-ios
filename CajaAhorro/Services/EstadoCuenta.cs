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
    public class EstadoCuentaService : IEstadoCuentaService
    {
        private readonly HttpClient _httpClient;

        public EstadoCuentaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PrestamoVigenteSocio>> GetTotalesPrestamosVigentesSocio(int idSocio)
        {
            var json = await _httpClient.GetStringAsync(ApiRest.sBaseUrlWebApi + $"EstadoCuenta/GetTotalesPrestamosVigentesSocio?IdSocio={idSocio}");
            return JsonConvert.DeserializeObject<List<PrestamoVigenteSocio>>(json);
        }

        public async Task<List<DetallePrestamoVigente>> GetDetallePrestamoVigenteSocio(int idSocio)
        {
            var json = await _httpClient.GetStringAsync(ApiRest.sBaseUrlWebApi + $"EstadoCuenta/GetDetallePrestamoVigenteSocio?IdSocio={idSocio}");
            return JsonConvert.DeserializeObject<List<DetallePrestamoVigente>>(json);
        }
    }
}
