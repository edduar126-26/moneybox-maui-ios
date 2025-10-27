using Money_Box.Models;
using Money_Box.Models.Local;
using Money_Box.ViewModels;
using Microsoft.Maui.Controls;
using System;
using Money_Box.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Money_Box.Views
{
    public partial class BajaCajaAhorro : ContentPage
    {
        private readonly caSocio _socio;
        private readonly PrestamosViewModel _viewModel;

        public BajaCajaAhorro(caSocio socio)
        {
            InitializeComponent();
            _socio = socio;

            NombreSociolbl.Text = $"{socio.NoEmpleado} - {socio.Nombre.Trim()}";
            NavigationPage.SetHasBackButton(this, false);

            var prestamo = new Prestamo
            {
                IdSocio = _socio.IdSocio
            };

            
            var factory = App.ServiceProvider.GetRequiredService<Func<caSocio, Prestamo, PrestamosViewModel>>();
            _viewModel = factory(socio,prestamo);
            _viewModel.InicializarParaConsulta(prestamo);

            ConfigViewModel();

            BindingContext = _viewModel;
        }

        public async void ConfigViewModel()
        {
            await _viewModel.GetCalAhorroAcumuladoCommand.ExecuteAsync(null);
            await _viewModel.GetCalDeudaActualCommand.ExecuteAsync(null);
            await _viewModel.GetCalImportePagarBajaCommand.ExecuteAsync(null);
        }

        private async void AceptarBtn_Clicked(object sender, EventArgs e)
        {
            if (!decimal.TryParse(MontoAPagartxt.Text?.Replace("$", "").Trim(), out decimal monto))
            {
                await DisplayAlert("Error", "Monto inválido", "OK");
                return;
            }

            var movimiento = new moMovimientosCaja
            {
                IdSocio = _socio.IdSocio,
                IdConceptoCaja = 2,
                ImporteMovimiento = monto,
                CantidadMovimiento = 0,
                PorcentajeMovimiento = 0,
                PlazoMovimiento = 0,
                FechaAlta = DateTime.Today,
                IdUsuarioAlta = _socio.IdSocio,
                FechaModificacion = DateTime.Today,
                IdUsuarioModifica = 1,
                PreliminarDefinitivo = "DE",
                EstatusAutorizacion = "AL",
                Estatus = true,
                Clave = "BC"
            };


            var factory = App.ServiceProvider.GetRequiredService<Func<caSocio, moMovimientosCaja?, BajaCajaViewModel>>();
            var vm = factory(_socio, movimiento);


            
            vm.NavegarLeyendaBajaCaja += async (s, args) =>
            {
                await Navigation.PushAsync(new LeyendaBajaCajaAhorro(args.Socio, args.FolioJson));
            };

            vm.InicializarComandos(); 
            await vm.BajaCajaAhorroCommand.ExecuteAsync(null); 
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
    }
}
