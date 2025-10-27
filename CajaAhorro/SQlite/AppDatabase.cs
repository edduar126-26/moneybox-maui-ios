
using Money_Box.IService;
using Money_Box.Models;
using Money_Box.Models.Local;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Money_Box.Data
{
    public class AppDatabase : IAppDatabase
    {
        private readonly SQLiteAsyncConnection _db;

        public AppDatabase(string dbPath)
        {
            _db = new SQLiteAsyncConnection(dbPath);

            // Crear tablas necesarias
            _db.CreateTableAsync<Socio>().Wait();
            _db.CreateTableAsync<moMovimientosCaja>().Wait();
            _db.CreateTableAsync<Notifier>().Wait();
        }

        // ----------- Métodos para Notificaciones -----------
        public Task<List<Notifier>> GetNotificacionesPendientesAsync() =>
            _db.Table<Notifier>().Where(n => !n.Resuelta).ToListAsync();

        public Task<Notifier> GetNotificacionPorIdAsync(int idNotificacion) =>
            _db.Table<Notifier>().Where(n => n.IdNotificacion == idNotificacion).FirstOrDefaultAsync();

        public Task<int> SaveNotificacionAsync(Notifier n) =>
            n.Id != 0 ? _db.UpdateAsync(n) : _db.InsertAsync(n);

        public Task<int> MarcarNotificacionResueltaAsync(int id) =>
            _db.ExecuteAsync("UPDATE Notifier SET Resuelta = 1 WHERE IdNotificacion = ?", id);

        public Task<int> DeleteNotificacionAsync(Notifier n) =>
            _db.DeleteAsync(n);

        // ----------- Métodos para Socios -----------

        public Task<List<Socio>> GetSociosAsync() =>
            _db.Table<Socio>().ToListAsync();

        public Task<int> InsertSocioAsync(Socio socio) =>
            _db.InsertAsync(socio);


        public Task<List<Clientes>> GetClientesAsync() =>
            _db.Table<Clientes>().ToListAsync();

        public Task<int> InsertClienteAsync(Clientes cliente) =>
            _db.InsertAsync(cliente);

        // ----------- Métodos para MovimientosCaja -----------

        public Task<int> InsertMovimientoCajaAsync(moMovimientosCaja mov) =>
            _db.InsertAsync(mov);

        public Task<List<moMovimientosCaja>> GetMovimientosCajaAsync() =>
            _db.Table<moMovimientosCaja>().ToListAsync();
    }
}
