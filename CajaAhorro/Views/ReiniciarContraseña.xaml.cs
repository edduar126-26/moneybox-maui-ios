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
    public partial class ReiniciarContraseña : ContentPage
    {
       
        private readonly AccesoViewModel vm;
        bool _nuevaPwdVisible = false;
        bool _confirmNuevaPwdVisible = false;
        public ReiniciarContraseña(string token, int uid)
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            vm = App.ServiceProvider.GetRequiredService<AccesoViewModel>();
           

            vm.Token = token;
            vm.Uid = uid;

            var factory = App.ServiceProvider.GetRequiredService<Func<caSocio, UsersViewModel>>();

            vm.NavegarAcceso += async (s, e) =>
            {
                await Navigation.PushAsync(new Acceso(factory));
            };


            BindingContext = vm;
            vm.Inicializar();
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


        private async void ResetContraseniabtnBtn_Clicked(object sender, EventArgs e)
        {
             await vm.ReiniciarContraseñaCommand.ExecuteAsync(null);
        }


        private async void RegresarBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PopToRootAsync();
            return true;
        }
    }
}