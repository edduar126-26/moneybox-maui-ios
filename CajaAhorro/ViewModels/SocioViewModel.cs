
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Money_Box.Models;
using Money_Box.Services;
using Money_Box.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using Money_Box.IService;
using System.Text;
using System.Threading.Tasks;
using Money_Box.Events;
using Money_Box.Helpers;

namespace Money_Box.ViewModels
{
    public partial class SocioViewModel : ObservableObject
    {

        #region Navegacion
        public event EventHandler<SocioNavigationArgs> NavegarSocio;
        #endregion
        #region Servicios
        private readonly ISocioService _socioService;
        private readonly IDialogService _dialogoService;
    
        #endregion
        #region Commands
        public IRelayCommand ActFechaTerminosCondicionesCommand { get; private set; }
        public IRelayCommand GetExistemailCommand { get; private set; }

        public IRelayCommand GetExisteEmailYRfCommand {  get; private set; }
        #endregion

        #region Variables

        [ObservableProperty] private string ahorroAcumulado;
        #endregion

        #region Constructores

        public caSocio _socio { get; }
        public SocioViewModel(ISocioService socioService, IDialogService dialogoService,caSocio Socio )
        {
            _socioService = socioService;
            _dialogoService = dialogoService;
            _socio = Socio;
        }

        #endregion

        #region InicializarComandos
        public void InicializarComandosSocio()
        {
            ActFechaTerminosCondicionesCommand = new AsyncRelayCommand(() => ActFechaTerminosCondiciones());
            GetExistemailCommand = new AsyncRelayCommand(() => GetExistemail());
            GetExisteEmailYRfCommand = new AsyncRelayCommand(() => GetExisteEmailYRfc());
        }
        #endregion

        #region Async Functions
        async Task ActFechaTerminosCondiciones()
        {
            try
            {

                var connection = await ConnectivityHelper.CheckInternetAsync();
                if (!connection.IsConnected)
                    if (!connection.IsConnected)
                    {
                        await _dialogoService.ShowError(connection.ErrorMessage);
                        return;
                    }

                var Item = await _socioService.ActFechaTerminosCondiciones(_socio.NoEmpleado.Value);

                    if (Item == 1)
                    {
                        await _dialogoService.ShowMessage("¡Hola!", "Bienvenido " + _socio.Nombre);
                        //este navega a menuSocio
                        NavegarSocio?.Invoke(this, new SocioNavigationArgs(_socio));
                    }
                    else
                        await _dialogoService.ShowError(
                            "Ocurrió un error al aceptar los terminos y condiciones");
              
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _dialogoService.ShowError("Ocurrió un error, favor de reportar a sistemas");
            }

            return;
        }

        async Task GetExistemail()
        {
            try
            {

                var connection = await ConnectivityHelper.CheckInternetAsync();
                if (!connection.IsConnected)
                    if (!connection.IsConnected)
                    {
                        await _dialogoService.ShowError(connection.ErrorMessage);
                        return;
                    }

                var Item = await _socioService.GetExistemail(_socio.CorreoElectronico);

                    if (Item == 0)
                        //este navegara a registrolaborales
                        NavegarSocio?.Invoke(this, new SocioNavigationArgs(_socio));
                    else
                        await _dialogoService.ShowError(
                            "El correo ya esta registrado");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _dialogoService.ShowError("Ocurrió un error, favor de reportar a sistemas");
            }
            return;
        }

        //Nueva funcion para validar por Email y RFC
        async Task GetExisteEmailYRfc()
        {
            try
            {

                var connection = await ConnectivityHelper.CheckInternetAsync();
                if (!connection.IsConnected)
                    if (!connection.IsConnected)
                    {
                        await _dialogoService.ShowError(connection.ErrorMessage);
                        return;
                    }

                var service = _socioService;

                    int emailExiste = await service.GetExistemail(_socio.CorreoElectronico);
                    int rfcExiste = await service.ExisteRfc(_socio.RFC);

                    if (emailExiste > 0 && rfcExiste > 0)
                    {
                        await _dialogoService.ShowError("El correo y el RFC ya están registrados");
                    }
                    else if (emailExiste > 0)
                    {
                        await _dialogoService.ShowError("El correo ya está registrado");
                    }
                    else if (rfcExiste > 0)
                    {
                        await _dialogoService.ShowError( "El RFC ya está registrado");
                    }
                    else
                    {
                        
                        //este navegara a registrolaborales
                        NavegarSocio?.Invoke(this, new SocioNavigationArgs(_socio));
                    }
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _dialogoService.ShowError("Ocurrió un error, favor de reportar a sistemas");
            }
        }

        #endregion;

     
    }
}



