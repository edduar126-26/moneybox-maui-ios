using Money_Box.Models;
using Money_Box.ViewModels;
using Money_Box.Views;
using System;
using System.Linq;
using Microsoft.Maui.Controls;

namespace Money_Box.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SocioDefinirContraseña : ContentPage
    {
        private caSocio socio;
        bool _pwdVisible = false;
        bool _confirmVisible = false;

        public SocioDefinirContraseña(caSocio socio)
        {
            InitializeComponent();
            this.socio = socio;
        }



        void OnTogglePasswordClicked(object sender, EventArgs e)
        {
            _pwdVisible = !_pwdVisible;
            PasswordEntry.IsPassword = !_pwdVisible;
            PasswordEyeBtn.Source = _pwdVisible ? "eye_open.png" : "eye_closed.png";
        }

        void OnToggleConfirmClicked(object sender, EventArgs e)
        {
            _confirmVisible = !_confirmVisible;
            ConfirmPasswordEntry.IsPassword = !_confirmVisible;
            ConfirmEyeBtn.Source = _confirmVisible ? "eye_open.png" : "eye_closed.png";
        }

        private async void OnContinueClicked(object sender, EventArgs e)
        {
            string password = PasswordEntry.Text?.Trim();
            string confirmPassword = ConfirmPasswordEntry.Text?.Trim();

            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                await DisplayAlert("Error", "Debes llenar ambos campos.", "OK");
                return;
            }
            if (password.Length < 8 || password.Length > 10)
            {
                await DisplayAlert("Error", "La contraseña debe tener entre 8 y 10 caracteres.", "OK");
                return;
            }
            if (password != confirmPassword)
            {
                await DisplayAlert("Error", "Las contraseñas no coinciden.", "OK");
                return;
            }

            
            socio.Contrasenia = password;

            var factory = App.ServiceProvider.GetRequiredService<Func<caSocio, string?, SocioRegistroDocumentos>>();
            await Navigation.PushAsync(factory(socio,null));
        }
    }
}