using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Money_Box.Models;

namespace Money_Box.Events
{
    public class FolioNavigationArgs : EventArgs
    {
        public string Folio { get; }
        public caSocio Socio { get; }
        public bool DesdeExpediente { get; }
        public bool OtroParametro { get; }

        public FolioNavigationArgs(string folio, caSocio socio, bool desdeExpediente, bool otroParametro)
        {
            Folio = folio;
            Socio = socio;
            DesdeExpediente = desdeExpediente;
            OtroParametro = otroParametro;
        }
    }
}
