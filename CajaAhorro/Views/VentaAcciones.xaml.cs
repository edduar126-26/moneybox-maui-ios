using Money_Box.Models;
using Money_Box.ViewModels;
using Money_Box.Views;
using System;
using System.Linq;
using Microsoft.Maui.Controls;
using Money_Box.Models.Local;

namespace Money_Box.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VentaAcciones : ContentPage
    {
        public caSocio socio;
        private VentaAccionesViewModel _vmConsulta;

        public VentaAcciones(caSocio Socio)
        {
            InitializeComponent();
            socio = Socio;
            NombreSociolbl.Text = $"{Socio.NoEmpleado} - {Socio.Nombre.Trim()}";
            NavigationPage.SetHasBackButton(this, false);

           
            var factoryConsulta = App.ServiceProvider
                .GetRequiredService<Func<caSocio, moMovimientosCaja?, VentaAccionesViewModel>>();
            _vmConsulta = factoryConsulta(socio, null);
            _vmConsulta.InicializarParaConsulta();

            BindingContext = _vmConsulta;

           
            _vmConsulta.GetCalAhorroAcumuladoCommand.Execute(null);
            _vmConsulta.GetCalDeudaActualCommand.Execute(null);
            _vmConsulta.GetCalImporteVentaAccionesMaximoCommand.Execute(null);
        }


        private async void RegresarBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(); 
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PopAsync();
            return true;
        }

        private async void AceptarBtn_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(MontoAccionesVendertxt.Text))
            {
                await DisplayAlert("Aviso", "Ingrese monto de acciones a vender", "OK");
                return;
            }

            if (!decimal.TryParse(MontoAccionesVendertxt.Text, out var monto) || monto <= 0)
            {
                await DisplayAlert("Aviso", "La cantidad debe ser mayor a 0", "OK");
                return;
            }

           
            var movimiento = new moMovimientosCaja
            {
                IdSocio = socio.IdSocio,
                IdConceptoCaja = 6,             
                ImporteMovimiento = monto
               
            };

          
            var factoryRegistro = App.ServiceProvider
                .GetRequiredService<Func<caSocio, moMovimientosCaja?, VentaAccionesViewModel>>();
            var vmRegistro = factoryRegistro(socio, movimiento);
            vmRegistro.InicializarParaRegistro();

           
            vmRegistro.OnFolioGeneradoRegistro += async (s, args) =>
            {
               
                await Navigation.PushAsync(new FolioGeneradoRegistroSocio(args.Folio, args.Socio, false, false));
            };

            vmRegistro.InsertMovimientoCommand.Execute(null);
        }


    }
}