using Money_Box.Models;
using Money_Box.IService;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Money_Box.Services
{
    public class PromesasService : IPromesasService
    {
        private readonly HttpClient _httpClient;

        public PromesasService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<EntRespuesta> GetConceptos(int idConceptoCaja)
        {
            var url = ApiRest.sBaseUrlWebApi + $"Promesas/GetConceptos?idConceptoCaja={idConceptoCaja}";
            var json = await _httpClient.GetStringAsync(url);

            JObject obj = JObject.Parse(json);
            var status = obj["Estado"]?.Value<bool>() ?? false;

            List<ConceptoPromesa> conceptos = new List<ConceptoPromesa>();

            if (status && obj["Resultado"] is JArray jarr)
            {
                conceptos = jarr.ToObject<List<ConceptoPromesa>>();
            }

            var respuesta = JsonConvert.DeserializeObject<EntRespuesta>(json);
            respuesta.Resultado = conceptos;
            return respuesta;
        }

        public async Task<EntRespuesta> GetPromesasXPrestamo(long idMovimiento)
        {
            var url = ApiRest.sBaseUrlWebApi + $"Promesas/GetPromesasXPrestamo?idPrestamo={idMovimiento}";
            var json = await _httpClient.GetStringAsync(url);

            JObject obj = JObject.Parse(json);
            var status = obj["Estado"]?.Value<bool>() ?? false;

            List<ConceptoPromesa> conceptos = new List<ConceptoPromesa>();

            if (status && obj["Resultado"] is JArray jarr)
            {
                conceptos = jarr.ToObject<List<ConceptoPromesa>>();
            }

            var respuesta = JsonConvert.DeserializeObject<EntRespuesta>(json);
            respuesta.Resultado = conceptos;
            return respuesta;
        }

        public async Task<EntRespuesta> InsertPromesa(int folio, decimal cantidad, int idConcepto, int idSocio, int anio)
        {
            var url = ApiRest.sBaseUrlWebApi + $"Promesas/InsertPromesa?folio={folio}&cantidad={cantidad}&idConcepto={idConcepto}&idSocio={idSocio}&anio={anio}";
            var json = await _httpClient.GetStringAsync(url);
            return JsonConvert.DeserializeObject<EntRespuesta>(json);
        }
    }
}
