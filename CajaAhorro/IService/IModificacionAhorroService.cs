using Money_Box.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Money_Box.IService
{
    public interface IModificacionAhorroService
    {
        Task<List<SocioModel>> GetDatosSocio(int idSocio);
        Task<EntRespuesta> GetDatosAhorroCP(int idSocio);
    }
}
