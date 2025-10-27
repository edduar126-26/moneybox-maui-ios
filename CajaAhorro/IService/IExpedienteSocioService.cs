using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Money_Box.Models.Local;

namespace Money_Box.IService
{
    public interface IExpedienteSocioService
    {
        Task<Respuesta> GetCalImporteMaximoVentaAcciones(int idSocio, double monto);
        Task<GetCallValidaMovimientos> GetCalValidaMovimientos(int idSocio, double monto, string claveCaja);
        Task<GetCallValidaMovimientos> GetCalValidaImporteAcciones(int idSocio, int tipoEmpleado, string sueldo, string descuento);
        Task<bool> InsertaFoto(EnviarFoto foto);
    }
}
