using System;
using System.Collections.Generic;
using System.Text;

namespace Money_Box.Models
{
    public class vwNotificaciones
    {

       
            public int IdNotificacion { get; set; }
            public int IdDestinatario { get; set; }
            public string Destinatario { get; set; }
            public string Asunto { get; set; }
            public int? FolioSolicitud { get; set; }
            public string Mensaje { get; set; }
            public bool Enviado { get; set; }
            public string EnviadoDesc { get; set; }
        
       
    }
}
