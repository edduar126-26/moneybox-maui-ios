using System;
using System.Collections.Generic;
using System.Text;

namespace Money_Box.Models
{
    public  class Clientes
    {
        public int IdCliente { get; set; }
        public string NombreCliente { get; set; }
        public string NombreContrato { get; set; }
        public string NombrePagare { get; set; }
        public string CLABECuentaBancaria { get; set; }
        public string RazonSocial { get; set; }
        public string RFCCliente { get; set; }
        public string Calle { get; set; }
        public string NumeroExterior { get; set; }
        public string NumeroInterior { get; set; }
        public string Colonia { get; set; }
        public string CodigoPostal { get; set; }
        public Nullable<int> IdEstado { get; set; }
        public string AbreviaturaEstado { get; set; }
        public string NombreEstado { get; set; }
        public Nullable<int> IdPoblacion { get; set; }
        public string NombrePoblacion { get; set; }
        public string NoTelefono { get; set; }
        public string RepresentanteLegal01 { get; set; }
        public string RepresentanteLegal02 { get; set; }
        public string RepresentanteLegal03 { get; set; }
        public bool Estatus { get; set; }
        public Nullable<System.DateTime> FechaAlta { get; set; }
        public Nullable<int> IdUsuarioAlta { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public Nullable<int> IdUsuarioModifica { get; set; }
    }
}
