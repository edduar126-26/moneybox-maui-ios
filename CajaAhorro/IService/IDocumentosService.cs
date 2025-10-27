using Money_Box.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Money_Box.IService
{
    internal interface IDocumentosService
    {
        Task<DocumentoSeleccionado?> TomarFotoAsync(string tipo, Func<string, Task> showAlert);
        Task<DocumentoSeleccionado?> SeleccionarArchivoAsync(string tipo, Func<string, Task> showAlert);
    }
}
