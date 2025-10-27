using System;
using System.Collections.Generic;
using System.Text;

namespace Money_Box.Models
{
    public  class Promesa
    {
        public int? FolioSolicitud { get; set; }
        public int IdMoPromesaPago { get; set; }
        public int IdcfPromesaPago { get; set; }
        public int IdmoMovimientoCaja { get; set; }
        public int IdmoPrestamos { get; set; }
        public int IdmoSaldos { get; set; }
        public bool Estatus { get; set; }
        public string Descripcion { get; set; }
        public double Cantidad { get; set; }
        public Nullable<System.DateTime> FechaAlta { get; set; }
        public Nullable<int> IdUsuarioAlta { get; set; }
        public Nullable<int> Anio { get; set; }
        public Nullable<System.DateTime> FechaModifica { get; set; }
        public Nullable<int> IdUsuarioModifica { get; set; }
    }
}
