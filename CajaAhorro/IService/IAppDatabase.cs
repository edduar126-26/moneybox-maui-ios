using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Money_Box.Models;
using Money_Box.Models.Local;


namespace Money_Box.IService
{
    public interface IAppDatabase
    {
        // Notificaciones
        Task<List<Notifier>> GetNotificacionesPendientesAsync();
        Task<Notifier> GetNotificacionPorIdAsync(int idNotificacion);
        Task<int> SaveNotificacionAsync(Notifier n);
        Task<int> MarcarNotificacionResueltaAsync(int id);
        Task<int> DeleteNotificacionAsync(Notifier n);

        // Socios
        Task<List<Socio>> GetSociosAsync();
        Task<int> InsertSocioAsync(Socio socio);

        // Movimientos de Caja
        Task<int> InsertMovimientoCajaAsync(moMovimientosCaja mov);
        Task<List<moMovimientosCaja>> GetMovimientosCajaAsync();

        // Clientes
        Task<List<Clientes>> GetClientesAsync();
        Task<int> InsertClienteAsync(Clientes cliente);
    }
}
