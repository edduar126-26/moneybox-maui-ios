using System;
using System.Collections.Generic;
using System.Text;

namespace Money_Box.Models
{
    public class Prestamo
    {
        public bool Automatico { get; set; }
        public string Clave { get; set; }
        public string Concepto { get; set; }
        public bool EsPrestamoEspecial { get; set; }
        public int IdConceptoCaja { get; set; }
        public bool Estatus { get; set; }
        public string IdCliente { get; set; }
        public string IdEmpresa { get; set; }
        public bool InterfazBanco { get; set; }
        public bool InterfazNomina { get; set; }
        public bool RequiereAval { get; set; }
        public string TextoBanco { get; set; }
        public string TextoNomina { get; set; }
        public string Tipo { get; set; }
        public string Reciprocidad { get; set; }
        public string Descuento { get; set; }


        public bool RequierePromesa { get; set; }

        public Nullable<int> IdConceptoDescuento { get; set; }
        public Nullable<int> IdConceptoInteres { get; set; }
        public Nullable<int> IdSocio { get; set; }
        public Nullable<int> PlazoPrestamo { get; set; }
        public string ClaveInteres { get; set; }
        public Nullable<double> MaximoPrestamo { get; set; }
        public Nullable<double> TasaInteresPrestamo { get; set; }
        public Nullable<double> Monto { get; set; }
    }
}
