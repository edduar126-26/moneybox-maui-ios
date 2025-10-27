using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace Money_Box.Models
{
    public class caSocio
    {
        public int IdSocio { get; set; }
        public int? IdCliente { get; set; }
        public int? NoEmpleado { get; set; }
        public string? ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public string? Nombre { get; set; }
        public string? RFC { get; set; }
        public string? NombreCorto { get; set; }
        public string? Empresa { get; set; }
        public string? CorreoElectronico { get; set; }
        public int IdEmpresa { get; set; }
        public DateTime? FechaIngresoEmpresa { get; set; }
        public decimal? SueldoBrutoMensual { get; set; }
        public int? IdTipoEmpleado { get; set; }
        public decimal? ImporteDescuentoLP { get; set; }
        public int? IdBanco { get; set; }
        public string? CLABECuentaBancaria { get; set; }
        public string? NombreBeneficiario1 { get; set; }
        public decimal? PorcentajeOtorgadoBeneficiario1 { get; set; }
        public decimal? ImporteDescuentoCP { get; set; }
        public string? CURPBeneficiario1 { get; set; }
        public string? NombreBeneficiario2 { get; set; }
        public decimal? PorcentajeOtorgadoBeneficiario2 { get; set; }
        public string? CURPBeneficiario2 { get; set; }
        public int? IdUsuarioModifica { get; set; }
        public string? Contrasenia { get; set; }
        public bool Estatus { get; set; }
        public bool? EstatusSocio { get; set; }
        public bool? EsSocioActivo { get; set; }
        public bool? EsEmpleadoEmpresa { get; set; }
        public string? Mensaje { get; set; }
        public bool? RequiereReseteo { get; set; }
        public bool? RequiereTermCondiciones { get; set; }

        public string Clave => !string.IsNullOrWhiteSpace(RFC) ? RFC : NoEmpleado?.ToString() ?? string.Empty;
    }
}
