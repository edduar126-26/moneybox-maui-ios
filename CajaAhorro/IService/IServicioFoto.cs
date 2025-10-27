using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Money_Box.IService
{
    public interface IServicioFoto
    {
        Task<FileResult> SeleccionarFotoAsync();
        Task<FileResult> CapturarFotoAsync();
        Task<HttpResponseMessage> SubirFotoAsync(FileResult file, string endpoint);
    }
}
