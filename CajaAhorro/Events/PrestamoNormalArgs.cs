using Money_Box.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Money_Box.Events
{
    public class PrestamoNormalArgs : EventArgs
    {
        public Prestamo Prestamo { get; set; }
        public caSocio Socio { get; set; }
        public List<ConceptoPromesa> ListaPromesas { get; } = new List<ConceptoPromesa>();

        public PrestamoNormalArgs(Prestamo prestamo, caSocio socio, List<ConceptoPromesa> listaPromesas)
        {
            Prestamo = prestamo;
            Socio = socio;
            ListaPromesas.AddRange(listaPromesas);
        }
    }
}
