using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Money_Box.Models;

namespace Money_Box.Events
{
    public class SocioNavigationArgs : EventArgs
    {
        public caSocio Socio { get; }

        public SocioNavigationArgs(caSocio socio)
        {
            Socio = socio;
        }
    }
}
