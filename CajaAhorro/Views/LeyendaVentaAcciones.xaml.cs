using Money_Box.Models;
using Money_Box.ViewModels;
using Money_Box.Helpers;
using Money_Box.Views;
using System;
using System.Linq;
using Microsoft.Maui.Controls;
using Money_Box.Models.Local;

namespace Money_Box.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LeyendaVentaAcciones : ContentPage
    {
        caSocio socio;
        Prestamo prestamo;
        public LeyendaVentaAcciones(caSocio Socio, Prestamo Prestamo)
        {
            InitializeComponent();
            NombreSociolbl.Text = Socio.NoEmpleado + " - " + Socio.Nombre.Trim();
            NavigationPage.SetHasBackButton(this, false);
            socio = Socio;
            prestamo = Prestamo;

            MontoVentaAccionestxt.Text = string.Format("{0:C2}", prestamo.Monto);

        }

        private async void RegresarBtn_Clicked(object sender, EventArgs e)
        {
            await ((NavigationPage)Parent).PushAsync(new VentaAcciones(socio));
        }

        protected override bool OnBackButtonPressed()
        {
            ((NavigationPage)Parent).PushAsync(new VentaAcciones(socio));
            return true;
        }

        private async void AceptarBtn_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Contraseniatxt.Text))
                await DisplayAlert("Error", "Ingresar Contraseña !!", "OK");

            else if (CryptoHelper.SHA512(Contraseniatxt.Text) != socio.Contrasenia)
                await DisplayAlert("Error", "La contrseña es incorrecta", "OK");

            else
            {
                moMovimientosCaja movimiento = new moMovimientosCaja
                {
                    IdSocio = socio.IdSocio,
                    IdConceptoCaja = prestamo.IdConceptoCaja,
                    ImporteMovimiento = decimal.Parse(prestamo.Monto.Value.ToString()),
                    IdAval = 0
                };

                var factory = App.ServiceProvider.GetRequiredService<Func<caSocio,moMovimientosCaja, VentaAccionesViewModel>>();
                var vm = factory(socio,movimiento);

                vm.InsertMovimientoCommand.Execute(null);
            }
        }

    

    }
}