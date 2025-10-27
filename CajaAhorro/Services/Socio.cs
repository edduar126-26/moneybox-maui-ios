using Money_Box.IService;
using Money_Box.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Money_Box.Services
{
    public class SocioService : ISocioService
    {
        private readonly HttpClient _httpClient;

        public SocioService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<int> ActFechaTerminosCondiciones(int noEmpleado)
        {
            var url = ApiRest.sBaseUrlWebApi + $"Socio/ActFechaTerminosCondiciones?NoEmpleado={noEmpleado}";
            var json = await _httpClient.GetStringAsync(url);
            return int.Parse(json);
        }

        public async Task<int> GetExistemail(string email)
        {
            var url = ApiRest.sBaseUrlWebApi + $"Socio/GetExistemail?email={email}";
            var json = await _httpClient.GetStringAsync(url);
            return int.Parse(json);
        }

        public async Task<int> ExisteRfc(string RFC)
        {
            var url = ApiRest.sBaseUrlWebApi + $"Socio/RfcExiste?RFC={RFC}";
            var json = await _httpClient.GetStringAsync(url);
            return int.Parse(json);
        }

        public async Task<int> ObtenerIdEmpresaDesdeAPI(int numeroEmpleado)
        {
            try
            {
                var url = ApiRest.sBaseUrlWebApi + "Socio/ValidaNumeroEmpleado";

                var body = new { NumeroEmpleado = numeroEmpleado };
                var jsonBody = JsonConvert.SerializeObject(body);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResult = await response.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<ValidaNumeroEmpleadoResponse>(jsonResult);
                    return resultado.estatus ? resultado.idEmpresa : -1;
                }
            }
            catch
            {
                // log error si se requiere
            }

            return -1;
        }
    }
}
