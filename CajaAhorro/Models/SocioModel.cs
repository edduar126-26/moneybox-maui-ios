using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Money_Box.Models
{
    public class SocioModel
    {
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Nombre { get; set; }
        public string CorreoElectronico { get; set; }
        public string NombreCortoEmpresa { get; set; }
        public Nullable<System.DateTime> FechaIngresoEmpresa { get; set; }
        public Nullable<decimal> SueldoBrutoMensual { get; set; }
        public Nullable<decimal> ImporteDescuentoAhorro { get; set; }
        public Nullable<decimal> ImpDescuentoAhorro { get; set; }
        public Nullable<decimal> ImporteDescuentoCP { get; set; }
        public Nullable<decimal> ImporteDescuentoLP { get; set; }
        public Nullable<decimal> ImpDescuentoPrestamo { get; set; }
        public string TipoEmpleado { get; set; }
        public string NombreBeneficiario { get; set; }
        public Nullable<decimal> PorcentajeOtorgado { get; set; }
        public string CURP { get; set; }
        public string NombreBanco { get; set; }
        public string CLABECuentaBancaria { get; set; }
    }
}


