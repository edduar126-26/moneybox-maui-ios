using Money_Box.Models;
using Money_Box.ViewModels;
using Money_Box.Views;
using System;
using System.Linq;
using Microsoft.Maui.Controls;

namespace Money_Box.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TerminosCondiciones : ContentPage
    {
        public caSocio socio;

        public TerminosCondiciones(caSocio Socio)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            socio = Socio;
        }

        private void AceptarBtn_Clicked(object sender, EventArgs e)
        {
            var factory = App.ServiceProvider.GetRequiredService<Func<caSocio, SocioViewModel>>();
            var viewModel = factory(socio);

            viewModel.InicializarComandosSocio(); 

            viewModel.NavegarSocio += async (s, args) =>
            {
                await Navigation.PushAsync(new MenuSocio(args.Socio));
            };

            viewModel.ActFechaTerminosCondicionesCommand.Execute(null);
        }

        private async void CancelarBtn_Clicked(object s, EventArgs e)
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