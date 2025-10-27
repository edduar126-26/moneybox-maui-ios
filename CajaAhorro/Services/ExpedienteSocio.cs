using Money_Box.IService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Money_Box.Models.Local;

namespace Money_Box.Services
{
    public class ExpedienteSocioService : IExpedienteSocioService
    {
        private readonly HttpClient _httpClient;
        public ExpedienteSocioService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Respuesta> GetCalImporteMaximoVentaAcciones(int idSocio, double monto)
        {
            var url = ApiRest.sBaseUrlWebApi + $"ExpedienteSocio/GetCalImporteMaximoVentaAcciones?IdSocio={idSocio}&ImporteSolicitado={monto}";
            var json = await _httpClient.GetStringAsync(url);
            return JsonConvert.DeserializeObject<Respuesta>(json);
        }

        public async Task<GetCallValidaMovimientos> GetCalValidaMovimientos(int idSocio, double monto, string claveCaja)
        {
            var url = ApiRest.sBaseUrlWebApi + $"ExpedienteSocio/GetCalValidaMovimientos?IdSocio={idSocio}&ClaveConceptoCaja={claveCaja}&Importe={monto}";
            var json = await _httpClient.GetStringAsync(url);
            return JsonConvert.DeserializeObject<GetCallValidaMovimientos>(json);
        }

        public async Task<GetCallValidaMovimientos> GetCalValidaImporteAcciones(int idSocio, int tipoEmpleado, string sueldo, string descuento)
        {
            var url = ApiRest.sBaseUrlWebApi + $"ExpedienteSocio/GetCalValidaImporteAcciones?idSocio={idSocio}&idTipoEmpleado={tipoEmpleado}&sueldoBrutoMensual={sueldo}&descuentoAhorro={descuento}";
            var json = await _httpClient.GetStringAsync(url);
            return JsonConvert.DeserializeObject<GetCallValidaMovimientos>(json);
        }

        public async Task<bool> InsertaFoto(EnviarFoto foto)
        {
            var url = ApiRest.sBaseUrlWebApi + "ExpedienteSocio/InsExpedienteSocio";
            var body = new StringContent(JsonConvert.SerializeObject(foto), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, body);
            return response.IsSuccessStatusCode;
        }
    }
}
