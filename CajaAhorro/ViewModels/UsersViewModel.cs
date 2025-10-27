using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Networking;
using Money_Box.IService;
using Money_Box.Models;
using Money_Box.Views;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Money_Box.Helpers;
using System.Text;
using System.Threading.Tasks;
using Money_Box.Events;

namespace Money_Box.ViewModels
{
    public partial class UsersViewModel : ObservableObject
    {
        // Servicios...
        private readonly ILogInService _logInService;
        private readonly IEmpresaService _empresaService;
        private readonly IUserService _userService;
        private readonly IDialogService _dialogService;

        // Eventos de navegación...
        public event EventHandler? Navegar;
        public event EventHandler<SocioNavigationArgs>? NavegarSocio;
        public event EventHandler<SocioNavigationArgs>? NavegarSocioRegistro;
        public event EventHandler<CambioContraseñaArgs>? NavegarCambioContraseña;



        [ObservableProperty] private string password = string.Empty;
        [ObservableProperty] private bool isPassword = true;



        public string PasswordIcon => IsPassword ? "eye_closed.png" : "eye_open.png";

        // Observable para empresas
        [ObservableProperty]
        private ObservableCollection<caSocio> caEmpresas = new();


        [ObservableProperty]
        private string loginClave = string.Empty;


        public caSocio Socio;

        // Commands
        public IAsyncRelayCommand LoginCommand { get; private set; } = null!;
        public IAsyncRelayCommand AuthCommand { get; private set; } = null!;
        public IAsyncRelayCommand RegistroCommand { get; private set; } = null!;
        public IAsyncRelayCommand GetEmpresasLoginCommand { get; private set; } = null!;
        public IAsyncRelayCommand UpdCambiaContraseniaCommand { get; private set; } = null!;

        public IRelayCommand TogglePasswordCommand { get; }

        public UsersViewModel(
            ILogInService logInService,
            IEmpresaService empresaService,
            IUserService userService,
            IDialogService dialogService,
            caSocio? socio)
        {

            TogglePasswordCommand = new RelayCommand(() => IsPassword = !IsPassword);

            _logInService = logInService;
            _empresaService = empresaService;
            _userService = userService;
            _dialogService = dialogService;

            // Si viene null, crea una, pero siempre la MISMA
            Socio = socio ?? new caSocio();
        }

        public void InicializarComandos()
        {
            LoginCommand = new AsyncRelayCommand(() =>
                Login(LoginClave, CryptoHelper.SHA512(Socio?.Contrasenia ?? ""), Socio?.IdEmpresa ?? 0));

            RegistroCommand = new AsyncRelayCommand(() =>
                Registro(Socio?.NoEmpleado ?? 0));

            GetEmpresasLoginCommand = new AsyncRelayCommand(() =>
                GetEmpresasSocio(LoginClave));

            UpdCambiaContraseniaCommand = new AsyncRelayCommand(() =>
                UpdCambiaContrasenia(Socio?.IdSocio ?? 0, Socio?.Contrasenia ?? ""));

            AuthCommand = new AsyncRelayCommand(() =>
                Auth(LoginClave, CryptoHelper.SHA512(Socio?.Contrasenia ?? ""), Socio?.IdEmpresa ?? 0));
        }

        public async Task GetEmpresasSocio(string clave)
        {
            try
            {
                var connection = await ConnectivityHelper.CheckInternetAsync();
                if (!connection.IsConnected)
                {
                    await _dialogService.ShowError(connection.ErrorMessage);
                    return;
                }

                int idCliente = 1;
                var Items = await _empresaService.GetEmpresas(idCliente, clave);

                if (Items.Estado && Items.Resultado is List<caSocio> lista)
                    CaEmpresas = new ObservableCollection<caSocio>(lista);
                else
                    await _dialogService.ShowError("No se encontraron empresas");
            }
            catch (Exception)
            {
                await _dialogService.ShowError("Error al obtener empresas");
            }
        }

        public async Task Registro(int noEmpleado)
        {
            try
            {
                var connection = await ConnectivityHelper.CheckInternetAsync();
                if (!connection.IsConnected)
                {
                    await _dialogService.ShowError(connection.ErrorMessage);
                    return;
                }

                var Items = await _empresaService.GetEmpresasXEmpleado(noEmpleado);

                if (Items.Estado && Items.Resultado is List<caSocio> lista && lista.Count > 0)
                {
                    CaEmpresas = new ObservableCollection<caSocio>(lista);
                    var respuesta = await _logInService.Registro(CaEmpresas[0].IdEmpresa, noEmpleado);

                    if (respuesta.Estado && respuesta.Resultado is caSocio nuevo)
                    {
                        // Mantén navegación con el socio retornado
                        nuevo.IdEmpresa = CaEmpresas[0].IdEmpresa;
                        NavegarSocioRegistro?.Invoke(this, new SocioNavigationArgs(nuevo));
                    }
                    else
                    {
                        await _dialogService.ShowError(respuesta.Mensaje ?? "Error en registro");
                    }
                }
                else
                {
                    await _dialogService.ShowError("El número de empleado no existe");
                }
            }
            catch (Exception)
            {
                await _dialogService.ShowError("Error al registrar");
            }
        }

        public async Task UpdCambiaContrasenia(int idSocio, string contrasenia)
        {
            try
            {
                var connection = await ConnectivityHelper.CheckInternetAsync();
                if (!connection.IsConnected)
                {
                    await _dialogService.ShowError(connection.ErrorMessage);
                    return;
                }

                await _userService.UpdCambiaContrasenia(idSocio, contrasenia);
                Navegar?.Invoke(this, EventArgs.Empty);
                await _dialogService.ShowMessage("Éxito", "La contraseña se actualizó correctamente");
            }
            catch (Exception)
            {
                await _dialogService.ShowError("Error al cambiar contraseña");
            }
        }

        public async Task Login(string claveUsuario, string contraseniaSha512, int idEmpresa)
        {
            try
            {
                var connection = await ConnectivityHelper.CheckInternetAsync();
                if (!connection.IsConnected)
                {
                    await _dialogService.ShowError(connection.ErrorMessage);
                    return;
                }

                var respuesta = await _logInService.Login(idEmpresa.ToString(), claveUsuario, contraseniaSha512);

                if (respuesta.Estatus && respuesta.Resultado is caSocio socioResp)
                {
                    socioResp.Mensaje = respuesta.Mensaje;
                    socioResp.Contrasenia = contraseniaSha512;

                    if (socioResp.Mensaje.Contains("bloqueado") || socioResp.Mensaje == "No existe")
                    {
                        await _dialogService.ShowError(socioResp.Mensaje);
                        return;
                    }

                    if (socioResp.EstatusSocio != true)
                    {
                        await _dialogService.ShowError("Este usuario no es socio activo");
                        return;
                    }

                    if (socioResp.RequiereReseteo == true)
                    {
                        NavegarCambioContraseña?.Invoke(this, new CambioContraseñaArgs(socioResp, false));
                    }
                    else
                    if (socioResp.RequiereTermCondiciones == true)
                    {
                        NavegarSocio?.Invoke(this, new SocioNavigationArgs(socioResp));
                    }
                    else
                    {
                        await _dialogService.ShowMessage("Bienvenido", $"Hola {socioResp.Nombre}");
                        NavegarSocio?.Invoke(this, new SocioNavigationArgs(socioResp));
                    }
                }
                else
                {
                    await _dialogService.ShowError("Número de empleado y/o contraseña incorrectos");
                }
            }
            catch (Exception)
            {
                await _dialogService.ShowError("Error en login");
            }
        }

        public async Task<bool> Auth(string claveUsuario, string contraseniaSha512, int idEmpresa)
        {
            try
            {
                var connection = await ConnectivityHelper.CheckInternetAsync();
                if (!connection.IsConnected)
                {
                    await _dialogService.ShowError(connection.ErrorMessage);
                    return false;
                }

                var respuesta = await _logInService.Login(idEmpresa.ToString(), claveUsuario, contraseniaSha512);
                return respuesta.Estatus && respuesta.Resultado is caSocio socio && socio.EstatusSocio == true;
            }
            catch
            {
                return false;
            }
        }


        public async Task<bool> VerificarYGuardarNotificaciones(int idSocio)
        {
            var notificaciones = await _userService.ObtenerNotificacionesPendientes(idSocio);
            return notificaciones != null && notificaciones.Any();
        }


        partial void OnIsPasswordChanged(bool oldValue, bool newValue)
            => OnPropertyChanged(nameof(PasswordIcon));
    }

}



