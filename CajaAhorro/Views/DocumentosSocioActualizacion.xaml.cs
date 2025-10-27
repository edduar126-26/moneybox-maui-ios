using Money_Box.Models;
using Money_Box.ViewModels;
using Money_Box.Views;
using Microsoft.Maui.Media;
using System;
using System.Linq;
using Microsoft.Maui.Controls;
using Money_Box.IService;

namespace Money_Box.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DocumentosSocioActualizacion : ContentPage
    {
        private caSocio socio;
        private DocumentoSeleccionado archivoINE;
        private DocumentoSeleccionado archivoDOM;
        private DocumentoSeleccionado archivoNOM;
        private IFilePickerService _filepicker;
        private readonly DocumentosSocioActualizacionViewModel _viewModel;
        private readonly IDocumentosService _docs;


        public DocumentosSocioActualizacion(caSocio socio, IFilePickerService filepicker    )
        {
            InitializeComponent();
            this.socio = socio;
            NombreSociolbl.Text = $"{socio.NoEmpleado} - {socio.Nombre.Trim()}";

            _viewModel = App.ServiceProvider.GetRequiredService<DocumentosSocioActualizacionViewModel>();
            _filepicker = filepicker;


            _docs = App.ServiceProvider.GetRequiredService<IDocumentosService>();
        }


        private async void BtnSeleccionarINE_Clicked(object sender, EventArgs e)
        {
            archivoINE = await SeleccionarArchivo("INE");
            LblINE.Text = archivoINE != null ? archivoINE.NombreArchivo : "No seleccionado";
        }

        private async void BtnSeleccionarDOM_Clicked(object sender, EventArgs e)
        {
            archivoDOM = await SeleccionarArchivo("Comprobante de domicilio");
            LblDOM.Text = archivoDOM != null ? archivoDOM.NombreArchivo : "No seleccionado";
        }

        private async void BtnSeleccionarNOM_Clicked(object sender, EventArgs e)
        {
            archivoNOM = await SeleccionarArchivo("Comprobante de nómina");
            LblNOM.Text = archivoNOM != null ? archivoNOM.NombreArchivo : "No seleccionado";
        }

    
        private async void BtnTomarFotoINE_Clicked(object sender, EventArgs e)
        {
            archivoINE = await TomarFoto("INE");
            LblINE.Text = archivoINE != null ? archivoINE.NombreArchivo : "No seleccionado";
        }

        private async void BtnTomarFotoDOM_Clicked(object sender, EventArgs e)
        {
            archivoDOM = await TomarFoto("Comprobante de domicilio");
            LblDOM.Text = archivoDOM != null ? archivoDOM.NombreArchivo : "No seleccionado";
        }
        private async void BtnTomarFotoNOM_Clicked(object sender, EventArgs e)
        {
            archivoNOM = await TomarFoto("Comprobante de nómina");
            LblNOM.Text = archivoNOM != null ? archivoNOM.NombreArchivo : "No seleccionado";
        }

        private async void BtnContinuar_Clicked(object sender, EventArgs e)
        {
            if (archivoINE == null || archivoDOM == null || archivoNOM == null)
            {
                await DisplayAlert("Faltan archivos", "Debes seleccionar los 3 archivos antes de continuar.", "OK");
                return;
            }

            AsignarPrefijos();

            var archivos = new List<DocumentoSeleccionado> { archivoINE, archivoDOM, archivoNOM };
            var registroCompleto = new RegistroSocioDto
            {
                Socio = socio,
                Archivos = archivos
            };

            await Navigation.PushAsync(new LeyendaIngresoCajadeAhorro(registroCompleto));
        }


        private Task ShowAlert(string msg) => DisplayAlert("Aviso", msg, "OK");

        private Task<DocumentoSeleccionado?> SeleccionarArchivo(string tipo)
            => _docs.SeleccionarArchivoAsync(tipo, ShowAlert);

        private Task<DocumentoSeleccionado?> TomarFoto(string tipo)
            => _docs.TomarFotoAsync(tipo, ShowAlert);

        private void AsignarPrefijos()
        {
            archivoINE.NombreArchivo = AgregarPrefijo("INE", archivoINE.NombreArchivo);
            archivoDOM.NombreArchivo = AgregarPrefijo("DOM", archivoDOM.NombreArchivo);
            archivoNOM.NombreArchivo = AgregarPrefijo("NOM", archivoNOM.NombreArchivo);
        }

        private string AgregarPrefijo(string prefijo, string nombreOriginal)
        {
            if (!nombreOriginal.StartsWith($"{prefijo}_"))
                return $"{prefijo}_{nombreOriginal}";
            return nombreOriginal;
        }

        private async void BtnSubirINE_Clicked(object sender, EventArgs e)
        {
            if (archivoINE == null)
            {
                await DisplayAlert("Faltante", "Primero selecciona o toma una foto del INE.", "OK");
                return;
            }

            archivoINE.NombreArchivo = AgregarPrefijo("INE", archivoINE.NombreArchivo);
            archivoINE.Tipo = "INE";

            var resultado = await _viewModel.SubirArchivoUnicoAsync(socio.IdSocio, archivoINE);
            if (resultado)
            {
                archivoINE = null;
                LblINE.Text = "No seleccionado";
            }
        }


        private async void BtnSubirDOM_Clicked(object sender, EventArgs e)
        {
            if (archivoDOM == null)
            {
                await DisplayAlert("Faltante", "Primero selecciona o toma una foto del comprobante de domicilio.", "OK");
                return;
            }

            archivoDOM.NombreArchivo = AgregarPrefijo("DOM", archivoDOM.NombreArchivo);
            archivoDOM.Tipo = "DOM";

            var resultado = await _viewModel.SubirArchivoUnicoAsync(socio.IdSocio, archivoDOM);
            if (resultado)
            {
                archivoDOM = null;
                LblDOM.Text = "No seleccionado";
            }
        }

        private async void BtnSubirNOM_Clicked(object sender, EventArgs e)
        {
            if (archivoNOM == null)
            {
                await DisplayAlert("Faltante", "Primero selecciona o toma una foto del comprobante de nómina.", "OK");
                return;
            }

            archivoNOM.NombreArchivo = AgregarPrefijo("NOM", archivoNOM.NombreArchivo);
            archivoNOM.Tipo = "NOM";

            var resultado = await _viewModel.SubirArchivoUnicoAsync(socio.IdSocio, archivoNOM);
            if (resultado)
            {
                archivoNOM = null;
                LblNOM.Text = "No seleccionado";
            }
        }


        private async void RegresarBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

    }
}