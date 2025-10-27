using System;
using System.Collections.Generic;
using System.Text;

namespace Money_Box.Models
{
    public class ConceptoPromesa
    {
        public int IdConceptoPromesa { get; set; }
        public int Anio{ get; set; }
        public int Mes{ get; set; }
        public string Descripcion { get; set; }
        public decimal Cantidad { get; set; }
        public decimal? SaldoPromesa { get; set; }
        public decimal? Importe { get; set; }
    }

    public class moPromesaPago
    {
        public int IdMoPromesaPago { get; set; }
        public int IdcfPromesaPago { get; set; }
        public string DescPromesaPago { get; set; }
        public int Anio { get; set; }
        public int IdmoMovimientoCaja { get; set; }
        public decimal Cantidad { get; set; }
    }
}
