using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;


namespace Money_Box.Models.Local
{
    public class moMovimientosCaja
    {
        [JsonProperty(PropertyName = "IdMovimiento")]
        public int IdMovimiento { get; set; }
        [JsonProperty(PropertyName = "FolioSolicitud")]
        public long Foliosolicitud { get; set; }
        [JsonProperty(PropertyName = "IdSocio")]
        public int IdSocio { get; set; }
        [JsonProperty(PropertyName = "IdPeriodo")]
        public int IdPeriodo { get; set; }
        [JsonProperty(PropertyName = "IdConceptoCaja")]
        public int IdConceptoCaja { get; set; }
        [JsonProperty(PropertyName = "ImporteMovimiento")]
        public decimal ImporteMovimiento { get; set; }
        [JsonProperty(PropertyName = "CantidadMovimiento")]
        public decimal? CantidadMovimiento { get; set; }
        [JsonProperty(PropertyName = "PorcentajeMovimiento")]
        public decimal? PorcentajeMovimiento { get; set; }
        [JsonProperty(PropertyName = "PlazoMovimiento")]
        public int? PlazoMovimiento { get; set; }
        [JsonProperty(PropertyName ="IdPrestamo")]
        public int IdPrestamo { get; set; }
        [JsonProperty(PropertyName = "FechaAlta")]
        public DateTime FechaAlta { get; set; }
        [JsonProperty(PropertyName = "IdUsuarioAlta")]
        public int IdUsuarioAlta { get; set; }
        [JsonProperty(PropertyName = "FechaModificacion")]
        public DateTime? FechaModificacion { get; set; }
        [JsonProperty(PropertyName = "IdUsuarioModifica")]
        public int? IdUsuarioModifica { get; set; }
        [JsonProperty(PropertyName = "PreliminarDefinitivo")]
        public string PreliminarDefinitivo { get; set; }
        [JsonProperty(PropertyName = "EstatusAutorizacion")]
        public string EstatusAutorizacion { get; set; }
        [JsonProperty(PropertyName = "Estatus")]
        public bool Estatus { get; set; }
        [JsonProperty(PropertyName ="EstatusPago")]
        public int EstatusPago { get; set; }
        public string Clave { get; set; }
        public int? IdAval { get; set; }
    }
    public class cNoSolicitud
    {
        public long NoSolicitud { get; set; }
    }
    public class EnviarFoto
    {
        public long NoSolicitud { get; set; }
        public byte[] ByteImagen { get; set; }
        public string Comprobante { get; set; }
    }
    public class GetCallValidaMovimientos
    {
        public bool AplicaMovimiento { get; set; }
        public string MensajeAviso { get; set; }
    }

    public class Respuesta
    {
        public decimal ImporteMaximoVentaAcciones { get; set; }
        public bool AplicaMovimiento { get; set; }
        public string MensajeAviso { get; set; }
    }

    public class changePassword
    {
        public int IdSocio { get; set; }
        public string NuevoPassword { get; set; }
    }
}
