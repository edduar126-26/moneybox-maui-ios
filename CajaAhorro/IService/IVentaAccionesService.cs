using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Money_Box.IService
{
    public interface IVentaAccionesService
    {
        Task<string> GetCalAhorroAcumulado(int idSocio);
        Task<string> GetCalDeudaActual(int idSocio);
        Task<string> GetCalImporteVentaAccionesMaximo(int idSocio);
    }
}
