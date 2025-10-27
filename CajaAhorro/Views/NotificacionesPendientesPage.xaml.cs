using Money_Box.Models;
using Money_Box.ViewModels;
using Money_Box.Views;
using System;
using System.Linq;
using Microsoft.Maui.Controls;

namespace Money_Box.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificacionesPendientesPage : ContentPage
    {
       private readonly NotificacionesSQLiteViewModel _viewModel;
        public int IdSocio {  get; set; }
        public NotificacionesPendientesPage(int idSocio)
        {
            InitializeComponent();



            var factory = App.ServiceProvider.GetRequiredService<Func<NotificacionesSQLiteViewModel>>();
            _viewModel = factory();
            BindingContext = _viewModel;
            this.IdSocio = idSocio;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
    

            try
            {
              
                await _viewModel.SincronizarNotificacionesDesdeApi(IdSocio);
                await _viewModel.MarcarComoEnviadas(IdSocio);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error al cargar notificaciones: {ex.Message}", "OK");
               
            }
        }

        private async void RegresarBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }


    }

}