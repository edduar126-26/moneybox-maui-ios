using Microsoft.Maui.Networking;
using System.Threading.Tasks;

namespace Money_Box.Helpers
{
    public static class ConnectivityHelper
    {
        public static Task<ConnectivityResult> CheckInternetAsync()
        {
            var result = new ConnectivityResult();

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                result.IsConnected = false;
                result.ErrorMessage = "Sin conexión a Internet. Por favor, revise su conexión.";
            }
            else
            {
                result.IsConnected = true;
            }

            return Task.FromResult(result);
        }
    }

    public class ConnectivityResult
    {
        public bool IsConnected { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
