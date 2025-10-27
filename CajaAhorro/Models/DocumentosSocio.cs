using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Money_Box.Models
{
    public class DocumentosSocio
    {
        [JsonProperty("urlPagareRelativa")]
        public string UrlPagare { get; set; }

        [JsonProperty("urlContratoRelativa")]
        public string UrlContrato { get; set; }
    }
}
