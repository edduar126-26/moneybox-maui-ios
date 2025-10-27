using Money_Box.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Money_Box.Models
{


    public class RegistroSocioDto
    {
        public caSocio Socio { get; set; }
        public List<DocumentoSeleccionado> Archivos { get; set; } = new List<DocumentoSeleccionado>(); 

    }
}
