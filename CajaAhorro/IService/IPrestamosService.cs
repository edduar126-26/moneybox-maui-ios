using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Money_Box.Models;

namespace Money_Box.IService
{
    public interface IPrestamosService
    {
        Task<string> GetCalAhorroAcumulado(int idSocio);
        Task<string> GetCalDeudaActual(int idSocio);
        Task<string> GetCalImporteMaximoPrestamoAntiguedad(int idSocio);
        Task<string> GetCalTasaInteresMensual(string claveConcepto);
        Task<string> GetCalImporteMaximoDisponiblePrestamo(int idSocio, string claveConcepto);
        Task<string> GetCalImportePagarReciprocidad(int idSocio, string claveConceptoPrestamo, string importeSolicitado);
        Task<string> GetCalImporteDescuento(int idSocio, string claveConceptoPrestamo, string importeSolicitado);
        Task<string> GetCalImportePrestamoActualSinReciprocidad(int idSocio, string claveConceptoPrestamo);
        Task<List<CaPrestamosEspeciales>> GetCatPrestamosEspeciales();
        Task<int> GetCallIdIdConceptoCaja(string claveConcepto);
        Task<EntRespuesta> GetListaPrestamos(int idEmpresa, int? IdTipoEmpleado);
        Task<EntRespuesta> ValidaMovimiento(int idSocio, int idConcepto);
        Task<EntRespuesta> GetDatosPrestamo(int idPrestamo);
        Task<EntRespuesta> ValidaAval(int idCliente, int noEmpleado);
    }
}
