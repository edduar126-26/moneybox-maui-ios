using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Networking;
using Money_Box.Helpers;
using Money_Box.IService;
using Money_Box.Views;
using System;
using System.Threading.Tasks;

namespace Money_Box.ViewModels
{
    public partial class AccesoViewModel : ObservableObject
    {
        private readonly ILogInService _logInService;
        private readonly IDialogService _dialogService;

        public event EventHandler NavegarAcceso;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string emailConfirmacion;


        [ObservableProperty] private string token;
        [ObservableProperty] private int uid;

        [ObservableProperty] private string newPassword;
        [ObservableProperty] private string confirmNewPassword;
        public IAsyncRelayCommand ValidaEmailCambioContraseñaCommand { get; private set; }
        public IAsyncRelayCommand ReiniciarContraseñaCommand { get; private set; }

        public AccesoViewModel(ILogInService logInService, IDialogService dialogService)
        {
            _logInService = logInService;
            _dialogService = dialogService;
        }

        public void Inicializar()
        {
            ValidaEmailCambioContraseñaCommand = new AsyncRelayCommand(ValidaEmailCambioContraseña);
            ReiniciarContraseñaCommand = new AsyncRelayCommand(ResetPassword);
        }

        private async Task ValidaEmailCambioContraseña()
        {
            try
            {
                var connection = await ConnectivityHelper.CheckInternetAsync();
                if (!connection.IsConnected)
                {
                    await _dialogService.ShowError(connection.ErrorMessage);
                    return;
                }

                var respuesta = await _logInService.ValidaEmailCambioContraseña(Email);

                if (respuesta != null)
                {
                    await _dialogService.ShowMessage("Aviso", "Se envió un correo electrónico para cambio de contraseña");
                    NavegarAcceso?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    await _dialogService.ShowError(respuesta.Mensaje ?? "Ocurrió un error al enviar el correo de recuperación");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _dialogService.ShowError("Ocurrió un error, favor de reportar a sistemas");
            }
        }

        private async Task ResetPassword()
        {
            try
            {
                var connection = await ConnectivityHelper.CheckInternetAsync();
                if (!connection.IsConnected)
                {
                    await _dialogService.ShowError(connection.ErrorMessage);
                    return;
                }

                if (string.IsNullOrWhiteSpace(NewPassword) || NewPassword.Length < 8)
                {
                    await _dialogService.ShowError("La contraseña debe tener al menos 8 caracteres.");
                    return;
                }
                if (NewPassword != ConfirmNewPassword)
                {
                    await _dialogService.ShowError("Las contraseñas no coinciden.");
                    return;
                }

                var respuesta = await _logInService.ReiniciarPassword(Token, Uid, NewPassword);
                if (respuesta != null)
                {
                    await _dialogService.ShowMessage("Aviso", "Contraseña restaurada con éxito.");
                    NavegarAcceso?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    await _dialogService.ShowError(respuesta.Mensaje ?? "No se pudo reiniciar la contraseña.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await _dialogService.ShowError("Ocurrió un error, favor de reportar a sistemas.");
            }
        }



    }
}
