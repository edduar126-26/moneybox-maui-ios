using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Money_Box.Models;

namespace Money_Box.Events
{
    public class VentaAccionesNavigationArgs : EventArgs
    {
        public caSocio Socio { get; }
        public Prestamo Prestamo { get; }

        public VentaAccionesNavigationArgs(caSocio socio, Prestamo prestamo)
        {
            Socio = socio;
            Prestamo = prestamo;
        }
    }
}
