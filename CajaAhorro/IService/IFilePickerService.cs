using Money_Box.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Money_Box.IService
{
    public interface IFilePickerService
    {
        Task<DocumentoSeleccionado?> PickFileAsync(string tipoDocumento);
    }
}
