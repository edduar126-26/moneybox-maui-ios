using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Money_Box.Models;

namespace Money_Box.IService
{
    public interface IEstadoCuentaService
    {
        Task<List<PrestamoVigenteSocio>> GetTotalesPrestamosVigentesSocio(int idSocio);
        Task<List<DetallePrestamoVigente>> GetDetallePrestamoVigenteSocio(int idSocio);
    }
}
