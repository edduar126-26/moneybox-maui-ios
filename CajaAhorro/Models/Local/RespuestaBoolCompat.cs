using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Money_Box.Models.Local
{
    public class RespuestaBoolCompat
    {
        [Newtonsoft.Json.JsonProperty("Resultado")]
        public bool Resultado { get; set; }

        [Newtonsoft.Json.JsonProperty("Estado")]
        public bool? Estado { get; set; }

        [Newtonsoft.Json.JsonProperty("Estatus")]
        public bool? Estatus { get; set; }

        public string? Mensaje { get; set; }

      
    }
}
