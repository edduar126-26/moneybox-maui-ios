using Money_Box.Models;
using Money_Box.ViewModels;
using Money_Box.Views;
using System;
using System.Linq;
using Microsoft.Maui.Controls;

namespace Money_Box.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PromesasPago : ContentPage
    {
        caSocio socio;
        int folioSolicitud;
        int idConceptoCaja;
        List<Promesa> ListaPromesas = new List<Promesa>();

        public PromesasPago(caSocio Socio, int FolioSolicitud, int IdConceptoCaja)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            //socio = Socio;
            //folioSolicitud = FolioSolicitud;
            //idConceptoCaja = IdConceptoCaja;

            //NombreSociolbl.Text = Socio.NoEmpleado + " -  " + Socio.Nombre.Trim();
            //ConfigViewModel(); 
        }

        // private void ConfigViewModel()
        //{
        //    PromesasViewModel viewModel = new PromesasViewModel(socio, idConceptoCaja);
        //    viewModel.GetConceptosPromesasCommand.Execute(this);
        //    PckrPromesas.BindingContext = viewModel;
        //}


        //private async void RegresarBtn_Clicked(object sender, EventArgs e)
        //{
        //    await ((NavigationPage)Parent).PushAsync(new 
        //        FolioGeneradoRegistroSocio(folioSolicitud.ToString(), socio, true, false));
        //}


        //private void AceptarBtn_Clicked(object sender, EventArgs e)
        //{
        //    PromesasViewModel viewModel = new PromesasViewModel(socio, ListaPromesas);
        //    viewModel.InsertPromesaCommand.Execute(this);
        //}

        //private async void AgregarBtn_Clicked(object sender, EventArgs e)
        //{
        //    if (PckrPromesas.SelectedIndex != -1 &&
        //        !string.IsNullOrEmpty(CantidadPromesa.Text))
        //    {

        //        ConceptoPromesa conceptoPromesa = (ConceptoPromesa)PckrPromesas.SelectedItem;

        //        Promesa promesa = new Promesa
        //        {
        //            Cantidad = double.Parse(CantidadPromesa.Text),
        //            IdcfPromesaPago = conceptoPromesa.IdPromesaPago,
        //            FolioSolicitud = folioSolicitud, 
        //            Descripcion = conceptoPromesa.DescPromesaPago,
        //            Anio = conceptoPromesa.Anio
        //        };

        //        bool existe = false;

        //        foreach (Promesa promesaPago in ListaPromesas)
        //        {
        //            if (!existe && promesaPago.IdcfPromesaPago == promesa.IdcfPromesaPago)
        //            {
        //                existe = true;
        //            }
        //        }

        //        if (!existe)
        //            ListaPromesas.Add(promesa);
        //        else
        //            await DisplayAlert("Aviso", "El concepto de promesa seleccionado ya existe", "OK");

        //        PckrPromesas.SelectedIndex = -1;
        //        CantidadPromesa.Text = "";

        //        PromesasViewModel viewModel = new PromesasViewModel(socio, ListaPromesas);
        //        LstPromesas.BindingContext = viewModel;


        //    }
        //    else
        //        await DisplayAlert("Aviso", "Debe llenar todos los campos", "OK");
        //}

        //protected override bool OnBackButtonPressed()
        //{
        //    ((NavigationPage)Parent).PushAsync(new MenuSocio(socio));
        //    return true;
        //}




    }
}