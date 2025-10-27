using Money_Box.Models;
using Money_Box.IService;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Money_Box.Services
{
    public class ModificacionAhorroService : IModificacionAhorroService
    {
        private readonly HttpClient _httpClient;

        public ModificacionAhorroService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<SocioModel>> GetDatosSocio(int idSocio)
        {
            var url = ApiRest.sBaseUrlWebApi + $"ModificacionAhorro/GetDatosSocio?IdSocio={idSocio}";
            var json = await _httpClient.GetStringAsync(url);
            return JsonConvert.DeserializeObject<List<SocioModel>>(json);
        }

        public async Task<EntRespuesta> GetDatosAhorroCP(int idSocio)
        {
            var url = ApiRest.sBaseUrlWebApi + $"Procesos/GetAhorroCP?idSocio={idSocio}";
            var json = await _httpClient.GetStringAsync(url);

            var objeto = JsonConvert.DeserializeObject(json);
            string jsonString = objeto.ToString();

            JObject obj = JObject.Parse(jsonString);
            var status = obj["Estado"]?.Value<bool>() ?? false;
            caSocio socio = new caSocio();

            if (status && obj["Resultado"] != null)
            {
                var jarr = obj["Resultado"].Value<JObject>();
                socio = jarr.ToObject<caSocio>();
            }

            var respuesta = JsonConvert.DeserializeObject<EntRespuesta>(json);
            respuesta.Resultado = socio;
            return respuesta;
        }
    }
}
