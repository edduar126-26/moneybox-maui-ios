using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Money_Box.Models;

namespace Money_Box.IService
{
    public interface ITablaAmortizacionService
    {
        Task<EntRespuesta> GetTablaAmortizacionPrestamo(int idSocio, string clave, string importe, List<PromesasPago> promesas);
    }
}
