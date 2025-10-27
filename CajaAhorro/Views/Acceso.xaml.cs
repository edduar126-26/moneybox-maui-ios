using Money_Box.Models;
using Money_Box.ViewModels;
using Money_Box.Views;
using System;
using System.Linq;
using Microsoft.Maui.Controls;
using Money_Box.Helpers;

namespace Money_Box.Views
{
    public partial class Acceso : ContentPage
    {
        private readonly UsersViewModel _viewModel;
        bool _pwdVisible = false;

        public Acceso(Func<caSocio?, UsersViewModel> vmFactory)
        {
            InitializeComponent();

            string version = AppInfo.VersionString;    
            string build = AppInfo.BuildString;          

            lblVersion.Text = $"Versión {version} (Build {build})";

            var socioCompartido = new caSocio();
            _viewModel = vmFactory(socioCompartido);
            BindingContext = _viewModel;

            _viewModel.InicializarComandos();


            _viewModel.NavegarSocioRegistro += async (s, e) =>
                await Navigation.PushAsync(new SocioRegistroNombre(e.Socio));

            _viewModel.NavegarSocio += async (s, e) =>
            {
                if (e.Socio.RequiereTermCondiciones == true)
                    await Navigation.PushAsync(new TerminosCondiciones(e.Socio));
                else
                    await Navigation.PushAsync(new MenuSocio(e.Socio));
            };

            _viewModel.NavegarCambioContraseña += async (s, e) =>
                await Navigation.PushAsync(new CambiodeContraseña(e.Socio, e.Flag));

            SetClickListeners();
        }

        private void ClaveUsuario_TextChanged(object sender, EventArgs e)
        {
            Ingresarlogo.IsVisible = string.IsNullOrEmpty(ClaveUsuario.Text);

            if (int.TryParse(ClaveUsuario.Text, out int numEmpleado))
            {
                _viewModel.Socio.NoEmpleado = numEmpleado;
                _viewModel.LoginClave = numEmpleado.ToString();
               
            }
        }

        private async void ClaveUsuario_Completed(object sender, EventArgs e)
        {
            await BuscarEmpresasAsync();
        }

        private async void ClaveUsuario_Unfocused(object sender, FocusEventArgs e)
        {
            await BuscarEmpresasAsync();
        }

        private async Task BuscarEmpresasAsync()
        {
            if (_viewModel.GetEmpresasLoginCommand.CanExecute(null))
                await _viewModel.GetEmpresasLoginCommand.ExecuteAsync(null);
        }

        private void RfcUsuario_TextChanged(object sender, EventArgs e)
        {
            Ingresarlogo.IsVisible = string.IsNullOrEmpty(RfcUsuario.Text);
            _viewModel.Socio.RFC = RfcUsuario.Text;
            _viewModel.LoginClave = RfcUsuario.Text;

        }

        private async void RfcUsuario_Completed(object sender, EventArgs e)
        {
            await BuscarEmpresasAsync();
        }

        private async void RfcUsuario_Unfocused(object sender, FocusEventArgs e)
        {
            await BuscarEmpresasAsync();
        }

      



        private async void LoginBtn_Click(object sender, EventArgs e)
        {
            string clave = MetodoSwitch.IsToggled ? RfcUsuario.Text : ClaveUsuario.Text;

            if (string.IsNullOrWhiteSpace(clave))
            {
                await DisplayAlert("Aviso", "Debe ingresar un número de empleado o RFC.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(Contrasenia.Text) || Contrasenia.Text.Length < 8)
            {
                await DisplayAlert("Aviso", "La contraseña debe contener mínimo 8 caracteres.", "OK");
                return;
            }

            if (PckrEmpresas.SelectedItem is not caSocio empresaSeleccionada)
            {
             
                if (PckrEmpresas.ItemsSource is IEnumerable<caSocio> lista && lista.Count() == 1)
                {
                    PckrEmpresas.SelectedIndex = 0;
                    empresaSeleccionada = lista.First();
                }
                else
                {
                    await DisplayAlert("Aviso", "Debe seleccionar una empresa.", "OK");
                    PckrEmpresas.IsVisible = true;
                    return;
                }
            }

    
            _viewModel.Socio.IdEmpresa = empresaSeleccionada.IdEmpresa;
            _viewModel.LoginClave = clave;
            _viewModel.Socio.Contrasenia = Contrasenia.Text;

            await _viewModel.Login(
                _viewModel.Socio.Clave,
                CryptoHelper.SHA512(_viewModel.Socio.Contrasenia),
                _viewModel.Socio.IdEmpresa
            );
        }

        private void MetodoSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            bool usarRfc = e.Value;
            ClaveUsuario.IsVisible = !usarRfc;
            RfcUsuario.IsVisible = usarRfc;

            Ingresarlogo.IsVisible = usarRfc
                ? string.IsNullOrEmpty(RfcUsuario.Text)
                : string.IsNullOrEmpty(ClaveUsuario.Text);
        }


      


        private void BtnRegistrarClick(object sender, EventArgs e)
        {
            var socio = new caSocio()
            {
                IdEmpresa = 0
            };

            ((NavigationPage)Application.Current.MainPage).
                                 PushAsync(new SocioRegistroNombre(socio));
        }


        private void SetClickListeners()
        {
            var resetTap = new TapGestureRecognizer();
            resetTap.Tapped += (s, e) =>
            {
                ((NavigationPage)Application.Current.MainPage).PushAsync(new ResetNewPassword());
            };
            OlvidasteContraseniaBtn.GestureRecognizers.Add(resetTap);

            var registroTap = new TapGestureRecognizer();
            registroTap.Tapped += BtnRegistrarClick;
            lblRegistrate.GestureRecognizers.Add(registroTap);
        }


        void OnTogglePasswordClicked(object sender, EventArgs e)
        {
            _pwdVisible = !_pwdVisible;
            Contrasenia.IsPassword = !_pwdVisible;
            PasswordEyeBtn.Source = _pwdVisible ? "eye_open.png" : "eye_closed.png";
        }


        protected override bool OnBackButtonPressed() => true;
    }

}