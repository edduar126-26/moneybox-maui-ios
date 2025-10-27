using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Money_Box.IService
{
    public interface ISocioService
    {
        Task<int> ActFechaTerminosCondiciones(int noEmpleado);
        Task<int> GetExistemail(string email);
        Task<int> ExisteRfc(string RFC);
        Task<int> ObtenerIdEmpresaDesdeAPI(int numeroEmpleado);
    }
}
