using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using Money_Box.Models;
using Money_Box.ViewModels;
using System.Text.RegularExpressions;
using Money_Box.Views;
using System;
using System.Linq;
using Money_Box.IService;

namespace Money_Box.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResetNewPassword : ContentPage
    {
        private readonly IDialogService _dialogService;
        private readonly AccesoViewModel viewModel;
        public ResetNewPassword()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            viewModel = App.ServiceProvider.GetRequiredService<AccesoViewModel>();
            _dialogService = App.ServiceProvider.GetRequiredService<IDialogService>();
            viewModel.Inicializar();


            var factory = App.ServiceProvider.GetRequiredService<Func<caSocio, UsersViewModel>>();

            viewModel.NavegarAcceso += async (s, e) =>
            {
                await Navigation.PushAsync(new Acceso(factory));
            };


            BindingContext = viewModel;
        }

        private async void EnviarClavebtnBtn_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(viewModel.Email) || string.IsNullOrWhiteSpace(viewModel.EmailConfirmacion))
            {
                await _dialogService.ShowError("Debe llenar todos los campos");
            }
            else if (!viewModel.Email.Trim().Equals(viewModel.EmailConfirmacion.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                await _dialogService.ShowError("El correo electrónico no coincide");
            }
            else if (!IsEmail(viewModel.Email.Trim()))
            {
                await _dialogService.ShowError("Ingresar correo electrónico válido");
            }
            else
            {
                await viewModel.ValidaEmailCambioContraseñaCommand.ExecuteAsync(null);
            }
        }

        private async void CancelarBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }

        protected override bool OnBackButtonPressed()
        {
             Navigation.PopToRootAsync();
            return true;
        }


        public bool IsEmail(string email)
        {
            Regex emailPatten = new Regex("^[_a-z0-9-]+(.[a-z0-9-]+)@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$");
            if (!string.IsNullOrEmpty(email))
                return emailPatten.IsMatch(email);
            else
                return false;

        }
    }
}
