using Microsoft.Maui.Networking;
using Money_Box.IService;
using Money_Box.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Money_Box.Services
{
    public class TablaAmortizacionService : ITablaAmortizacionService
    {
        private readonly HttpClient _httpClient;

        public TablaAmortizacionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

    
        public async Task<EntRespuesta> GetTablaAmortizacionPrestamo(int idSocio, string clave, string importe, List<PromesasPago>? promesas)
        {
            var url = ApiRest.sBaseUrlWebApi + $"Procesos/GetTablaAmortizacionPrestamo?idSocio={idSocio}&clave={clave}&importe=0{importe}";
            var body = new StringContent(JsonConvert.SerializeObject(promesas), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, body);
            var json = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"Respuesta JSON: {json}");
            var obj = JObject.Parse(json);
            var lista = obj["Resultado"]?.ToObject<List<caTablaAmortizacionPrestamo>>() ?? new();

            var respuesta = JsonConvert.DeserializeObject<EntRespuesta>(json);
            respuesta.Resultado = lista;
            return respuesta; 
        }
    }
}
