using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Money_Box.Models.Local
{
    //Modelo MovimientoCaja 
    public class MovimientoCaja
    {
        [PrimaryKey, AutoIncrement]
        public int IdMovimiento { get; set; }
        public int IdSocio { get; set; }
        public int IdPeriodo { get; set; }
        public int IdConceptoCaja { get; set; }
        public decimal ImporteMovimiento { get; set; }
        public decimal CantidadMovimiento { get; set; }
        public decimal PorcentajeMovimiento { get; set; }
        public int PlazoMovimiento { get; set; }
        public DateTime FechaAlta { get; set; }
        public int IdUsuarioAlta { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int IdUsuarioModifica { get; set; }
        public string PreliminarDefinitivo { get; set; }
        public string EstatusAutorizacion { get; set; }
        public bool Estatus { get; set; }
    }
}
