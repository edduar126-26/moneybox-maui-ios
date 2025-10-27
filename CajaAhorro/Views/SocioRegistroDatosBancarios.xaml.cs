using Money_Box.Models;
using Money_Box.ViewModels;
using Money_Box.Views;
using System;
using System.Linq;
using Microsoft.Maui.Controls;
using System.Text.RegularExpressions;

namespace Money_Box.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class SocioRegistroDatosBancarios : ContentPage
    {
        caSocio socio; 
        public SocioRegistroDatosBancarios(caSocio Socio)
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
            socio = Socio;

            ConfigViewModel();
        }

        private void ConfigViewModel()
        {
            var viewModel = App.ServiceProvider.GetRequiredService<BancosViewModel>();
            viewModel.InicializarComandos();

            viewModel.GetBancosCommand.Execute(null);
            BindingContext = viewModel;
        }

        private async void RegresarBtn_Clicked(object sender, EventArgs e)
        {
            await ((NavigationPage)this.Parent).PushAsync(new SocioRegistroBeneficiarios(socio));
        }

        protected override bool OnBackButtonPressed()
        {
            ((NavigationPage)this.Parent).PushAsync(new SocioRegistroBeneficiarios(socio));
            return true;
        }

        private async void Siguientebtn_Clicked(object sender, EventArgs e)
        {
            Regex noCuentaPatten = new Regex(@"^[0-9]{18}$");

            var viewModel = (BancosViewModel)BindingContext;

            if (viewModel.BancoSeleccionado == null)
            {
                await DisplayAlert("Error", "Debe seleccionar un banco", "OK");
                return;
            }

            if (string.IsNullOrEmpty(NoCuentatxt.Text))
            {
                await DisplayAlert("Error", "Debe agregar la CLABE Interbancaria", "OK");
                return;
            }

            if (!noCuentaPatten.IsMatch(NoCuentatxt.Text))
            {
                await DisplayAlert("Error", "La CLABE Interbancaria no es válida", "OK");
                return;
            }

            socio.Estatus = false;
            socio.IdUsuarioModifica = 1;

            socio.IdBanco = viewModel.BancoSeleccionado.IdBanco;
            socio.CLABECuentaBancaria = NoCuentatxt.Text;

            await ((NavigationPage)Parent).PushAsync(new SocioDefinirContraseña(socio));
        }
    }
}