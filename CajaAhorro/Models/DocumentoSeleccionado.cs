using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Money_Box.Models
{
    public class DocumentoSeleccionado
    {
        public string NombreArchivo { get; set; }
        public string Uri { get; set; } 
        public Func<Task<Stream>> AbrirStreamAsync { get; set; }
        public string Tipo { get; internal set; }
        public byte[] Archivo { get; internal set; }
    }
}
