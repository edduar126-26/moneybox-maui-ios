
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using Money_Box.Models;
using Money_Box.Models.Local;
using Money_Box.ViewModels;
using Money_Box.Views;
using System;
using System.Linq;

namespace Money_Box.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrestamosEspeciales : ContentPage
    {
        caSocio socio;
        caSocio aval = new caSocio();
        Prestamo prestamo;

        private readonly PrestamosViewModel _viewModel;
        public PrestamosEspeciales(Prestamo Prestamo, caSocio Socio)
        {
            InitializeComponent();

            socio = Socio;
            prestamo = Prestamo;

            NombreSociolbl.Text = Socio.NoEmpleado + " - " + Socio.Nombre.Trim();
            NavigationPage.SetHasBackButton(this, false);

            //App.ServiceProvider viene desde app.xaml.cs
           var factory = App.ServiceProvider.GetRequiredService<Func<caSocio, PrestamosViewModel>>();
            _viewModel = factory(socio);

            _viewModel.InicializarParaConsulta(prestamo);
            _viewModel.InicializarConsultaEmpresa();

            BindingContext = _viewModel;


            ConfigViewModel();
            GetEmpresas(); 
        }

        private void ConfigViewModel()
        {
            _viewModel.GetCalAhorroAcumuladoCommand.ExecuteAsync(null);
            _viewModel.GetCalDeudaActualCommand.ExecuteAsync(null);
            _viewModel.GetCalTasaInteresMensualCommand.ExecuteAsync(null);
            _viewModel.GetCalImporteMaximoPrestamoAntiguedadCommand.ExecuteAsync(null);
            _viewModel.GetCalImporteMaximoDisponiblePrestamoCommand.ExecuteAsync(null);
            _viewModel.GetCalImportePrestamoActualSinReciprocidadCommand.ExecuteAsync(null);
            _viewModel.GetCatPrestamosEspecialesCommand.ExecuteAsync(null);
        }

        private async void RegresarBtn_Clicked(object sender, EventArgs e)
        {
            await ((NavigationPage)Parent).PushAsync(new MovimientosPrestamos(socio));
        }

        private async void TablaAmortizacionBtn_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(MontoPrestamoAsolicitartxt.Text))
                await DisplayAlert("Aviso", "La cantidad del préstamo debe ser mayor a 0", "OK");

            else if (decimal.Parse(MontoPrestamoAsolicitartxt.Text) <= 0)
                await DisplayAlert("Aviso", "La cantidad del préstamo debe ser mayor a 0", "OK");

            else
            {
                Prestamo prestamos = new Prestamo
                {
                    Descuento = MontoPrestamoAsolicitartxt.Text,
                    Reciprocidad = MontoPagarParaReciprocidadtxt.Text,
                    Monto = double.Parse(MontoPrestamoAsolicitartxt.Text),
                    Concepto = prestamo.Concepto
                };

                //await ((NavigationPage)Parent)
                //    .PushAsync(new TablaAmortizacionPrestamo(prestamos, socio, null));
            }
        }

        private async void ValidaAvalBtn_Clicked(object sender, EventArgs e)
        {
        
          _viewModel.InicializarValidaAval();
          await _viewModel.ValidaAvalCommand.ExecuteAsync(null);
        }
        

        protected override bool OnBackButtonPressed()
        {
            ((NavigationPage)Parent).PushAsync(new MovimientosPrestamos(socio));
            return true;
        }

        private void GetEmpresas()
        {
            
            _viewModel.GetListaEmpresasCommand.Execute(this);
        }

        private void MontoPrestamoAsolicitartxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(MontoPrestamoAsolicitartxt.Text))
            {
                Prestamo prestamos = new Prestamo
                {
                    IdSocio = prestamo.IdSocio,
                    Monto = double.Parse(MontoPrestamoAsolicitartxt.Text),
                    Clave = prestamo.Clave
                };

                _viewModel.GetCalImportePagarReciprocidadCommand.ExecuteAsync(null);
                _viewModel.GetCalImporteDescuentoCommand.ExecuteAsync(null); 
            }
        }

        private async void AceptarBtn_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(MontoPrestamoAsolicitartxt.Text))
                await DisplayAlert("Aviso", "La cantidad del préstamo debe ser mayor a 0", "OK");

            else if (decimal.Parse(MontoPrestamoAsolicitartxt.Text) <= 0)
                await DisplayAlert("Aviso", "La cantidad del préstamo debe ser mayor a 0", "OK");

            else
            {
                string montoReciprocidad = MontoPagarParaReciprocidadtxt.Text.Replace("$", "");

                if (decimal.Parse(montoReciprocidad) > 0)
                    await DisplayAlert("Aviso", "Para obtener el préstamo debes depositar " +
                        MontoPagarParaReciprocidadtxt.Text, "OK");

                else
                {
                    moMovimientosCaja movimiento = new moMovimientosCaja
                    {
                        IdSocio = socio.IdSocio,
                        ImporteMovimiento = decimal.Parse(MontoPrestamoAsolicitartxt.Text),
                        IdConceptoCaja = prestamo.IdConceptoCaja,
                        IdAval = aval.IdSocio
                    };

                  
                    //_viewModel.InicializarParaInsertar(movimiento, socio, null);
                    //await _viewModel.InsertMovimientoCommand.ExecuteAsync(null);
                }

                
            }
        }
    }
}