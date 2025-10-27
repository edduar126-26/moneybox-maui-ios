using System;
using System.Collections.Generic;
using System.Text;

namespace Money_Box.Models
{
    public class CaPrestamosEspeciales
    {
        public int IdConceptoCaja { get; set; }
        public string ClaveConceptoCaja { get; set; }
        public string ConceptoCaja { get; set; }
        public int PlazoPrestamo { get; set; }
        public float TasaInteresesPrestamo { get; set; }
    }
    public class PrestamoVigenteSocio
    {
        public long IdSocio { get; set; }
        public long NoEmpleado { get; set; }
        public string Nombre { get; set; }
        public decimal AhorroAcumulado { get; set; }
        public int CantidadPrestamosActivos { get; set; }
        public decimal MontoTotalPrestamo { get; set; }
        public decimal SaldoTotalPendiente { get; set; }
        public decimal TotalDescuentosPrestamo { get; set; }
    }
    public class DetallePrestamoVigente
    {
        public long IdSocio { get; set; }
        public long NoEmpleado { get; set; }
        public string Nombre { get; set; }
        public long IdMovimiento { get; set; }
        public long FolioSolicitud { get; set; }
        public string TipoPrestamo { get; set; }
        public string FechaPrestamo { get; set; }
        public string ImportePrestamo { get; set; }
        public string TasaPrestamo { get; set; }
        public string ImporteDescuento { get; set; }
        public string SaldoPrestamo { get; set; }
    }
}