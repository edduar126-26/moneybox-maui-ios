using Money_Box.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Money_Box.Events
{
    public class CambioContraseñaArgs : EventArgs
    {
        public caSocio Socio { get; }

        public bool Flag { get; }
        public CambioContraseñaArgs(caSocio socio, bool flag)
        {
            Socio = socio;
            Flag = flag;
        }
    }
}
