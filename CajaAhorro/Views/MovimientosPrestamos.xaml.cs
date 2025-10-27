using Money_Box.Models;
using Money_Box.ViewModels;
using Money_Box.Views;
using System;
using System.Linq;
using Microsoft.Maui.Controls;

namespace Money_Box.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MovimientosPrestamos : ContentPage
    {
        public caSocio socio;

        public MovimientosPrestamos(caSocio Socio)
        {
            InitializeComponent();
            socio = Socio;
            NombreSociolbl.Text = Socio.NoEmpleado + " - " + Socio.Nombre.Trim();
            NavigationPage.SetHasBackButton(this, false);
            ConfigViewModel();

        }

        private void ConfigViewModel()
        {
            var viewModel = App.ServiceProvider.GetRequiredService<MovimientosPrestamosViewModel>();

            viewModel.InicializarConsultaPrestamos(socio);
            viewModel.GetTipoPrestamoCommand.Execute(this);

            viewModel.OnNavegarPrestamoNormal += async (s, args) =>
            {
                await Navigation.PushAsync(new PrestamoNormal(args.Prestamo, args.Socio, args.ListaPromesas));
            };

            BindingContext = viewModel;
        }

        private void BtnAceptarPrestamo(object sender, EventArgs e)
        {
            if (PckrPrestamos.SelectedIndex == -1)
            {
                DisplayAlert("Alerta", "Debe Seleccionar un Préstamo", "Ok");
                return;
            }

            var movimiento = (Prestamo)PckrPrestamos.SelectedItem;

            if (BindingContext is MovimientosPrestamosViewModel viewModel)
            {
                viewModel.InicializarConsultaValidaMovimientos(movimiento.IdConceptoCaja, socio);
                viewModel.ValidaMovimientoDuplicadoCommand.Execute(this);
            }
        }

        private async void RegresarBtn_Clicked(object sender, EventArgs e)
        {
           await ((NavigationPage)Parent).PushAsync(new MenuSocio(socio));
        }

        

        protected override bool OnBackButtonPressed()
        {
            ((NavigationPage)Parent).PushAsync(new MenuSocio(socio));
            return true;
        }

    }
}