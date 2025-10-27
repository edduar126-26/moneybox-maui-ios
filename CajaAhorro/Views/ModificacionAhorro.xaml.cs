using Money_Box.Models;
using Money_Box.ViewModels;
using Microsoft.Maui.Controls;
using System;
using Money_Box.Models.Local;

namespace Money_Box.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModificacionAhorro : ContentPage
    {
        private readonly ModificacionAhorroViewModel _viewModel;
        private caSocio _socio { get; set; }
        public ModificacionAhorro(caSocio socio)
        {
            InitializeComponent();

            _socio = socio;

            NavigationPage.SetHasBackButton(this, false);
            NombreSociolbl.Text = socio.NoEmpleado + " - " + socio.Nombre.Trim();

            // Crear instancia básica de movimiento
            var movimiento = new moMovimientosCaja
            {
                IdSocio = socio.IdSocio,
                IdConceptoCaja = 9,
                FechaAlta = DateTime.Today,
                IdUsuarioAlta = socio.IdSocio
            };

            // Usar factory con socio y movimiento base
            var factory = App.ServiceProvider.GetRequiredService<Func<caSocio, moMovimientosCaja, ModificacionAhorroViewModel>>();
            _viewModel = factory(socio, movimiento); // Aqui trata de mandar datos del movimiento pero

            _viewModel.InicializarCommandosSocio();

            _viewModel.NavegarSocio += (s, e) => Navigation.PushAsync(new MenuSocio(e.Socio));
            _viewModel.NavegarFolioRegistroSocio += (s, e) =>
                Navigation.PushAsync(new FolioGeneradoRegistroSocio(e.Folio, e.Socio, false, false));

            BindingContext = _viewModel;

            _viewModel.GetDatosSocioCommand.Execute(null);
        }

        private async void RegresarBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MenuSocio(_socio));
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new MenuSocio(_socio));
            return true;
        }

        private async void AceptarBtn_Clicked(object sender, EventArgs e)
        {
            if (decimal.TryParse(DescuentoCambiarAtxt.Text, out var importe) && importe > 0)
            {
               
                _viewModel.Movimiento ??= new moMovimientosCaja
                {
                    IdSocio = _socio.IdSocio,
                    IdConceptoCaja = 9,
                    FechaAlta = DateTime.Today,
                    IdUsuarioAlta = _socio.IdSocio
                };
                _viewModel.Movimiento.ImporteMovimiento = importe;

                _viewModel.InicializarCommandoInsertcion();
                await _viewModel.InsertMovimientoCommand.ExecuteAsync(null);
            }
            else
            {
                await DisplayAlert("Aviso", "La cantidad debe ser mayor a 0", "OK");
            }
        }
    }
}
