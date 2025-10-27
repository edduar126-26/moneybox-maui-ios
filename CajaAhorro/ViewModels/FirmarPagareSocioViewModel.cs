using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Money_Box.IService;
using Money_Box.Services;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace Money_Box.ViewModels
{
    public partial class FirmarPagareSocioViewModel : ObservableObject
    {
        private readonly int _folioSolicitud;
        private readonly IUserService _userService;
        private readonly IDialogService _dialogService;

        [ObservableProperty]
        private string firmaBase64;

        public IAsyncRelayCommand FirmaCommand { get; }

        public FirmarPagareSocioViewModel(int folioSolicitud, IUserService userService, IDialogService dialogService)
        {
            _folioSolicitud = folioSolicitud;
            _userService = userService;
            _dialogService = dialogService;

            FirmaCommand = new AsyncRelayCommand(EnviarFirmaAsync);
        }

        public async Task<bool> EnviarFirmaAsync()
        {
            bool exito = await _userService.EnviarFirmaAsync(FirmaBase64, _folioSolicitud);

            if (exito)
                await _dialogService.ShowMessage("Exito", "Firma enviada correctamente");
            else
                await _dialogService.ShowError("No se pudo enviar la firma");

            return exito;
        }

        public async Task<bool> YaFirmado()
        {
            return await _userService.YaPagareFirmadoAsync(_folioSolicitud);
        }
    }
}
