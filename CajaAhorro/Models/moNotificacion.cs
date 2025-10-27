using System;
using System.Collections.Generic;
using System.Text;

namespace Money_Box.Models
{
    public class moNotificacion
    {
        public int IdNotificacion { get; set; }
        public int IdDestinatario { get; set; }
        public string Asunto { get; set; }
        public string Mensaje { get; set; }
        public bool Enviado { get; set; }
    }
}
