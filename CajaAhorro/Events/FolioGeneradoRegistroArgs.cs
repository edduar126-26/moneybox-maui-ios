using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Money_Box.Models;

namespace Money_Box.Events
{   
   
    public class FolioGeneradoRegistroArgs: EventArgs
    {
        public string Folio { get; }
        public caSocio Socio { get; }

        public bool CamaraEnabled { get; }
        public bool SocioNuevo { get; }
        public FolioGeneradoRegistroArgs(string folio, caSocio socio, bool camaraenabled, bool socionuevo)
        {
            Folio = folio;
            Socio = socio; 
            CamaraEnabled = camaraenabled;
            SocioNuevo = socionuevo;
        }
    }
}
