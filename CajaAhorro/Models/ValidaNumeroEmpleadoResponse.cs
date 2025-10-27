using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Money_Box.Models
{
    public class ValidaNumeroEmpleadoResponse
    {
        public bool estatus { get; set; }
        public string mensaje { get; set; }
        public int idEmpresa { get; set; }
        public string nombreEmpresa { get; set; }
    }
}
