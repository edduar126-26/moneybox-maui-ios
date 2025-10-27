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
using Money_Box.Models.Local;

namespace Money_Box.Services
{
    public class MovimientosCajaService : IMovimientosCajaService
    {
        private readonly HttpClient _httpClient;

        public MovimientosCajaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> InsertMovimiento(moMovimientosCaja m)
        {
            var url = $"{ApiRest.sBaseUrlWebApi}Prestamo/PostmoMovimientoCaja?IdSocio={m.IdSocio}&IdPeriodo={m.IdPeriodo}&IdConceptoCaja={m.IdConceptoCaja}&CantidadMovimiento={m.CantidadMovimiento}&ImporteMovimiento={m.ImporteMovimiento}&PorcentajeMovimiento={m.PorcentajeMovimiento}&PlazoMovimiento={m.PorcentajeMovimiento}&IdPrestamo={m.IdPrestamo}&IdUsuarioAlta={m.IdUsuarioAlta}&IdAval={m.IdAval}";

            return await _httpClient.GetStringAsync(url);
        }
    }
}
