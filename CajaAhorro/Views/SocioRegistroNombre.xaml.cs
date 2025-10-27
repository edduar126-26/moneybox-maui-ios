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
    public partial class SocioRegistroNombre : ContentPage
    {
        caSocio socio;
        public SocioRegistroNombre(caSocio newSocio)
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);

            socio = newSocio; 

        }

        private async void CancelarBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }

        public bool IsEmail(string email)
        {
            Regex emailPatten = new Regex(@"^[a-z,A-Z]{1,10}((-|.)\w+)*@\w+(.[a-z0-9-]+)((.[a-z]+)|)((.[a-z]+)|)$");

            if (!string.IsNullOrEmpty(email))
                return emailPatten.IsMatch(email);
            else
                return false;
        }


        private async void Siguientebtn_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ApellidoPaternotxt.Text) ||
                string.IsNullOrEmpty(ApellidoMaternotxt.Text) ||
                string.IsNullOrEmpty(Nombretxt.Text) ||
                string.IsNullOrEmpty(CorreoElectronicotxt.Text) ||
                string.IsNullOrEmpty(RFCtxt.Text))
            {
                await DisplayAlert("Aviso", "Debe llenar toda la información", "OK");
                return;
            }

            if (!IsEmail(CorreoElectronicotxt.Text.Trim()))
            {
                await DisplayAlert("Aviso", "Email no válido", "OK");
                return;
            }

            if (RFCtxt.Text.Length != 13)
            {
                await DisplayAlert("Aviso", "El RFC ingresado no es válido", "OK");
                return;
            }

            socio.IdTipoEmpleado = 0;
            socio.ApellidoPaterno = ApellidoPaternotxt.Text.Trim();
            socio.ApellidoMaterno = ApellidoMaternotxt.Text.Trim();
            socio.Nombre = Nombretxt.Text.Trim();
            socio.RFC = RFCtxt.Text.Trim();
            socio.CorreoElectronico = CorreoElectronicotxt.Text.Trim();

            var factory = App.ServiceProvider.GetRequiredService<Func<caSocio, SocioViewModel>>();
            var viewModel = factory(socio);

            viewModel.NavegarSocio += async (senderVm, args) =>
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await Navigation.PushAsync(new SocioRegistroLaborales(args.Socio));
                });
            };

            viewModel.InicializarComandosSocio();
            viewModel.GetExisteEmailYRfCommand.Execute(null);
        }

    }
}