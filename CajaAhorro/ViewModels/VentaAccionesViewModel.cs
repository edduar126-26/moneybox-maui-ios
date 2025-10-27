using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Money_Box.Events;
using Money_Box.Helpers;
using Money_Box.IService;
using Money_Box.Models;
using Money_Box.Models.Local;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Globalization;


namespace Money_Box.ViewModels
{
    public partial class VentaAccionesViewModel : ObservableObject
    {

        #region Servicios
        private readonly IVentaAccionesService _ventaAccionesService;
        private readonly IMovimientosCajaService _movimientosCajaService;
        private readonly IUserService _userService;
        private readonly IPrestamosService _prestamosService;
        private readonly IDialogService _dialogService;
        public event EventHandler<SocioNavigationArgs> OnNavegarAVentaAcciones;
        public event EventHandler<FolioGeneradoRegistroArgs> OnFolioGeneradoRegistro;
        #endregion

        #region Commands
        public IAsyncRelayCommand GetCalAhorroAcumuladoCommand { get; private set; }
        public IAsyncRelayCommand GetCalDeudaActualCommand { get; private set; }
        public IAsyncRelayCommand GetCalImporteVentaAccionesMaximoCommand { get; private set; }
        public IAsyncRelayCommand InsertMovimientoCommand { get; private set; }
        public IAsyncRelayCommand GetValidaMovimientoDuplicadoCommand { get; private set; }


        #endregion

        #region Variables Observables
        [ObservableProperty] private string ahorroAcumulado;
        [ObservableProperty] private string deudaActual;
        [ObservableProperty] private string folio;
        [ObservableProperty] private string importeVentaMaximo;
        #endregion

        #region Constructores
        public caSocio _socio { get; }
        public moMovimientosCaja? _movimiento { get; set; }
        public VentaAccionesViewModel(
            IVentaAccionesService ventaAccionesService,
            IMovimientosCajaService movimientosCajaService,
            IUserService userService,
            IPrestamosService prestamosService,
            IDialogService dialogService,
            caSocio Socio,
            moMovimientosCaja? movimiento = null
            )
        {
            _ventaAccionesService = ventaAccionesService;
            _movimientosCajaService = movimientosCajaService;
            _userService = userService;
            _prestamosService = prestamosService;
            _dialogService = dialogService;
            _socio = Socio;
            _movimiento = movimiento;
        }
      
        #endregion

        #region Inicializadores de comandos

        public void InicializarParaConsulta()
        {
            GetCalAhorroAcumuladoCommand = new AsyncRelayCommand(() => GetCalAhorroAcumulado());
            GetCalDeudaActualCommand = new AsyncRelayCommand(() => GetCalDeudaActual());
            GetCalImporteVentaAccionesMaximoCommand = new AsyncRelayCommand(() => GetCalImporteVentaAccionesMaximo());
        }

        public void InicializarParaValidacionDuplicado()
        {
            GetValidaMovimientoDuplicadoCommand = new AsyncRelayCommand(() => GetValidaMovimientoDuplicado());
        }

        public void InicializarParaRegistro()
        {
            InsertMovimientoCommand = new AsyncRelayCommand(() => InsertMovimiento());
        }

        #endregion

        #region Async Functions
        async Task GetCalAhorroAcumulado()
        {
            try
            {

                var connection = await ConnectivityHelper.CheckInternetAsync();
                if (!connection.IsConnected)
                    if (!connection.IsConnected)
                    {
                        await _dialogService.ShowError(connection.ErrorMessage);
                        return;
                    }


                var Item = await _ventaAccionesService.GetCalAhorroAcumulado(_socio.IdSocio);
                    AhorroAcumulado = string.Format("{0:C2}", Convert.ToDecimal(Item));
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _dialogService.ShowError("Ocurrió un error, favor de reportar a sistemas");
            }

            return;
        }

        async Task GetCalDeudaActual()
        {
            try
            {
                var connection = await ConnectivityHelper.CheckInternetAsync();
                if (!connection.IsConnected)
                    if (!connection.IsConnected)
                    {
                        await _dialogService.ShowError(connection.ErrorMessage);
                        return;
                    }

                var Item = await _ventaAccionesService.GetCalDeudaActual(_socio.IdSocio);
                    DeudaActual = String.Format("{0:C2}", Convert.ToDecimal(Item));
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _dialogService.ShowError("Ocurrió un error, favor de reportar a sistemas");
            }

            return;
        }
        
        async Task GetValidaMovimientoDuplicado()
        {
            try
            {

                var connection = await ConnectivityHelper.CheckInternetAsync();
                if (!connection.IsConnected)
                    if (!connection.IsConnected)
                    {
                        await _dialogService.ShowError(connection.ErrorMessage);
                        return;
                    }

                EntRespuesta response = await _prestamosService
                        .ValidaMovimiento(_socio.NoEmpleado.Value, 6);
                    
                    if (!response.Estado)
                        //para navegar a VentasAcciones
                        OnNavegarAVentaAcciones?.Invoke(this, new SocioNavigationArgs(_socio));

                    else
                        await _dialogService.ShowMessage("Aviso", "Ya tienes una solicitud del mismo tipo pendiente de autorizar");
              
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _dialogService.ShowError("Ocurrió un error, favor de reportar a sistemas");
            }

            return;
        }

        async Task GetCalImporteVentaAccionesMaximo()
        {
            try
            {

                var connection = await ConnectivityHelper.CheckInternetAsync();
                if (!connection.IsConnected)
                    if (!connection.IsConnected)
                    {
                        await _dialogService.ShowError(connection.ErrorMessage);
                        return;
                    }

                var Item = await _ventaAccionesService.GetCalImporteVentaAccionesMaximo(_socio.IdSocio);
                    ImporteVentaMaximo = String.Format("{0:C2}", Convert.ToDecimal(Item));
           

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _dialogService.ShowError("Ocurrió un error, favor de reportar a sistemas");
            }

            return;
        }

        public void SetMovimiento(moMovimientosCaja movimiento) => _movimiento = movimiento;

        async Task InsertMovimiento()
        {
            try
            {
                var connection = await ConnectivityHelper.CheckInternetAsync();
                if (!connection.IsConnected)
                {
                    await _dialogService.ShowError(connection.ErrorMessage);
                    return;
                }

                if (_movimiento == null)
                {
                    await _dialogService.ShowError("Movimiento no inicializado.");
                    return;
                }

                var idPeriodo = await _userService.GetPeriodoVigente(_movimiento.IdSocio);

                var movimientoCaja = new moMovimientosCaja
                {
                    IdSocio = _movimiento.IdSocio,
                    IdPeriodo = Convert.ToInt32(idPeriodo),
                    IdConceptoCaja = _movimiento.IdConceptoCaja,
                    ImporteMovimiento = _movimiento.ImporteMovimiento,
                    CantidadMovimiento = 0,
                    PorcentajeMovimiento = 0,
                    PlazoMovimiento = 0,
                    FechaAlta = DateTime.Today,
                    IdUsuarioAlta = _movimiento.IdSocio,
                    FechaModificacion = DateTime.Today,
                    IdMovimiento = 0,
                    IdUsuarioModifica = 1,
                    PreliminarDefinitivo = "DE",
                    EstatusAutorizacion = "AL",
                    Estatus = true,
                    IdAval = _movimiento.IdAval
                };

                Folio = await _movimientosCajaService.InsertMovimiento(movimientoCaja);

                var cNoSol = JsonConvert.DeserializeObject<List<cNoSolicitud>>(Folio ?? "[]");
                if (cNoSol?.Count > 0)
                {
                    Folio = cNoSol[0].NoSolicitud.ToString();
                    OnFolioGeneradoRegistro?.Invoke(this, new FolioGeneradoRegistroArgs(Folio, _socio, true, false));
                }
                else
                {
                    await _dialogService.ShowError("No se obtuvo folio de la inserción.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _dialogService.ShowError("Ocurrió un error, favor de reportar a sistemas");
            }
        }


        #endregion;

    }
}



