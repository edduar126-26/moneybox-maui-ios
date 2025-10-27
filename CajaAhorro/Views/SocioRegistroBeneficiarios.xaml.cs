using Money_Box.Models;
using Money_Box.ViewModels;
using Money_Box.Views;
using System;
using System.Linq;
using Microsoft.Maui.Controls;

namespace Money_Box.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SocioRegistroBeneficiarios : ContentPage
    {
        caSocio socio;
        public SocioRegistroBeneficiarios(caSocio Socio)
        {
            InitializeComponent();

            NavigationPage.SetHasBackButton(this, false);
            socio = Socio;
        }

        private async void RegresarBtn_Clicked(object sender, EventArgs e)
        {
            await ((NavigationPage)this.Parent).PushAsync(new SocioRegistroLaborales(socio));
        }

        protected override bool OnBackButtonPressed()
        {
            ((NavigationPage)this.Parent).PushAsync(new SocioRegistroLaborales(socio));
            return true;
        }

        private async void Siguientebtn_Clicked(object sender, EventArgs e)
        {
            PorcentajeOtorgado2txt.Text = "0" + PorcentajeOtorgado2txt.Text;

            if (string.IsNullOrEmpty(Beneficiario1Nombretxt.Text) ||
                string.IsNullOrEmpty(PorcentajeOtorgado1txt.Text) ||
                string.IsNullOrEmpty(Curp1txt.Text))

                await DisplayAlert("Aviso", "Debe llenar por lo menos los datos del Beneficiario 1", "OK");

            else if (!string.IsNullOrEmpty(Beneficiario2Nombretxt.Text) &&
                Beneficiario1Nombretxt.Text.Trim().Equals(Beneficiario2Nombretxt.Text.Trim()))

                await DisplayAlert("Aviso", "No puede registrar el mismo beneficiario", "OK");

            else if (string.IsNullOrEmpty(PorcentajeOtorgado2txt.Text) &&
                int.Parse(PorcentajeOtorgado1txt.Text) != 100)

                await DisplayAlert("Aviso", "El porcentaje otorgado debe ser igual a 100", "OK");

            else if ((int.Parse(PorcentajeOtorgado1txt.Text.Split('.')[0]) +
                int.Parse(PorcentajeOtorgado2txt.Text.Split('.')[0])) != 100)

                await DisplayAlert("Aviso", "Ambos porcentajes otorgados deben sumar 100", "OK");

            else if (!string.IsNullOrEmpty(Curp2txt.Text) &&
                Curp1txt.Text.Trim().Equals(Curp2txt.Text.Trim()))

                await DisplayAlert("Aviso", "No se puede registrar el mismo CURP", "OK");

            else
            {
                socio.NombreBeneficiario1 = Beneficiario1Nombretxt.Text;
                socio.PorcentajeOtorgadoBeneficiario1 = int.Parse(PorcentajeOtorgado1txt.Text);
                socio.CURPBeneficiario1 = Curp1txt.Text;

                if (string.IsNullOrEmpty(Beneficiario2Nombretxt.Text) ||
                    string.IsNullOrEmpty(PorcentajeOtorgado2txt.Text) ||
                    string.IsNullOrEmpty(Curp2txt.Text))
                {
                    socio.NombreBeneficiario2 = "";
                    socio.PorcentajeOtorgadoBeneficiario2 = 0;
                    socio.CURPBeneficiario2 = "";
                }
                else
                {
                    socio.NombreBeneficiario2 = Beneficiario2Nombretxt.Text;
                    socio.PorcentajeOtorgadoBeneficiario2 = int.Parse(PorcentajeOtorgado2txt.Text);
                    socio.CURPBeneficiario2 = Curp2txt.Text;
                }

                await ((NavigationPage)Parent).PushAsync(new SocioRegistroDatosBancarios(socio));
            }
        }
    }
}