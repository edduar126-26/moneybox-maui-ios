using Money_Box.Models;
using Money_Box.Models.Local;
using Newtonsoft.Json;
using Microsoft.Maui.Controls;
using System.Collections.Generic;

namespace Money_Box.Views
{
    public partial class LeyendaBajaCajaAhorro : ContentPage
    {
        private readonly caSocio _socio;

        public LeyendaBajaCajaAhorro(caSocio socio, string folioJson) 
        {
            InitializeComponent();
            _socio = socio;

           
            NoSolicitudtxt.Text = folioJson;

            NombreSociolbl.Text = $"{socio.NoEmpleado} - {socio.Nombre.Trim()}";
            NavigationPage.SetHasBackButton(this, false);
        }

        private async void AceptarBtn_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new MenuSocio(_socio));
        }
    }
}
