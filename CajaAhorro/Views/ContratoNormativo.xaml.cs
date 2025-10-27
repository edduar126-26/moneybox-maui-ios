using Money_Box.Models;
using Money_Box.ViewModels;
using Microsoft.Maui.Controls;
using System;
using Money_Box.Services;

namespace Money_Box.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContratoNormativo : ContentPage
    {
        private readonly int _idUsuario;
        private readonly FirmaContratoNormativoViewModel viewModel;

        public ContratoNormativo(int idUsuario)
        {
            InitializeComponent();

            var factory = App.ServiceProvider.GetRequiredService<Func<int, FirmaContratoNormativoViewModel>>();
            viewModel = factory(idUsuario);

            _idUsuario = idUsuario;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
             CargarContrato(_idUsuario);
             VerificarFirma();
        }

        private async void VerificarFirma()
        {
            bool firmado = await viewModel.ContratoFirmado(_idUsuario);

            if (firmado)
            {
                await DisplayAlert("Aviso", "El Contrato ya fue firmado.", "OK");
                await Navigation.PopAsync();
            }
        }

        private void CargarContrato(int idUsuario)
        {
            try
            {
                var Url = ApiRest.sUrlBase + "/Registro/DescargarContratoNormativo?IdUsuario=" + idUsuario;
                var viewerUrl = $"https://docs.google.com/gview?embedded=true&url={Uri.EscapeDataString(Url)}"; 

                webViewContrato.Source = new UrlWebViewSource { Url = viewerUrl };
                webViewContrato.IsVisible = true;
                lblMensaje.Text = "Revisa el contrato antes de firmarlo.";
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al cargar el contrato: " + ex.Message;
            }
        }

        private void OnFirmarClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FirmaDeContratoNormativo(_idUsuario));
        }

        private async void RegresarBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
