using Microsoft.Maui.Controls;
using Money_Box.Models;
using Money_Box.ViewModels;


namespace Money_Box.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConsultaEstadoCuenta : ContentPage
    {
        private readonly caSocio socio;
        private readonly EstadoCuentaViewModel viewModel;

        public ConsultaEstadoCuenta(caSocio Socio)
        {
            InitializeComponent();

            socio = Socio;
            NombreSociolbl.Text = $"{socio.NoEmpleado} - {socio.Nombre.Trim()}";
            NavigationPage.SetHasBackButton(this, false);

            viewModel = App.ServiceProvider.GetRequiredService<EstadoCuentaViewModel>();

            BindingContext = viewModel;

            ConfigurarViewModelAsync();
        }

        private async void ConfigurarViewModelAsync()
        {
            viewModel.InicializarParaConsultaTotales(socio.IdSocio);

            if (viewModel.GetTotalesPrestamosVigentesSocioCommand.CanExecute(null))
                await viewModel.GetTotalesPrestamosVigentesSocioCommand.ExecuteAsync(null);

            if (viewModel.GetDetallePrestamoVigenteSocioCommand.CanExecute(null))
                await viewModel.GetDetallePrestamoVigenteSocioCommand.ExecuteAsync(null);
        }

        private async void RegresarBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MenuSocio(socio));
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new MenuSocio(socio));
            return true;
        }

        //private async void LstDetallePrestVig_ItemTapped(object sender, ItemTappedEventArgs e)
        //{
        //    if (e.Item is DetallePrestamoVigente prestamoSeleccionado)
        //    {
        //        ((ListView)sender).SelectedItem = null;
        //        await Navigation.PushAsync(new ConsultaDetallePrestamo(prestamoSeleccionado, socio));
        //    }
        //}


        private async void LstDetallePrestVig_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var seleccionado = e.CurrentSelection?.FirstOrDefault() as DetallePrestamoVigente;
            if (seleccionado == null) return;

            var cv = (CollectionView)sender;
            cv.SelectedItem = null;

            await Navigation.PushAsync(new ConsultaDetallePrestamo(seleccionado, socio));
        }


    }
}
