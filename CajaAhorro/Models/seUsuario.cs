using System;
using System.Collections.Generic;
using System.Text;

namespace Money_Box.Models
{
    public class seUsuario
    {
        public int IdUsuario { get; set; }
        public Nullable<int> ClaveUsuario { get; set; }
        public string Contrasenia { get; set; }
        public Nullable<System.DateTime> FechaRecuperacionContrasenia { get; set; }
        public string IdSesion { get; set; }
        public Nullable<System.DateTime> FechaUltimaConexion { get; set; }
        public bool Estatus { get; set; }
        public Nullable<int> idRol { get; set; }
        public virtual ICollection<caSocio> caSocios { get; set; }
        //public virtual seRol seRol { get; set; }
    }
  
}
