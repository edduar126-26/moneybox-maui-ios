using Money_Box.Models;
using Money_Box.ViewModels;
using Money_Box.Views;
using System;
using System.Linq;
using Microsoft.Maui.Controls;
using Money_Box.Helpers;

namespace Money_Box.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Autenticar : ContentPage
    {
        private readonly UsersViewModel _usersViewModel;
        private readonly FirmarPagareSocioViewModel _firmarPagareViewModel;
        private readonly caSocio _socio = new();
        private readonly int _folioSolicitud;

        public Autenticar(int folioSolicitud)
        {
            InitializeComponent();

            _folioSolicitud = folioSolicitud;

            // construir socio base
            _socio = new caSocio();

           
            var usersFactory = App.ServiceProvider.GetRequiredService<Func<caSocio, UsersViewModel>>();
            _usersViewModel = usersFactory(_socio);

      
            _firmarPagareViewModel = App.ServiceProvider.GetRequiredService<Func<int, FirmarPagareSocioViewModel>>()
                .Invoke(folioSolicitud);

            _usersViewModel.InicializarComandos();

            BindingContext = _usersViewModel;

            VerificarFirma();
        }

        private async void VerificarFirma()
        {
            bool firmado = await _firmarPagareViewModel.YaFirmado();

            if (firmado)
            {
                await DisplayAlert("Aviso", "Los Documentos ya fueron firmados.", "OK");
                await Navigation.PopAsync();
            }
        }

        private async void ClaveUsuario_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ClaveUsuario.Text) && int.TryParse(ClaveUsuario.Text, out var numEmpleado))
            {
                _usersViewModel.Socio.NoEmpleado = numEmpleado;
               

                await _usersViewModel.GetEmpresasLoginCommand.ExecuteAsync(null);
                PckrEmpresas.BindingContext = _usersViewModel;
            }
        }

        private async void LoginBtn_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ClaveUsuario.Text))
            {
                await DisplayAlert("Aviso", "Debe ingresar un usuario", "OK");
                return;
            }

            if (Contrasenia.Text.Length < 8)
            {
                await DisplayAlert("Aviso", "La contraseña debe contener mínimo 8 dígitos", "OK");
                return;
            }

            if (PckrEmpresas.ItemsSource is null || PckrEmpresas.ItemsSource.OfType<object>().Count() == 0)
            {
                await DisplayAlert("Aviso", "No se encontró ningún empleado", "OK");
                return;
            }

            if (PckrEmpresas.SelectedIndex == -1)
            {
                await DisplayAlert("Aviso", "Debe seleccionar una empresa", "OK");
                return;
            }

            var socioSeleccionado = (caSocio)PckrEmpresas.SelectedItem;

            _usersViewModel.Socio.IdEmpresa = socioSeleccionado.IdEmpresa;
            _usersViewModel.Socio.Contrasenia = Contrasenia.Text;

            bool autenticado = await _usersViewModel.Auth(
                _usersViewModel.Socio.Clave,
                CryptoHelper.SHA512(_usersViewModel.Socio.Contrasenia),
                _usersViewModel.Socio.IdEmpresa
            );

            if (autenticado)
            {
                await Navigation.PushAsync(new FirmarPagare(_folioSolicitud.ToString()));
            }
            else
            {
                await DisplayAlert("Acceso Denegado", "Credenciales incorrectas o usuario inactivo", "OK");
            }
        }
    }

}