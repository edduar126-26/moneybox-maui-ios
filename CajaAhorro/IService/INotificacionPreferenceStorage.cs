using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Money_Box.IService
{
    public interface INotificacionPreferenceStorage
    {
        List<int> GetResueltas();
        void MarcarComoResuelta(int idNotificacion);
        bool EstaResuelta(int idNotificacion);
        void Limpiar();
    }
}
