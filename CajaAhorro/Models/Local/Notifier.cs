using SQLite;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Money_Box.Models.Local
{
    public class Notifier
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int IdNotificacion { get; set; } 

        public int IdDestinatario { get; set; }

        public string Asunto { get; set; }

        public string Mensaje { get; set; }

        public bool Resuelta { get; set; }

        public int? FolioSolicitud { get; set; }

        public bool TieneFolio => FolioSolicitud > 0;
    }
}


