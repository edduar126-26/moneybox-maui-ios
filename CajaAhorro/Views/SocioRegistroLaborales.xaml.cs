using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using Money_Box.Models;
using Money_Box.ViewModels;
using Money_Box.Views;
using System;
using System.Linq;


namespace Money_Box.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SocioRegistroLaborales : ContentPage
    {
        caSocio socio;

        public SocioRegistroLaborales(caSocio newSocio)
        {
            InitializeComponent();

            NavigationPage.SetHasBackButton(this, false);

            socio = newSocio;

            ConfigViewModel();
        }

        private void ConfigViewModel()
        {
            var viewModelFactory = App.ServiceProvider.GetRequiredService<Func<caSocio, EmpresasViewModel>>();
            var viewModel = viewModelFactory.Invoke(socio);

            viewModel.InicializarComandos();
            viewModel.GetTodoTipoEmpleadoCommand.Execute(null);
            PckrEmpleados.BindingContext = viewModel;
        }

        private async void RegresarBtn_Clicked(object sender, EventArgs e)
        {
            await ((NavigationPage)Parent).PushAsync(new SocioRegistroNombre(socio));
        }

        protected override bool OnBackButtonPressed()
        {
            ((NavigationPage)Parent).PushAsync(new SocioRegistroNombre(socio));
            return true;
        }


        private void SueldoBrutoMensualtxt_Focused(object sender, FocusEventArgs e)
        {
            try
            {
                var texto = SueldoBrutoMensualtxt.Text?.Replace("$", "").Replace(",", "").Trim();
                SueldoBrutoMensualtxt.Text = texto;
            }
            catch { }
        }

        private void SueldoBrutoMensualtxt_Unfocused(object sender, FocusEventArgs e)
        {
            try
            {
                if (double.TryParse(SueldoBrutoMensualtxt.Text, out double valor))
                {
                    SueldoBrutoMensualtxt.Text = valor.ToString("C", new System.Globalization.CultureInfo("es-MX"));
                }
            }
            catch { }
        }


        private double ParseMonedaSeguro(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return 0;

            texto = texto.Replace("$", "").Replace(",", "").Trim();

            return double.TryParse(texto, out double valor) ? valor : 0;
        }

     

        private void EntryMoneda_Focused(object sender, FocusEventArgs e)
        {
            if (sender is Entry entry)
            {
                entry.Text = entry.Text?.Replace("$", "").Replace(",", "").Trim();
            }
        }

        private void EntryMoneda_Unfocused(object sender, FocusEventArgs e)
        {
            if (sender is Entry entry && double.TryParse(entry.Text, out double valor))
            {
                entry.Text = valor.ToString("C", new System.Globalization.CultureInfo("es-MX"));
            }
        }


        private async void Siguientebtn_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAhorroCP.Text))
                txtAhorroCP.Text = "0";

            if (PckrEmpleados.SelectedIndex == -1)
            {
                await DisplayAlert("Aviso", "Debe seleccionar Tipo de Empleado", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(FechaIngesoEmpresatxt.Date.ToString()))
            {
                await DisplayAlert("Aviso", "Debes seleccionar una fecha", "OK");
                return;
            }

           
            double sueldo = ParseMonedaSeguro(SueldoBrutoMensualtxt.Text);
            double lp = ParseMonedaSeguro(ImporteAccionesparaCajatxt.Text);
            double cp = ParseMonedaSeguro(txtAhorroCP.Text);

            if (sueldo <= 0 || lp <= 0 ||
                sueldo > 99999999 || lp > 99999999.99 || cp > 99999999.99)
            {
                await DisplayAlert("Aviso", "La cantidad ingresada no es válida", "OK");
                return;
            }

            if (sueldo <= lp)
            {
                await DisplayAlert("Aviso", "El sueldo bruto mensual debe ser mayor al ingreso a caja", "OK");
                return;
            }

            caTiposEmpleado tiposEmpleado = (caTiposEmpleado)PckrEmpleados.SelectedItem;

            socio.IdTipoEmpleado = tiposEmpleado.idTipoEmpleado;
            socio.FechaIngresoEmpresa = FechaIngesoEmpresatxt.Date;
            socio.SueldoBrutoMensual = Convert.ToDecimal(sueldo);
            socio.ImporteDescuentoLP = Convert.ToDecimal(lp);
            socio.ImporteDescuentoCP = Convert.ToDecimal(cp);

            await ((NavigationPage)Application.Current.MainPage)
                .PushAsync(new SocioRegistroBeneficiarios(socio));
        }


    }
}
