using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Money_Box.Models;

namespace Money_Box.Events
{
    public class LeyendaNavigationArgs : EventArgs
    {
        public caSocio Socio { get; }
        public string FolioJson { get; }

        public LeyendaNavigationArgs(caSocio socio, string folioJson)
        {
            Socio = socio;
            FolioJson = folioJson;
        }
    }
}
