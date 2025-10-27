using Money_Box.Models;
using Money_Box.ViewModels;
using Money_Box.Views;
using System;
using System.Linq;
using Microsoft.Maui.Controls;

namespace Money_Box.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConsultaDetallePrestamo : ContentPage
    {
        private readonly EstadoCuentaViewModel _viewModel;
        private readonly caSocio _socio;
        private readonly DetallePrestamoVigente _detallePrestamo;

        public ConsultaDetallePrestamo(DetallePrestamoVigente detallePrestamo, caSocio socio)
        {
            InitializeComponent();

            _socio = socio;
            _detallePrestamo = detallePrestamo;

            NombreSociolbl.Text = $"{socio.NoEmpleado} - {socio.Nombre.Trim()}";
            NavigationPage.SetHasBackButton(this, false);

            
            BindingContext = _detallePrestamo;
            Prestamolbl.Text = _detallePrestamo.TipoPrestamo.Trim();

            
            _viewModel = App.ServiceProvider.GetRequiredService<EstadoCuentaViewModel>();
            _viewModel.InicializarParaConsultaPromesas(_socio.IdSocio, _detallePrestamo.IdMovimiento);

            
            _viewModel.GetPromesasXPrestamoCommand?.ExecuteAsync(null);

           
          //  LstPromesas.BindingContext = _viewModel;
        }

        private async void RegresarBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            MainThread.BeginInvokeOnMainThread(async () => await Navigation.PopAsync());
            return true; 
        }
    }

}