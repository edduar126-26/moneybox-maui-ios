using System;
using System.Collections.Generic;
using System.Text;

namespace Money_Box.Models
{
    [Serializable]
    public class DtoRespuesta <T>
    {
       
            public bool Estado { get; set; }
            public bool Caduca { get; set; }
            public string Mensaje { get; set; }
            public Exception Excepcion { get; set; }
      
            public T Resultado { get; set; }

            public static implicit operator DtoRespuesta<T>(DtoRespuesta<List<string>> v)
            {
                throw new NotImplementedException();
            }
        
    }
}
