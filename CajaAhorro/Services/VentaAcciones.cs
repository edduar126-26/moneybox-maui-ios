using Money_Box.IService;
using System.Net.Http;
using System.Threading.Tasks;

namespace Money_Box.Services
{
    public class VentaAccionesService : IVentaAccionesService
    {
        private readonly HttpClient _httpClient;

        public VentaAccionesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetCalAhorroAcumulado(int idSocio)
        {
            var url = ApiRest.sBaseUrlWebApi + $"VentaAcciones/GetCalAhorroAcumulado?IdSocio={idSocio}";
            return await _httpClient.GetStringAsync(url);
        }

        public async Task<string> GetCalDeudaActual(int idSocio)
        {
            var url = ApiRest.sBaseUrlWebApi + $"VentaAcciones/GetCalDeudaActual?IdSocio={idSocio}";
            return await _httpClient.GetStringAsync(url);
        }

        public async Task<string> GetCalImporteVentaAccionesMaximo(int idSocio)
        {
            var url = ApiRest.sBaseUrlWebApi + $"VentaAcciones/GetCalImporteMaximoVentaAcciones?IdSocio={idSocio}";
            return await _httpClient.GetStringAsync(url);
        }
    }
}
