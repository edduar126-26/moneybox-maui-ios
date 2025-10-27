using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Networking;
using Money_Box.Helpers;
using Money_Box.IService;
using Money_Box.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;

namespace Money_Box.ViewModels
{
    public partial class BancosViewModel : ObservableObject
    {
        private readonly IBancosService _bancoService;
        private readonly IDialogService _dialogService;

        [ObservableProperty]
        private ObservableCollection<Bancos> caBancos = new();

        [ObservableProperty]
        private Bancos bancoSeleccionado;

        public IAsyncRelayCommand GetBancosCommand { get; private set; }

        public BancosViewModel(IBancosService bancoService, IDialogService dialogService)
        {
            _bancoService = bancoService;
            _dialogService = dialogService;
        }

        public void InicializarComandos()
        {
            GetBancosCommand = new AsyncRelayCommand(GetBancos);
        }

        private async Task GetBancos()
        {
            try
            {
                var connection = await ConnectivityHelper.CheckInternetAsync();
                if (!connection.IsConnected)
                {
                    await _dialogService.ShowError(connection.ErrorMessage);
                    return;
                }

                var bancos = await _bancoService.GetBancos();

                CaBancos.Clear(); 
                foreach (var banco in bancos)
                {
                    CaBancos.Add(banco);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _dialogService.ShowError("Ocurrió un error, favor de reportar a sistemas");
            }
        }

    }
}
