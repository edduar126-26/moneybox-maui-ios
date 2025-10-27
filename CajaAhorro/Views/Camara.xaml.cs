using Money_Box.Models;
using Money_Box.ViewModels;
using Money_Box.Models.Local;
using Money_Box.Views;
using Newtonsoft.Json;
using Microsoft.Maui.Media;
using Microsoft.Maui.Controls;
using System;
using System.IO;
using System.Collections.Generic;
using Money_Box.Events;

namespace Money_Box.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Camara : ContentPage
    {
        private readonly ExpedienteSocioViewModel _viewModel;
        private readonly string _noSolicitud;
        private readonly caSocio _socio;
        private string _path;

        public Camara(string noSolicitud, caSocio socio)
        {
            InitializeComponent();

            _noSolicitud = noSolicitud;
            _socio = socio;

            _viewModel = 
            App.ServiceProvider.GetRequiredService<ExpedienteSocioViewModel>();
            _viewModel.InicializarComandos();


            _viewModel.NavegarFolio += OnNavegarFolio;

            BindingContext = _viewModel;

            AceptarBtn.IsVisible = false;
        }



        private void OnNavegarFolio(object sender, FolioNavigationArgs e)
        {
            Navigation.PushAsync(new FolioGeneradoRegistroSocio(e.Folio, e.Socio, true, false));
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            _viewModel.NavegarFolio -= OnNavegarFolio;
        }

        private async void TomarFotoBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();

                if (photo != null)
                {
                    _path = photo.FullPath;
                    AceptarBtn.IsVisible = true;

                    var stream = await photo.OpenReadAsync();
                    Expediente.Source = ImageSource.FromStream(() => stream);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo tomar la foto: {ex.Message}", "OK");
            }
        }

        private async void AceptarBtn_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreComprobante.Text))
            {
                await DisplayAlert("Aviso", "Ingresar nombre de comprobante", "OK");
                return;
            }

            if (string.IsNullOrEmpty(_path))
            {
                await DisplayAlert("Aviso", "No se encontró la imagen capturada", "OK");
                return;
            }

            try
            {
                byte[] bytes = File.ReadAllBytes(_path);
                var cNoSol = JsonConvert.DeserializeObject<List<cNoSolicitud>>(_noSolicitud);

                var foto = new EnviarFoto
                {
                    NoSolicitud = int.Parse(cNoSol[0].NoSolicitud.ToString()),
                    ByteImagen = bytes,
                    Comprobante = txtNombreComprobante.Text
                };

                var args = new ExpedienteSocioViewModel.FotoArgs(foto, _socio);
                await _viewModel.InsertaFotoCommand.ExecuteAsync(args);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un problema al enviar la foto: {ex.Message}", "OK");
            }
        }
    }
}
