using CommunityToolkit.Mvvm.ComponentModel;
using Money_Box.IService;
using Money_Box.Models;
using Money_Box.Services;
using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace Money_Box.ViewModels
{
    public partial class FirmarPagareViewModel : ObservableObject
    {
        private readonly IUserService _userService;
        private readonly IDialogService _dialogService;


        [ObservableProperty]
        private string urlPagare;

        [ObservableProperty]
        private string urlContrato;

        public int FolioSolicitud { get; }

        public FirmarPagareViewModel(IUserService userService, int folioSolicitud, IDialogService dialogService)
        {
            _userService = userService;
            FolioSolicitud = folioSolicitud;

            _ = LoadDocumentosAsync();
            _dialogService = dialogService;
        }

        private async Task LoadDocumentosAsync()
        {
            try
            {
                var documentos = await _userService.GetRutaArchivosSocioAsync(FolioSolicitud);

                var urlBase = ApiRest.sUrlBase;

                if (documentos != null)
                {
                    urlPagare = urlBase + documentos.UrlPagare;
                    urlContrato = urlBase + documentos.UrlContrato;
                }
                else
                {
                    await _dialogService.ShowError("No se encontraron documentos.");
                }
            }
            catch (Exception ex)
            {
                await _dialogService.ShowError($"Falló la carga de documentos: {ex.Message}");
            }
        }
    }
}
