using System;
using System.Collections.Generic;
using System.Text;

namespace Money_Box.Models
{
    
        public class caTablaAmortizacionPrestamo
        {
            public int NoRenglon { get; set; }
            public string FechaPago { get; set; }
            public decimal? SaldoInsoluto { get; set; } = 0m;
            public decimal PagoMensual { get; set; }
            public decimal Capital { get; set; }
            public decimal AmortizacionExtraordinaria { get; set; }
            public decimal Intereses { get; set; }
            public decimal IVA { get; set; }
        }
    
}
