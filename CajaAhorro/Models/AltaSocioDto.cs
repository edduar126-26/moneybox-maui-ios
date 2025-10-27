using Money_Box.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Money_Box.Models
{

    public  class caBeneficiarios
    {
      
        public string Nombre { get; set; }
        public Nullable<decimal> PorcentajeOtorgado { get; set; }
        public string CURP { get; set; }
     
    }

    public class ModelCatalogoDatosSocio
    {
    
        public int IdEmpresa { get; set; }
        public Nullable<bool> Estatus { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string RFC { get; set; }
        public string Nombre { get; set; }
        public string Calle { get; set; }
        public string NumeroExterior { get; set; }
        public string NumeroInterior { get; set; }
        public string Colonia { get; set; }
        public string CodigoPostal { get; set; }
        public int IdEstado { get; set; }
        public int IdPoblacion { get; set; }
        public string Password { get; set; }
        public string CorreoElectronico { get; set; }
        public DateTime FechaIngresoEmpresa { get; set; }
        public decimal SueldoBrutoMensual { get; set; }
        public int IdTipoEmpleado { get; set; }
        public bool ContratoNormativoActivo { get; set; }
        public Nullable<decimal> ImporteDescuentoAhorroLP { get; set; }
        public Nullable<decimal> ImporteDescuentoAhorroCP { get; set; }
        public Nullable<int> IdBanco { get; set; }
        public string CLABECuentaBancaria { get; set; }
        public List<caBeneficiarios> Beneficiario { get; set; }
       
    }


    public class AltaSocioDto
    {
        public ModelCatalogoDatosSocio Socio { get; set; }
        public List<caBeneficiarios> Beneficiarios { get; set; }
    }
}
