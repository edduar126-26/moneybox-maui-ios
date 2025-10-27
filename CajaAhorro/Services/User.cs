using Microsoft.Maui.Networking;
using Money_Box.IService;
using Money_Box.Models;
using Money_Box.Models.Local;
using Money_Box.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Money_Box.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly IAppDatabase _notificacionDB;

        public UserService(HttpClient httpClient,IAppDatabase notificacionDB )
        {
            _httpClient = httpClient;
            _notificacionDB = notificacionDB;
        }

        public async Task<EntRespuesta> InsertaSocio(caSocio socio)
        {
            var socioWeb = MapearASocio(socio);


            var altaSocio = new AltaSocioDto
            {
                Socio = socioWeb,
                Beneficiarios = new List<caBeneficiarios>
                {
                    new caBeneficiarios
                    {
                        Nombre = socio.NombreBeneficiario1 ?? "",
                        CURP = socio.CURPBeneficiario1 ?? "",
                        PorcentajeOtorgado = socio.PorcentajeOtorgadoBeneficiario1 ?? 0
                    },
                    new caBeneficiarios
                    {
                        Nombre = socio.NombreBeneficiario2 ?? "",
                        CURP = socio.CURPBeneficiario2 ?? "",
                        PorcentajeOtorgado = socio.PorcentajeOtorgadoBeneficiario2 ?? 0
                    }
                }
            };

            var URLWebAPI = ApiRest.sBaseUrlWebApi + "Acceso/InsertaAltaCajaAhorro";

            using (var Client = new HttpClient())
            {
                var content = JsonConvert.SerializeObject(altaSocio);
                var body = new StringContent(content, Encoding.UTF8, "application/json");

                var json = await Client.PostAsync(URLWebAPI, body);
                var responseText = await json.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<EntRespuesta>(responseText);
            }
        }

        private ModelCatalogoDatosSocio MapearASocio(caSocio socio)
        {
            return new ModelCatalogoDatosSocio
            {
                IdEmpresa = socio.IdEmpresa,
                Estatus = socio.Estatus,
                ApellidoPaterno = socio.ApellidoPaterno,
                ApellidoMaterno = socio.ApellidoMaterno,
                RFC = socio.RFC,
                Nombre = socio.Nombre,
                CorreoElectronico = socio.CorreoElectronico,
                FechaIngresoEmpresa = socio.FechaIngresoEmpresa ?? DateTime.Now,
                SueldoBrutoMensual = socio.SueldoBrutoMensual ?? 0,
                IdTipoEmpleado = socio.IdTipoEmpleado ?? 0,
                ImporteDescuentoAhorroLP = socio.ImporteDescuentoLP ?? 0,
                ImporteDescuentoAhorroCP = socio.ImporteDescuentoCP ?? 0,
                IdBanco = socio.IdBanco ?? 0,
                CLABECuentaBancaria = socio.CLABECuentaBancaria ?? "",
                Password = socio.Contrasenia
            };
        }
        public async Task UpdCambiaContrasenia(int IdSocio, string Contrasenia)
        {
            var url = $"{ApiRest.sBaseUrlWebApi}Socio/UpdCambiaContrasenia?IdSocio={IdSocio}&Contrasenia={Contrasenia}";
            await _httpClient.GetAsync(url);
        }

        public async Task<string> BajaCajaSocio(moMovimientosCaja MovimientoCaja)
        {
            var url = $"{ApiRest.sBaseUrlWebApi}moMovimientosCajas/PostmoMovimientosCaja";
            var body = JsonConvert.SerializeObject(MovimientoCaja);
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);
            return response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : null;
        }

        public async Task<string> GetDescuentoMaximoAhorro(int IdSocio)
        {
            var url = $"{ApiRest.sBaseUrlWebApi}Socio/GetDescuentoMaximoAhorro?Idsocio={IdSocio}";
            return await _httpClient.GetStringAsync(url);
        }

        public async Task<string> GetCalImporteBajaCajaAhorro(int IdSocio)
        {
            var url = $"{ApiRest.sBaseUrlWebApi}Socio/GetCalImportePagarBaja?IdSocio={IdSocio}";
            return await _httpClient.GetStringAsync(url);
        }

        public async Task<string> GetPeriodoVigente(int IdSocio)
        {
            var url = $"{ApiRest.sBaseUrlWebApi}Socio/GetPeriodoVigente?IdSocio={IdSocio}";
            return await _httpClient.GetStringAsync(url);
        }

        public async Task<DocumentosSocio> GetRutaArchivosSocioAsync(int folioSolicitud)
        {
            var url = $"{ApiRest.sBaseUrlWebApi}Socio/ObtenerDocumentosPagare?folioSolicitud={folioSolicitud}";
            var response = await _httpClient.GetAsync(url);
            return response.IsSuccessStatusCode
                ? JsonConvert.DeserializeObject<DocumentosSocio>(await response.Content.ReadAsStringAsync())
                : null;
        }

        public async Task<bool> SubirArchivosAsync(int idSocio, List<DocumentoSeleccionado> archivos)
        {
            var url = ApiRest.sBaseUrlWebApi + $"Acceso/SubirArchivos?idSocio={idSocio}";

            using (var Client = new HttpClient())
            {
                using (var form = new MultipartFormDataContent())
                {
                    foreach (var archivo in archivos)
                    {
                        if (archivo == null) continue;

                        var stream = await archivo.AbrirStreamAsync();
                        var contenido = new StreamContent(stream);

                        var extension = Path.GetExtension(archivo.NombreArchivo)?.ToLowerInvariant();
                        var mimeType = ObtenerMimeType(extension);

                        contenido.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                        {
                            Name = "\"archivos\"",
                            FileName = $"\"{archivo.NombreArchivo}\""
                        };
                        contenido.Headers.ContentType = new MediaTypeHeaderValue(mimeType);

                        form.Add(contenido);
                    }

                    var response = await Client.PostAsync(url, form);
                    return response.IsSuccessStatusCode;
                }
            }
        }


        public async Task<bool> SubirArchivoUnicoAsync(int idSocio, DocumentoSeleccionado archivo)
        {
                var tipo = archivo.Tipo.ToUpper();

                var url = ApiRest.sBaseUrlWebApi + $"Acceso/SubirArchivoIndividual?idSocio={idSocio}&tipo={tipo}";

                using (var client = new HttpClient())
                {
                using (var form = new MultipartFormDataContent())
                   {
                       var stream = await archivo.AbrirStreamAsync();
                        var contenido = new StreamContent(stream);

                       var extension = Path.GetExtension(archivo.NombreArchivo)?.ToLowerInvariant();
                       var mimeType = ObtenerMimeType(extension);

                       contenido.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                       {
                            Name = "\"archivoNuevo\"",
                           FileName = $"\"{archivo.NombreArchivo}\""
                        };
                        contenido.Headers.ContentType = new MediaTypeHeaderValue(mimeType);

                       form.Add(contenido, "archivoNuevo");

                        var response = await client.PostAsync(url, form);
                        return response.IsSuccessStatusCode; 
                    }
               }
        }

        private string ObtenerMimeType(string extension)
        {
            switch (extension)
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".pdf":
                    return "application/pdf";
                default:
                    return "application/octet-stream";
            }
        }

        public async Task<bool> EnviarFirmaAsync(string firmaBase64, int folioSolicitud)
        {
            try
            {
                var url = ApiRest.sBaseUrlWebApi + $"Socio/FirmarPagare?folioSolicitud={folioSolicitud}";
                using (var client = new HttpClient())
                {
                    var firmaModel = new FirmaModel
                    {
                        FirmaBase64 = firmaBase64,
                        FechaFirma = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    };


                    var jsonContent = JsonConvert.SerializeObject(firmaModel);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");


                    var response = await client.PostAsync(url, content);


                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error al enviar la firma: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> EnviarFirmaAsyncN(string firmaBase64, int IdUsuario)
        {
            try
            {
                var url = ApiRest.sBaseUrlWebApi + $"Socio/FirmarContratoNormativo?IdUsuario={IdUsuario}";
                using (var client = new HttpClient())
                {
                    var firmaModel = new FirmaModel
                    {
                        FirmaBase64 = firmaBase64,
                        FechaFirma = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    };


                    var jsonContent = JsonConvert.SerializeObject(firmaModel);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");


                    var response = await client.PostAsync(url, content);


                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error al enviar la firma: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> YaPagareFirmadoAsync(int folioSolicitud)
        {
            try
            {
                var url = ApiRest.sBaseUrlWebApi + $"Socio/YaPagareFirmado?folioSolicitud={folioSolicitud}";

                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                        return false;

                    var json = await response.Content.ReadAsStringAsync();
                    var firmado = JObject.Parse(json)["Firmado"].Value<bool>();
                    return firmado;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en YaPagareFirmadoAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> YaContratoCFirmado(int IdUsuario)
        {
            try
            {
                var url = ApiRest.sBaseUrlWebApi + "Socio/YaContratoNFirmado?IdUsuario=" + IdUsuario;

                using (var Client = new HttpClient())
                {
                    var response = await Client.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                        return false;

                    var json = await response.Content.ReadAsStringAsync();
                    var firmado = JObject.Parse(json)["Firmado"].Value<bool>();
                    return firmado;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en YaContratoCFirmado: {ex.Message}");
                return false;
            }
        }

        public async Task<List<vwNotificaciones>> BuscarNotificaciones(int IdSocio)
        {
            try
            {
                var URLWebAPI = ApiRest.sBaseUrlWebApi + "Socio/ObtenerNotificacionesPorIdSocio?Id=" + IdSocio;
                var client = new HttpClient();
                var response = await client.GetAsync(URLWebAPI);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<DtoRespuesta<List<vwNotificaciones>>>(json);

                    if (resultado?.Estado == true && resultado.Resultado?.Any() == true)
                    {
                        return resultado.Resultado;
                    }
                }
            }
            catch (Exception ex)
            {
                //Aqui puedo debugear
            }

            return null;
        }

        public async Task<List<Notifier>> ObtenerNotificacionesPendientes(int idSocio)
        {
            var notisDesdeApi = await BuscarNotificaciones(idSocio);

            List<moNotificacion> notificacionesParaActualizar = new List<moNotificacion>();

            if (notisDesdeApi == null)
                return await _notificacionDB.GetNotificacionesPendientesAsync();

            foreach (var item in notisDesdeApi)
            {
                var yaExiste = await _notificacionDB.GetNotificacionPorIdAsync(item.IdNotificacion);

                if (yaExiste == null)
                {
                    var noti = new Notifier
                    {
                        IdNotificacion = item.IdNotificacion,
                        Asunto = item.Asunto,
                        Mensaje = item.Mensaje,
                        Resuelta = false,
                        FolioSolicitud = item.FolioSolicitud,
                    };

                    await _notificacionDB.SaveNotificacionAsync(noti);


                    notificacionesParaActualizar.Add(new moNotificacion
                    {
                        IdNotificacion = item.IdNotificacion,
                        IdDestinatario = item.IdDestinatario,
                        Asunto = item.Asunto,
                        Mensaje = item.Mensaje,
                        Enviado = true
                    });
                }

            }



            return await _notificacionDB.GetNotificacionesPendientesAsync();

        }

        public async Task<string> ActualizarNotificacionesAsync(int idUsuario, List<moNotificacion> notificaciones)
        {
            var URLWebAPI = ApiRest.sBaseUrlWebApi + "Socio/ActualizarNotificaciones";

            var requestBody = new ActualizarNotificacion
            {
                IdUsuario = idUsuario,
                Notificaciones = notificaciones
            };

            var json = JsonConvert.SerializeObject(requestBody);

            using (var client = new HttpClient())
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(URLWebAPI, content);

                if (response.IsSuccessStatusCode)
                {
                    var respuestaJson = await response.Content.ReadAsStringAsync();
                    return respuestaJson;
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();

                    return null;
                }
            }
        }
    }

}



