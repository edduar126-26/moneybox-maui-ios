using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Money_Box.Models.Local
{
    //Modelo MovimientoSocio
    public class Socio
    {
        [PrimaryKey, AutoIncrement]
        public int IdSocio { get; set; }
        public int NoEmpleado { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Nombre { get; set; }
        public int IdEmpresa { get; set; }
        public DateTime FechaIngresoEmpresa { get; set; }
        public decimal SueldoBrutoMensual { get; set; }
        public int IdTipoEmpleado { get; set; }
        public decimal ImporteAccionesTipoA { get; set; }
        public int CantidadAccionesTipoA { get; set; }
        public decimal ImporteAccionesTipoB { get; set; }
        public int CantidadAccionesTipoB { get; set; }
        public decimal ImporteAccionesTipoC { get; set; }
        public int CantidadAccionesTipoC { get; set; }
        public decimal ImporteAccionesSuscritas { get; set; }
        public int CantidadAccionesSuscritas { get; set; }
        public int IdBanco { get; set; }
        public string CLABECuentaBancaria { get; set; }
        public string CorreoElectronico { get; set; }
        public string Calle { get; set; }
        public string NumeroExterior { get; set; }
        public string NumeroInterior { get; set; }
        public int IdPoblacion { get; set; }
        public int IdEstado { get; set; }
        public string CodigoPostal { get; set; }
        public bool Estatus { get; set; }
        public int IdUsuario { get; set; }
        public int IdAval { get; set; }
        public decimal ImporteDescuentoAhorro { get; set; }
        public string NombreBeneficiario1 { get; set; }
        public string PorcentajeOtorgadoBeneficiario1 { get; set; }
        public string CURPBeneficiario1 { get; set; }
        public string NombreBeneficiario2 { get; set; }
        public string PorcentajeOtorgadoBeneficiario2 { get; set; }
        public string CURPBeneficiario2 { get; set; }
        public int IdUsuarioModifica { get; set; }
    }
}
