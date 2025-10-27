
using Microsoft.Maui.Controls;
using Money_Box.IService;
using Money_Box.Models;
using Money_Box.Models.Local;
using Money_Box.ViewModels;
using Money_Box.Views;
using System;
using System.Linq;

namespace Money_Box.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SocioRegistroDocumentos : ContentPage
    {
        private caSocio socio;
        private readonly IFilePickerService _filePicker;
        public ExpedienteSocioViewModel _expedienteSocio;
        private DocumentoSeleccionado archivoINE;
        private DocumentoSeleccionado archivoDOM;
        private DocumentoSeleccionado archivoNOM;
        private DocumentoSeleccionado archivoFolio;
        private readonly string? _folio;
        private readonly IDocumentosService _docs;

        public SocioRegistroDocumentos(caSocio Socio, string? EsFolio, IFilePickerService filePicker)
        {
            InitializeComponent();
            socio = Socio;
            _filePicker = filePicker;

            _folio = EsFolio;
            _docs = App.ServiceProvider.GetRequiredService<IDocumentosService>();
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            bool esComprobante = !string.IsNullOrWhiteSpace(_folio);

            ContenedorComprobante.IsVisible = esComprobante;
            ContenedorINE.IsVisible = !esComprobante;
            ContenedorDOM.IsVisible = !esComprobante;
            ContenedorNOM.IsVisible = !esComprobante;
        }


        private async void BtnSeleccionarComprobante_Clicked(object sender, EventArgs e)
        {
            archivoFolio = await SeleccionarArchivo("Comprobante");
            LblFolio.Text = archivoFolio != null ? archivoFolio.NombreArchivo : "No seleccionado";
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

      
        private async void BtnTomarFotoComprobante_Clicked(object sender, EventArgs e)
        {
            archivoFolio = await TomarFoto("Comprobante");
            LblFolio.Text = archivoFolio != null ? archivoFolio.NombreArchivo : "No seleccionado";
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
            var btn = (Button)sender;
            btn.IsEnabled = false;
            try
            {
                if (!string.IsNullOrWhiteSpace(_folio))
                {
                    if (archivoFolio == null)
                    {
                        await DisplayAlert("Faltan archivos", "Debes seleccionar o tomar la foto del comprobante.", "OK");
                        return;
                    }

                    AsignarPrefijos(_folio);
                    await EnsureBytesAsync(archivoFolio);
                    var ok = await SubirComprobantePrestamoAsync(archivoFolio, socio, _folio);
                    if (ok)
                    {
                        await DisplayAlert("Éxito", "Comprobante subido correctamente.", "OK");
                        await ((NavigationPage)Parent).PushAsync(new MenuSocio(socio));
                    }
                    return;
                }

                if (archivoINE == null || archivoDOM == null || archivoNOM == null)
                {
                    await DisplayAlert("Faltan archivos", "Debes seleccionar los 3 archivos antes de continuar.", "OK");
                    return;
                }

                AsignarPrefijos(null);

                var archivos = new List<DocumentoSeleccionado>();
                if (archivoINE != null) archivos.Add(archivoINE);
                if (archivoDOM != null) archivos.Add(archivoDOM);
                if (archivoNOM != null) archivos.Add(archivoNOM);

                var registroCompleto = new RegistroSocioDto { Socio = socio, Archivos = archivos };

                await Navigation.PushAsync(new LeyendaIngresoCajadeAhorro(registroCompleto));
            }
            finally
            {
                btn.IsEnabled = true;
            }
        }


        private Task ShowAlert(string msg) => DisplayAlert("Aviso", msg, "OK");

        private async Task<DocumentoSeleccionado?> SeleccionarArchivo(string tipo)
            => await _docs.SeleccionarArchivoAsync(tipo, ShowAlert);

        private async Task<DocumentoSeleccionado?> TomarFoto(string tipo)
            => await _docs.TomarFotoAsync(tipo, ShowAlert);



        private void AsignarPrefijos(string? folio)
        {
            if (string.IsNullOrWhiteSpace(folio))
            {
                if (archivoINE != null) archivoINE.NombreArchivo = AgregarPrefijo("INE", archivoINE.NombreArchivo);
                if (archivoDOM != null) archivoDOM.NombreArchivo = AgregarPrefijo("DOM", archivoDOM.NombreArchivo);
                if (archivoNOM != null) archivoNOM.NombreArchivo = AgregarPrefijo("NOM", archivoNOM.NombreArchivo);
            }
            else
            {
                if (archivoFolio != null) archivoFolio.NombreArchivo = AgregarPrefijoComprobante("Comprobante", _folio, archivoFolio.NombreArchivo);
            }
        }

        private string AgregarPrefijo(string prefijo, string nombreOriginal)
        {
            if (!nombreOriginal.StartsWith($"{prefijo}_"))
                return $"{prefijo}_{nombreOriginal}";
            return nombreOriginal;
        }

        private string AgregarPrefijoComprobante(string prefijo, string folio, string nombreOriginal)
        {
            var ext = Path.GetExtension(nombreOriginal);
            var baseName = $"{prefijo}_{folio}";
            var currentBase = Path.GetFileNameWithoutExtension(nombreOriginal);

            if (currentBase.StartsWith(baseName, StringComparison.OrdinalIgnoreCase))
                return $"{currentBase}{ext}";

            return $"{baseName}{ext}";
        }



        private async Task<bool> SubirComprobantePrestamoAsync(DocumentoSeleccionado doc, caSocio socio, string folio)
        {
            try
            {
                var enviar = new EnviarFoto
                {
                    NoSolicitud = int.Parse(folio),
                    ByteImagen = doc.Archivo,
                    Comprobante = Path.GetFileNameWithoutExtension(doc.NombreArchivo) 
                };

                var factory = App.ServiceProvider.GetRequiredService<Func<ExpedienteSocioViewModel>>();
                _expedienteSocio = factory();
                _expedienteSocio.InicializarComandos();

               
                var args = new ExpedienteSocioViewModel.FotoArgs(enviar, socio);

                _expedienteSocio.InsertaFotoCommand.Execute(args);

                return true;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo subir el comprobante:\n{ex.Message}", "OK");
                return false;
            }
        }


        private static async Task EnsureBytesAsync(DocumentoSeleccionado doc)
        {
            if (doc == null) return;
            if (doc.Archivo == null || doc.Archivo.Length == 0)
            {
                var s = await doc.AbrirStreamAsync();
                var ms = new MemoryStream();
                await s.CopyToAsync(ms);
                doc.Archivo = ms.ToArray();
            }
        }

        private async void RegresarBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(); 
        }
    }
}