using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Money_Box.Events;
using Money_Box.Helpers;
using Money_Box.IService;
using Money_Box.Models;
using Money_Box.Models.Local;
using Money_Box.Views;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Money_Box.ViewModels
{
    public partial class BajaCajaViewModel : ObservableObject
    {
        #region Servicios

        private readonly IExpedienteSocioService _expedienteSocioService;
        private readonly IUserService _userService;
        private readonly IMovimientosCajaService _movimientosCajaService;
        private readonly IPrestamosService _prestamosService;
        private readonly IDialogService _dialogService;

        #endregion

        #region Navegacion

        public event EventHandler<SocioNavigationArgs> NavegarBajaCajaAhorro;
        public event EventHandler<LeyendaNavigationArgs> NavegarLeyendaBajaCaja;


        #endregion

        #region Variables


        public caSocio _socio { get; }
        public moMovimientosCaja _movimiento { get; }


        #endregion

        #region Comandos

        public IAsyncRelayCommand BajaCajaAhorroCommand { get; private set; }
        public IAsyncRelayCommand GetValidaMovimientoDuplicadoCommand { get; private set; }

        #endregion

        #region Constructor
        
        public BajaCajaViewModel(
            IExpedienteSocioService expedienteSocioService,
            IUserService userService,
            IMovimientosCajaService movimientosCajaService,
            IPrestamosService prestamosService,
            IDialogService dialogService,
            caSocio Socio,
            moMovimientosCaja movimientoCaja = null
          )
        {
            _expedienteSocioService = expedienteSocioService;
            _userService = userService;
            _movimientosCajaService = movimientosCajaService;
            _prestamosService = prestamosService;
            _dialogService = dialogService;
            _socio = Socio;
            _movimiento = movimientoCaja;
        }

        #endregion

       

        #region Inicialización de Comandos

        public void InicializarComandos()
        {
            if (_movimiento != null)
                BajaCajaAhorroCommand = new AsyncRelayCommand(BajaCajaAhorro);
            else
                GetValidaMovimientoDuplicadoCommand = new AsyncRelayCommand(GetValidaMovimientoDuplicado);
        }

        #endregion

        #region Funciones

        private async Task BajaCajaAhorro()
        {
            try
            {
                var connection = await ConnectivityHelper.CheckInternetAsync();
                if (!connection.IsConnected)
                {
                    await _dialogService.ShowError(connection.ErrorMessage);
                    return;
                }

                var response = await _expedienteSocioService.GetCalValidaMovimientos(
                    _socio.IdSocio,
                    double.Parse(_movimiento.ImporteMovimiento.ToString()),
                    _movimiento.Clave);

                if (response.AplicaMovimiento)
                {
                    var periodo = await _userService.GetPeriodoVigente(_socio.IdSocio);
                    _movimiento.IdPeriodo = int.Parse(periodo);
                    _movimiento.IdAval = 0;

                    var folioJson = await _movimientosCajaService.InsertMovimiento(_movimiento);


                    var cNoSol = JsonConvert.DeserializeObject<List<cNoSolicitud>>(folioJson ?? "[]");

                    if (cNoSol?.Count > 0 && !string.IsNullOrWhiteSpace(cNoSol[0].NoSolicitud.ToString()))
                    {
                        var folio = cNoSol[0].NoSolicitud.ToString();
                      
                        NavegarLeyendaBajaCaja?.Invoke(this, new LeyendaNavigationArgs(_socio, folio)); 
                    }
                    else
                    {
                        await _dialogService.ShowError("Ocurrió un error al crear el movimiento (sin folio).");
                    }
                    
                }
                else
                {
                    await _dialogService.ShowError(response.MensajeAviso);
                }
            }
            catch (Exception ex)
            {
                await _dialogService.ShowError("Error al procesar solicitud: " + ex.Message);
            }
        }

        private async Task GetValidaMovimientoDuplicado()
        {
            try
            {
                var connection = await ConnectivityHelper.CheckInternetAsync();
                if (!connection.IsConnected)
                {
                    await _dialogService.ShowError(connection.ErrorMessage);
                    return;
                }

                var response = await _prestamosService.ValidaMovimiento(_socio.IdSocio, 2);

                if (!response.Estado)
                {
                    
                    NavegarBajaCajaAhorro?.Invoke(this, new SocioNavigationArgs(_socio));
                }
                else
                {
                    await _dialogService.ShowMessage("Aviso", "Ya tienes una solicitud del mismo tipo pendiente de autorizar");
                }
            }
            catch (Exception ex)
            {
                await _dialogService.ShowError("Error al validar movimiento: " + ex.Message);
            }
        }

        #endregion
    }
}