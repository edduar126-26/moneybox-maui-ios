using Money_Box.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Money_Box.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FolioGeneradoRegistroSocio : ContentPage
    {
        public caSocio socio;
        public bool socioNuevo;
        string folio;

        public FolioGeneradoRegistroSocio(string Folio, caSocio Socio, bool CamaraEnabled, bool SocioNuevo)
        {
            InitializeComponent();

            lblAddComp.IsVisible = CamaraEnabled;
            comprobante.IsVisible = CamaraEnabled;
            socioNuevo = SocioNuevo;

            FolioGeneradoMvtotxt.Text = Folio;
            NombreSociolbl.Text = Socio.NoEmpleado + " - " + Socio.Nombre.Trim();
            NavigationPage.SetHasBackButton(this, false);
            folio = Folio;
            socio = Socio;

        }

        private async void AceptarBtn_Clicked(object sender, EventArgs e)
        {
            if (socioNuevo)
                await Navigation.PopToRootAsync();
            else
                await ((NavigationPage)Parent).PushAsync(new MenuSocio(socio));
        }

        private async void CamaraBtn_Clicked(object sender, EventArgs e)
        {
            var factory = App.ServiceProvider.GetRequiredService<Func<caSocio, string?, SocioRegistroDocumentos>>();
            await Navigation.PushAsync(factory(socio,folio));
  
        }

    }
}