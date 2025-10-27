using Money_Box.Models;
using Money_Box.ViewModels;
using Money_Box.Views;
using System;
using System.Linq;
using Microsoft.Maui.Controls;

namespace Money_Box.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LeyendaIngresoCajadeAhorro : ContentPage
    {

        private readonly InsertUserViewModel _viewmodel;
        caSocio socio;
        RegistroSocioDto registro;
        public LeyendaIngresoCajadeAhorro(RegistroSocioDto datos)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            registro = datos;
            socio = datos.Socio;

            var factory = App.ServiceProvider.GetRequiredService<Func<RegistroSocioDto, InsertUserViewModel>>();
            _viewmodel = factory(registro);

            Descuentolbl.Text = socio.IdTipoEmpleado == 1 ?
                $"Descuento Semanal {socio.ImporteDescuentoLP}" :
                $"Descuento Quincenal {socio.ImporteDescuentoLP}";

        }

        private async void RegresarBtn_Clicked(object sender, EventArgs e)
        {
            await ((NavigationPage)Parent).PushAsync(new SocioRegistroDatosBancarios(socio));
        }

        private async void AceptarBtn_Clicked(object sender, EventArgs e)
        {


            await _viewmodel.InsertaSocioCommand.ExecuteAsync(null);

        }
    }
}