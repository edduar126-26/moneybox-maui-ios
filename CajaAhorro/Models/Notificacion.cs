using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Money_Box.Models
{
    public class Notificacion
    {
        public int IdNotificacion { get; set; }
        public string Mensaje { get; set; }
        public string FolioSolicitud {  get; set; }
        public DateTime Fecha { get; set; }
    }
}
