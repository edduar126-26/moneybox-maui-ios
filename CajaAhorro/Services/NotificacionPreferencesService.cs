using Microsoft.Maui.Storage;
using Money_Box.IService;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Money_Box.Services
{
    public class NotificacionPreferenceStorage : INotificacionPreferenceStorage
    {
        private const string KeyResueltas = "notificaciones_resueltas";

        public List<int> GetResueltas()
        {
            var json = Preferences.Get(KeyResueltas, "[]");
            return JsonConvert.DeserializeObject<List<int>>(json);
        }

        public void MarcarComoResuelta(int idNotificacion)
        {
            var resueltas = GetResueltas();
            if (!resueltas.Contains(idNotificacion))
            {
                resueltas.Add(idNotificacion);
                Preferences.Set(KeyResueltas, JsonConvert.SerializeObject(resueltas));
            }
        }

        public bool EstaResuelta(int idNotificacion) =>
            GetResueltas().Contains(idNotificacion);

        public void Limpiar() =>
            Preferences.Remove(KeyResueltas);
    }
}
