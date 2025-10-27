using Money_Box.Models;
using Money_Box.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Money_Box.Views;
using System;
using Money_Box.Helpers;
using System.Linq;
using Microsoft.Maui.Controls;

namespace Money_Box.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CambiodeContraseña : ContentPage
    {
        bool _oldPwdVisible = false;
        bool _nuevaPwdVisible = false;
        bool _confirmNuevaPwdVisible = false;
        private UsersViewModel _viewModel;
        caSocio socio;
        bool flag;

        public CambiodeContraseña(caSocio Socio, bool Flag)
        {
            InitializeComponent();

            socio = Socio;
            flag = Flag;

            NombreSociolbl.Text = Socio.NoEmpleado + " - " + Socio.Nombre.Trim();
            NavigationPage.SetHasBackButton(this, false);

            
            var factory = App.ServiceProvider.GetRequiredService<Func<caSocio, UsersViewModel>>();
            _viewModel = factory(Socio);

            // 3. Inicializar los comandos
            _viewModel.InicializarComandos();

            //Navegacion
            _viewModel.Navegar += async (s, e) =>
            {
                await Navigation.PushAsync(new Acceso(factory)); 
            };
        }

        private async void RegresarBtn_Clicked(object sender, EventArgs e)
        {
            var factory = App.ServiceProvider.GetRequiredService<Func<caSocio, UsersViewModel>>();
            if (flag)
                await ((NavigationPage)Parent).PushAsync(new MenuSocio(socio));
            else
                await ((NavigationPage)Parent).PushAsync(new Acceso(factory));
        }

        void OnToggleOldPwdClicked(object sender, EventArgs e)
        {
            _oldPwdVisible = !_oldPwdVisible;
            Contraseniatxt.IsPassword = !_oldPwdVisible;
            OldPwdEyeBtn.Source = _oldPwdVisible ? "eye_open.png" : "eye_closed.png";
        }

        void OnToggleNuevaPwdClicked(object sender, EventArgs e)
        {
            _nuevaPwdVisible = !_nuevaPwdVisible;
            NuevaContraseniatxt.IsPassword = !_nuevaPwdVisible;
            NuevaPwdEyeBtn.Source = _nuevaPwdVisible ? "eye_open.png" : "eye_closed.png";
        }

        void OnToggleConfirmNuevaPwdClicked(object sender, EventArgs e)
        {
            _confirmNuevaPwdVisible = !_confirmNuevaPwdVisible;
            ConfirmaNuevaContraseniatxt.IsPassword = !_confirmNuevaPwdVisible;
            ConfirmNuevaPwdEyeBtn.Source = _confirmNuevaPwdVisible ? "eye_open.png" : "eye_closed.png";
        }

        private async void GuardarClave_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Contraseniatxt.Text))
            {
                await DisplayAlert("Error", "Ingresar contraseña", "OK");
                return;
            }

            if (CryptoHelper.SHA512(Contraseniatxt.Text) != socio.Contrasenia)
            {
                await DisplayAlert("Error", "La contraseña es incorrecta", "OK");
                return;
            }

            if (CryptoHelper.SHA512(NuevaContraseniatxt.Text) != CryptoHelper.SHA512(ConfirmaNuevaContraseniatxt.Text))
            {
                await DisplayAlert("Error", "La contraseña ingresada no coincide", "OK");
                return;
            }

            if (NuevaContraseniatxt.Text.Length < 8)
            {
                await DisplayAlert("Aviso", "La nueva contraseña debe tener al menos 8 dígitos", "OK");
                return;
            }

       
            socio.Contrasenia = NuevaContraseniatxt.Text;

         
            if (_viewModel.UpdCambiaContraseniaCommand.CanExecute(null)) 
                _viewModel.UpdCambiaContraseniaCommand.Execute(null);
        }

        protected override bool OnBackButtonPressed()
        {
            var factory = App.ServiceProvider.GetRequiredService<Func<caSocio, UsersViewModel>>();
            if (flag)
                ((NavigationPage)Parent).PushAsync(new MenuSocio(socio));
            else
                ((NavigationPage)Parent).PushAsync(new Acceso(factory));
            return true;

        }
    }
}