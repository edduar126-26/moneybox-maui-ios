using Money_Box.Models;
using Money_Box.ViewModels;
using Microsoft.Maui.ApplicationModel;
using Money_Box.Views;
using System;
using System.Linq;
using Microsoft.Maui.Controls;
using Money_Box.Models.Local;

namespace Money_Box.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuSocio : ContentPage
    {
        public caSocio socio;
        private UsersViewModel _viewModel;
        private bool hayNotificacionesPendientes = false;
        private bool animandoCampana = false;
        public MenuSocio(caSocio Socio)
        {
            InitializeComponent();

            NombreSociolbl.Text = Socio.NoEmpleado + " - " + Socio.Nombre.Trim();
            NavigationPage.SetHasBackButton(this, false);
          
            socio = Socio;

            var factory = App.ServiceProvider.GetRequiredService<Func<caSocio, UsersViewModel>>();
            _viewModel = factory(socio);

            BindingContext = _viewModel;
            _ = VerificarNotificacionesAsync();

        }

     
        private async Task VerificarNotificacionesAsync()
        {
             hayNotificacionesPendientes = await _viewModel.VerificarYGuardarNotificaciones(socio.IdSocio);
             PuntoRojo.IsVisible = hayNotificacionesPendientes;

            if (hayNotificacionesPendientes)
            {
                AnimateBell();
                await DisplayAlert("🔔 Aviso", "Tienes notificaciones pendientes", "OK");

            }
            else
            {
                DetenerAnimacionCampana();
            }
        }

        private void AnimateBell()
        {
            if (animandoCampana)
               return;

            animandoCampana = true;
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                while (animandoCampana)
                {
                    await CampanitaBtn.RotateTo(-10, 150, Easing.Linear);
                    await CampanitaBtn.RotateTo(10, 300, Easing.Linear);
                    await CampanitaBtn.RotateTo(0, 150, Easing.Linear);
                    await Task.Delay(3000);
                }
            });
        }

        private void DetenerAnimacionCampana()
        {
            animandoCampana = false;
        }
        private async void NotificacionesBtn_Clicked(object sender, EventArgs e)
        {
            hayNotificacionesPendientes = false;
            PuntoRojo.IsVisible = false;

            await Navigation.PushAsync(new NotificacionesPendientesPage(socio.IdSocio));
        }

        private async void CambiarContraseniaBtn_Clicked(object sender, EventArgs e)
        {
            await ((NavigationPage)Parent).PushAsync(new CambiodeContraseña(socio, true));
        }
       
        private void BajaCajaAhorroBtn_Clicked(object sender, EventArgs e)
        {
            var factory = App.ServiceProvider.GetRequiredService<Func<caSocio, moMovimientosCaja?, BajaCajaViewModel>>();
            var vm = factory(socio, null);
            vm.InicializarComandos();

            vm.NavegarBajaCajaAhorro += async (s, args) =>
            {
                await Navigation.PushAsync(new BajaCajaAhorro(args.Socio));
            };

            vm.GetValidaMovimientoDuplicadoCommand.Execute(null);
        }
        private async void SalirBtn_Clicked(object sender, EventArgs e)
        {
            var acceso = App.ServiceProvider.GetRequiredService<Func<caSocio?, UsersViewModel>>();
            Application.Current.MainPage = new NavigationPage(new Acceso(acceso));
        }
      
        private void ModificacionAhorroBtn_Clicked(object sender, EventArgs e)
        {
            var factory = App.ServiceProvider.GetRequiredService<Func<caSocio, moMovimientosCaja?, ModificacionAhorroViewModel>>();
            var vm = factory(socio, null);
            vm.InicializarCommandosSocio();

            vm.NavegarSocio += async (s, args) =>
            {
                await Navigation.PushAsync(new ModificacionAhorro(args.Socio));
            };

            vm.GetValidaMovimientoDuplicadoCommand.Execute(null); 

        }

        private void ModificacionAhorroCPBtn_Clicked(object sender, EventArgs e)
        {
            var factory = App.ServiceProvider.GetRequiredService<Func<caSocio, moMovimientosCaja?, ModificacionAhorroViewModel>>();
            var vm = factory(socio, null);

            vm.InicializarCommandosSocio();
            vm.NavegarSocio += async (s, args) =>
            {
                await Navigation.PushAsync(new AhorroCortoPlazo(args.Socio));
            };
            vm.GetValidaMovimientoDuplicadoCPCommand.Execute(null);
        }



        private async void ConsultaEstadoCuenta_Clicked(object sender, EventArgs e)
        {
            await ((NavigationPage)Parent).PushAsync(new ConsultaEstadoCuenta(socio));
        }

      
        private void VentaAccionesBtn_Clicked(object sender, EventArgs e)
        {

            var factory = App.ServiceProvider.GetRequiredService<Func<caSocio, moMovimientosCaja?, VentaAccionesViewModel>>();
            var vm = factory(socio, null);

            vm.InicializarParaValidacionDuplicado();

            vm.OnNavegarAVentaAcciones += async (s, args) =>
            {
                await Navigation.PushAsync(new VentaAcciones(args.Socio));
            };

            vm.GetValidaMovimientoDuplicadoCommand.Execute(null);
        }

    
        private async void SolicitudPrestamoBtn_Clicked(object sender, EventArgs e)
        {
            await ((NavigationPage)Parent).PushAsync(new MovimientosPrestamos(socio));
        }

        private async void Actualizar_Documentos(object sender, EventArgs e)
        {
            var factory = App.ServiceProvider.GetRequiredService<Func<caSocio, DocumentosSocioActualizacion>>();
            await Navigation.PushAsync(factory(socio));
        }

        private async void AvisodePrivacidadBtn_Clicked(object sender, EventArgs e)
        {
            await ((NavigationPage)Parent).PushAsync(new AvisoDePrivacidad());
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}