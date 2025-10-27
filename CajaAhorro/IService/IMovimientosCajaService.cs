using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Money_Box.Models.Local;

namespace Money_Box.IService
{
    public interface IMovimientosCajaService
    {
        Task<string> InsertMovimiento(moMovimientosCaja movimiento);
    }
}
