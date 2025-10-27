
using Money_Box.IService;
using Money_Box.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Money_Box.Services
{
    public class PrestamosService : IPrestamosService
    {
        private readonly HttpClient _httpClient;

        public PrestamosService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetCalAhorroAcumulado(int idSocio)
        {
            var url = ApiRest.sBaseUrlWebApi + $"Prestamo/GetCalAhorroAcumulado?IdSocio={idSocio}";
            var json = await _httpClient.GetStringAsync(url);
            return string.IsNullOrEmpty(json) ? "0.00" : json;
        }

        public async Task<string> GetCalDeudaActual(int idSocio)
        {
            var url = ApiRest.sBaseUrlWebApi + $"Prestamo/GetCalDeudaActual?IdSocio={idSocio}";
            var json = await _httpClient.GetStringAsync(url);
            return string.IsNullOrEmpty(json) ? "0.00" : json;
        }

        public async Task<EntRespuesta> GetListaPrestamos(int idEmpresa, int? IdTipoEmpleado)
        {
            var url = ApiRest.sBaseUrlWebApi + $"empresas/GetConceptosPrestamo?Idempresa={idEmpresa}" + "&IdTipoEmpleado=" + IdTipoEmpleado;
            var json = await _httpClient.GetStringAsync(url);
            var obj = JObject.Parse(json);
            var prestamos = obj["Resultado"]?.ToObject<List<Prestamo>>() ?? new();
            var respuesta = JsonConvert.DeserializeObject<EntRespuesta>(json);
            respuesta.Resultado = prestamos;
            return respuesta;
        }

        public async Task<EntRespuesta> ValidaAval(int idCliente, int noEmpleado)
        {
            var url = ApiRest.sBaseUrlWebApi + $"Procesos/GetValidaEmpleado?idEmpresa={idCliente}&noEmpleado={noEmpleado}";
            var json = await _httpClient.GetStringAsync(url);
            var obj = JObject.Parse(json);
            var lst = obj["Estado"]?.Value<bool>() == true
                ? obj["Resultado"]?.ToObject<List<caSocio>>() ?? new()
                : new List<caSocio>();

            var respuesta = JsonConvert.DeserializeObject<EntRespuesta>(json);
            respuesta.Resultado = lst;
            return respuesta;
        }

      
        public async Task<string> GetCalImporteMaximoPrestamoAntiguedad(int IdSocio)
        {
            string ImporteMaximoPrestamoAntiguedad;
            var url = ApiRest.sBaseUrlWebApi + "Prestamo/GetCalImporteMaximoPrestamoAntiguedad?IdSocio=" + IdSocio;
            var json = await _httpClient.GetStringAsync(url);
            ImporteMaximoPrestamoAntiguedad = string.IsNullOrEmpty(json) ? "0.00" : json;
            return ImporteMaximoPrestamoAntiguedad;
        }

        public async Task<string> GetCalTasaInteresMensual(string ClaveConcepto)
        {
            string TasaInteresMensual;
            var url = ApiRest.sBaseUrlWebApi + "Prestamo/GetCalTasaInteresMensual?ClaveConceptoCajaAhorro=" + ClaveConcepto;
            var json = await _httpClient.GetStringAsync(url);
            TasaInteresMensual = string.IsNullOrEmpty(json) ? "0.00" : json;
            return TasaInteresMensual;
        }

        public async Task<string> GetCalImporteMaximoDisponiblePrestamo(int IdSocio, string ClaveConcepto)
        {
            string ImporteMaximoDisponiblePrestamo;
            var url = ApiRest.sBaseUrlWebApi + "Prestamo/GetCalImporteMaximoDisponiblePrestamo?IdSocio=" + IdSocio + "&ClaveConceptoCajaAhorro=" + ClaveConcepto;
            var json = await _httpClient.GetStringAsync(url);
            ImporteMaximoDisponiblePrestamo = string.IsNullOrEmpty(json) ? "0.00" : json;
            return ImporteMaximoDisponiblePrestamo;
        }

        public async Task<string> GetCalImportePagarReciprocidad(int IdSocio, string ClaveConceptoPrestamo, string ImporteSolicitado)
        {
            string ImportePagarReciprocidad;
            var url = ApiRest.sBaseUrlWebApi + "Prestamo/GetCalImportePagarReciprocidad?IdSocio=" + IdSocio + "&ClaveConceptoPrestamo=" + ClaveConceptoPrestamo + "&ImportePrestamoSolicitado=" + 0 + ImporteSolicitado;
            var json = await _httpClient.GetStringAsync(url);
            ImportePagarReciprocidad = string.IsNullOrEmpty(json) ? "0.00" : json;
            return ImportePagarReciprocidad;
        }

        public async Task<string> GetCalImporteDescuento(int IdSocio, string ClaveConceptoPrestamo, string ImporteSolicitado)
        {
            string ImporteDescuento;
            var url = ApiRest.sBaseUrlWebApi + "Prestamo/GetCalImporteDescuento?IdSocio=" + IdSocio + "&ClaveConceptoPrestamo=" + ClaveConceptoPrestamo + "&ImportePrestamoSolicitado=" + 0 + ImporteSolicitado;
            var json = await _httpClient.GetStringAsync(url);
            ImporteDescuento = string.IsNullOrEmpty(json) ? "0.00" : json;
            return ImporteDescuento;
        }

        public async Task<string> GetCalImportePrestamoActualSinReciprocidad(int IdSocio, string ClaveConceptoPrestamo)
        {
            string ImportePrestamoActualSinReciprocidad;
            var url = ApiRest.sBaseUrlWebApi + "Prestamo/GetCalImportePrestamoActualSinReciprocidad?IdSocio=" + IdSocio + "&ClaveConceptoPrestamo=" + ClaveConceptoPrestamo;
            var json = await _httpClient.GetStringAsync(url);
            ImportePrestamoActualSinReciprocidad = string.IsNullOrEmpty(json) ? "0.00" : json;
            return ImportePrestamoActualSinReciprocidad;
        }

        public async Task<List<CaPrestamosEspeciales>> GetCatPrestamosEspeciales()
        {
            var url = ApiRest.sBaseUrlWebApi + "Prestamo/GetCatPrestamosEspeciales";
            var json = await _httpClient.GetStringAsync(url);

            return string.IsNullOrEmpty(json)
                ? new List<CaPrestamosEspeciales>()
                : JsonConvert.DeserializeObject<List<CaPrestamosEspeciales>>(json);
        }

        public async Task<int> GetCallIdIdConceptoCaja(string claveConcepto)
        {
            var url = ApiRest.sBaseUrlWebApi + $"Prestamo/GetCalImporteDescuento?ClaveConcepto={claveConcepto}";
            var json = await _httpClient.GetStringAsync(url);
            return short.TryParse(json, out short idConceptoCaja) ? idConceptoCaja : 0;
        }


        public async Task<EntRespuesta> ValidaMovimiento(int idSocio, int idConcepto)
        {
            var url = ApiRest.sBaseUrlWebApi + $"Procesos/GetValidaMovimientoDuplicado?idSocio={idSocio}&idConcepto={idConcepto}";
            var json = await _httpClient.GetStringAsync(url);
            return JsonConvert.DeserializeObject<EntRespuesta>(json);
        }

        public async Task<EntRespuesta> GetDatosPrestamo(int idPrestamo)
        {
            var url = ApiRest.sBaseUrlWebApi + $"Procesos/GetConceptoPrestamo?idConcepto={idPrestamo}";
            var json = await _httpClient.GetStringAsync(url);

            var obj = JObject.Parse(json);
            var resultadoObj = obj["Resultado"]?.ToObject<Prestamo>() ?? new Prestamo();

            var respuesta = JsonConvert.DeserializeObject<EntRespuesta>(json);
            respuesta.Resultado = resultadoObj;

            return respuesta;
        }
    }
}
