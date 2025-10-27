using System;
using System.Collections.Generic;
using System.Text;

namespace Money_Box.Models
{
    public class ActualizarNotificacion
    {
        public int IdUsuario { get; set; }
        public List<moNotificacion> Notificaciones { get; set; }
    }
}

