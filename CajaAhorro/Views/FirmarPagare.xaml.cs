using Money_Box.Models;
using Money_Box.ViewModels;
using Money_Box.Views;
using System;
using System.Linq;
using Microsoft.Maui.Controls;
using Money_Box.Services;

namespace Money_Box.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FirmarPagare : ContentPage
    {
        public string FolioSolicitud {  get; set; }

        public FirmarPagare(string folioSolicitud)
        {
            InitializeComponent();

            FolioSolicitud = folioSolicitud;


        }


        private void OnVerContratoClicked(object sender, EventArgs e)
        {
            var Url = ApiRest.sUrlBase + "/Registro/DescargarContrato?folioSolicitud=" + FolioSolicitud;
            var viewerUrl = $"https://docs.google.com/gview?embedded=true&url={Uri.EscapeDataString(Url)}";
            pdfViewer.Source = new UrlWebViewSource { Url = viewerUrl };
        }

        private void OnVerPagareClicked(object sender, EventArgs e)
        {
            var Url = ApiRest.sUrlBase + "/Registro/DescargarPagare?folioSolicitud=" + FolioSolicitud;
            var viewerUrl = $"https://docs.google.com/gview?embedded=true&url={Uri.EscapeDataString(Url)}";
            pdfViewer.Source = new UrlWebViewSource { Url = viewerUrl };
        }

        private void OnFirmarClicked(object sender, EventArgs e)
        {

            if (int.TryParse(FolioSolicitud, out int folio))
            {

                Navigation.PushAsync(new FirmarPagareSocio(folio));
            }

        }

        private async void RegresarBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
