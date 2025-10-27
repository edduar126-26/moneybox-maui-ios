using Money_Box.Models;
using Money_Box.Models.Local;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Money_Box.IService
{
    public interface IUserService
    {
        Task<EntRespuesta> InsertaSocio(caSocio socio);
        Task UpdCambiaContrasenia(int IdSocio, string Contrasenia);
        Task<string> BajaCajaSocio(moMovimientosCaja MovimientoCaja);
        Task<string> GetDescuentoMaximoAhorro(int IdSocio);
        Task<string> GetCalImporteBajaCajaAhorro(int IdSocio);
        Task<string> GetPeriodoVigente(int IdSocio);
        Task<DocumentosSocio> GetRutaArchivosSocioAsync(int folioSolicitud);
        Task<bool> SubirArchivosAsync(int idSocio, List<DocumentoSeleccionado> archivos);
        Task<bool> SubirArchivoUnicoAsync(int idSocio, DocumentoSeleccionado archivo);
        Task<bool> EnviarFirmaAsync(string firmaBase64, int folioSolicitud);
        Task<bool> EnviarFirmaAsyncN(string firmaBase64, int IdUsuario);
        Task<bool> YaPagareFirmadoAsync(int folioSolicitud);
        Task<bool> YaContratoCFirmado(int IdUsuario);
        Task<List<Notifier>> ObtenerNotificacionesPendientes(int idSocio);
        Task<string> ActualizarNotificacionesAsync(int idUsuario, List<moNotificacion> notificaciones);
        Task<List<vwNotificaciones>> BuscarNotificaciones(int IdSocio);
    }
}
