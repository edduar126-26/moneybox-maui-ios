using Money_Box.Models;
using Money_Box.ViewModels;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;

namespace Money_Box.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TablaAmortizacionPrestamo : ContentPage
    {
        public TablaAmortizacionPrestamo(Prestamo prestamo, caSocio socio, List<ConceptoPromesa>? promesas)  //e igualmente se toma como 1
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);

            var promesasCombinadas = new List<ConceptoPromesa>();

            if (promesas != null)
            {
                promesasCombinadas = CombinarPromesas(promesas);
            }
         
            var factory = App.ServiceProvider
                .GetRequiredService<Func<Prestamo, caSocio, List<ConceptoPromesa>, TablaAmortizacionPrestamoViewModel>>();

            var viewModel = factory(prestamo, socio, promesasCombinadas);

            BindingContext = viewModel;

           
            viewModel.InicializaConsultaTablaPrestamo();
            viewModel.GetTablaAmortizacionPrestamoCommand.Execute(null);
        }

        private List<ConceptoPromesa> CombinarPromesas(List<ConceptoPromesa> promesas)
        {
            var resultado = new List<ConceptoPromesa>();
            foreach (var grupo in promesas.GroupBy(p => p.Mes))
            {
                resultado.Add(new ConceptoPromesa
                {
                    Mes = grupo.Key,
                    Cantidad = grupo.Sum(p => p.Cantidad)
                });
            }
            return resultado;
        }

        private async void RegresarBtn_Clicked(object sender, EventArgs e)
        {
            var vm = BindingContext as TablaAmortizacionPrestamoViewModel;
            var prestamo = vm?.Prestamo;
            var socio = vm?.Socio;
            var promesas = vm?.ConceptoPromesas;

            if (prestamo != null && socio != null && promesas != null)
            {
                prestamo.IdSocio = socio.IdSocio;
                await Navigation.PushAsync(new PrestamoNormal(prestamo, socio, promesas));
            }
        }
    }
}
