using Money_Box.IService;
using Money_Box.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Money_Box.Services
{
    public class EmpresaService : IEmpresaService
    {
        private readonly HttpClient _httpClient;

        public EmpresaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<caEmpresa>> GetEmpresas()
        {
            var url = $"{ApiRest.sBaseUrlWebApi}Empresas/Get";
            var json = await _httpClient.GetStringAsync(url);
            return JsonConvert.DeserializeObject<List<caEmpresa>>(json);
        }

        public async Task<EntRespuesta> GetEmpresas(int idCliente, string clave)
        {
            var url = $"{ApiRest.sBaseUrlWebApi}Acceso/GetEmpresas?IdCliente={idCliente}&clave={Uri.EscapeDataString(clave)}";
            var json = await _httpClient.GetStringAsync(url);

            var obj = JObject.Parse(json);
            var lst = obj["Estado"].Value<bool>() ? obj["Resultado"].ToObject<List<caSocio>>() : new List<caSocio>();

            var respuesta = JsonConvert.DeserializeObject<EntRespuesta>(json);
            respuesta.Resultado = lst;
            return respuesta;
        }

        public async Task<EntRespuesta> GetEmpresasXEmpleado(int noEmpleado)
        {
            var url = $"{ApiRest.sBaseUrlWebApi}Acceso/GetEmpresasEmpleado?noEmpleado={noEmpleado}";
            var json = await _httpClient.GetStringAsync(url);

            var obj = JObject.Parse(json);
            var lst = obj["Estado"].Value<bool>() ? obj["Resultado"].ToObject<List<caSocio>>() : new List<caSocio>();

            var respuesta = JsonConvert.DeserializeObject<EntRespuesta>(json);
            respuesta.Resultado = lst;
            return respuesta;
        }

        public async Task<EntRespuesta> GetEmpresas(int idCliente)
        {
            var url = $"{ApiRest.sBaseUrlWebApi}Clientes/GetEmpresas/{idCliente}";
            var json = await _httpClient.GetStringAsync(url);

            var obj = JObject.Parse(json);
            var lst = obj["Estado"].Value<bool>() ? obj["Resultado"].ToObject<List<caEmpresa>>() : new List<caEmpresa>();

            var respuesta = JsonConvert.DeserializeObject<EntRespuesta>(json);
            respuesta.Resultado = lst;
            return respuesta; 
        }
    }
}
