using Money_Box.Models;
using Money_Box.Models.Local;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Money_Box.IService
{
    public interface ILogInService
    {
        Task<EntRespuesta> Login(string idEmpresa, string usuario, string contrasenia);
        Task<EntRespuesta> Registro(int idEmpresa, int noEmpleado);
        Task<RespuestaBoolCompat> ReiniciarPassword(string token, int uid, string newPassword);
        Task<RespuestaBoolCompat> ValidaEmailCambioContraseña(string email);
    }
}
