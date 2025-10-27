using Money_Box.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Money_Box.IService
{
    public interface IEmpresaService
    {
        Task<List<caEmpresa>> GetEmpresas();
        Task<EntRespuesta> GetEmpresas(int idCliente, string clave);
        Task<EntRespuesta> GetEmpresasXEmpleado(int noEmpleado);
        Task<EntRespuesta> GetEmpresas(int idCliente);
    }
}
