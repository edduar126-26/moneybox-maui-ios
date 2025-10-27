using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Money_Box.Events;
using Money_Box.Helpers;
using Money_Box.IService;
using Money_Box.Models;
using Money_Box.Models.Local;
using Money_Box.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Money_Box.ViewModels
{
    public partial class ModificacionAhorroViewModel : ObservableObject
    {
        #region Navegacion
        public event EventHandler<SocioNavigationArgs> NavegarSocio;
        public event EventHandler<FolioGeneradoRegistroArgs> NavegarFolioRegistroSocio;
        #endregion

        #region Servicios
        private readonly IModificacionAhorroService _modificacionAhorroService;
        private readonly IPrestamosService _prestamosService;
        private readonly IUserService _userService;
        private readonly IMovimientosCajaService _movimientosCajaService;
        private readonly IDialogService _dialogService;
        #endregion

        #region Commands
        public IAsyncRelayCommand GetDatosSocioCommand { get;private set; }
        public IAsyncRelayCommand GetDatosAhorroCPCommand { get;private set; }
        public IAsyncRelayCommand GetValidaMovimientoDuplicadoCommand { get;private set; }
        public IAsyncRelayCommand GetValidaMovimientoDuplicadoCPCommand { get;private set; }
        public IAsyncRelayCommand InsertMovimientoCommand { get; private set; }

        #endregion


        #region Variables Observables

        private bool CanInsert() => Movimiento is not null && Movimiento.ImporteMovimiento > 0;

        [ObservableProperty]
        private string sueldoBrutoMensual;

        [ObservableProperty]
        private string fechaIngresoEmpresa;

        [ObservableProperty]
        private string importeDescuentoAhorro;

        [ObservableProperty]
        private string impDescuentoAhorro;

        [ObservableProperty]
        private string impDescuentoPrestamo;


        #endregion

        #region Constructor

        public caSocio Socio { get; }

        public moMovimientosCaja? Movimiento { get; set; }
        public ModificacionAhorroViewModel(
       IModificacionAhorroService modificacionAhorroService,
       IPrestamosService prestamosService,
       IUserService userService,
       IDialogService dialogService,
       IMovimientosCajaService movimientosCajaService,
       caSocio socio,
       moMovimientosCaja? movimiento = null)
        {
            _modificacionAhorroService = modificacionAhorroService;
            _prestamosService = prestamosService;
            _userService = userService;
            _dialogService = dialogService;
            _movimientosCajaService = movimientosCajaService;

            Socio = socio;            // <-- esta es la buena
            Movimiento = movimiento;  // <-- guarda el movimiento recibido (si aplica)
        }

        #endregion

        #region InicializarComandos
        public void InicializarCommandosSocio()
        {
            GetDatosSocioCommand = new AsyncRelayCommand(() => GetDatosSocio());
            GetDatosAhorroCPCommand = new AsyncRelayCommand(() => GetDatosAhorroCP());
            GetValidaMovimientoDuplicadoCommand = new AsyncRelayCommand(() => GetValidaMovimientoDuplicado());
            GetValidaMovimientoDuplicadoCPCommand = new AsyncRelayCommand(() => ValidaMovimientoDuplicadoCP());
        }

        public void InicializarCommandoInsertcion()
        {
            InsertMovimientoCommand = new AsyncRelayCommand(InsertMovimiento, CanInsert);
        }
        #endregion

        #region Funciones
        async Task GetDatosSocio()
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

                    var Item = await _modificacionAhorroService.GetDatosSocio(Socio.IdSocio);

                    if (Item.Count > 0)
                    {
                        if (!Item[0].SueldoBrutoMensual.HasValue)
                            Item[0].SueldoBrutoMensual = 0;

                        if (!Item[0].FechaIngresoEmpresa.HasValue)
                            Item[0].FechaIngresoEmpresa = DateTime.Now;

                        if (!Item[0].ImpDescuentoPrestamo.HasValue)
                            Item[0].ImpDescuentoPrestamo = 0;

                        if (!Item[0].ImpDescuentoAhorro.HasValue)
                            Item[0].ImpDescuentoAhorro = 0;

                        if (!Item[0].ImporteDescuentoLP.HasValue)
                            Item[0].ImporteDescuentoLP = 0;

                        SueldoBrutoMensual = string.Format("{0:C2}", Item[0].SueldoBrutoMensual.Value);
                        FechaIngresoEmpresa = Item[0].FechaIngresoEmpresa.Value.ToShortDateString();
                        ImpDescuentoAhorro = string.Format("{0:C2}", Item[0].ImporteDescuentoLP.Value);
                        ImpDescuentoPrestamo = string.Format("{0:C2}", Item[0].ImpDescuentoPrestamo.Value);
                        ImporteDescuentoAhorro = string.Format("{0:C2}", Item[0].ImpDescuentoAhorro.Value);
                    }
                    else
                        await _dialogService.ShowError(
                            "Ocurrió un error al consultar los datos");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _dialogService.ShowError("Ocurrió un error, favor de reportar a sistemas");
               // await ((NavigationPage)Application.Current.MainPage).PushAsync(new MenuSocio(socio));
                NavegarSocio?.Invoke(this,new SocioNavigationArgs(Socio));
            }

            return;
        }

        async Task GetDatosAhorroCP()
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

                  var Item = await _modificacionAhorroService.GetDatosSocio(Socio.IdSocio);

                    if (Item.Count > 0)
                    {
                        if (!Item[0].SueldoBrutoMensual.HasValue)
                            Item[0].SueldoBrutoMensual = 0;

                        if (!Item[0].FechaIngresoEmpresa.HasValue)
                            Item[0].FechaIngresoEmpresa = DateTime.Now;

                        if (!Item[0].ImporteDescuentoCP.HasValue)
                            Item[0].ImporteDescuentoCP = 0;

                        if (!Item[0].ImpDescuentoAhorro.HasValue)
                            Item[0].ImpDescuentoAhorro = 0;

                        if (!Item[0].ImpDescuentoPrestamo.HasValue)
                            Item[0].ImpDescuentoPrestamo = 0;

                        SueldoBrutoMensual = string.Format("{0:C2}", Item[0].SueldoBrutoMensual.Value);
                        FechaIngresoEmpresa = Item[0].FechaIngresoEmpresa.Value.ToShortDateString();
                        ImpDescuentoAhorro = string.Format("{0:C2}", Item[0].ImporteDescuentoCP.Value);
                        ImpDescuentoPrestamo = string.Format("{0:C2}", Item[0].ImpDescuentoPrestamo.Value);
                        ImporteDescuentoAhorro = string.Format("{0:C2}", Item[0].ImpDescuentoAhorro.Value);

                    }
                    else
                        await _dialogService.ShowError(
                            "Ocurrió un error al consultar los datos");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _dialogService.ShowError("Ocurrió un error, favor de reportar a sistemas");
               
                NavegarSocio?.Invoke(this, new SocioNavigationArgs(Socio));
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
                        .ValidaMovimiento(Socio.IdSocio, 9);

                    if (!response.Estado)
                       
                        NavegarSocio?.Invoke(this, new SocioNavigationArgs(Socio));

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

        async Task ValidaMovimientoDuplicadoCP()
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
                        .ValidaMovimiento(Socio.IdSocio, 12);

                    if (!response.Estado)
                       
                        NavegarSocio?.Invoke(this, new SocioNavigationArgs(Socio));
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

                if (Movimiento is null || Movimiento.ImporteMovimiento <= 0)
                {
                    await _dialogService.ShowError("Movimiento no inicializado o importe inválido."); // esta callendo aqui
                    return;
                }

                var periodoStr = await _userService.GetPeriodoVigente(Socio.IdSocio);
                var idPeriodo = int.TryParse(periodoStr, out var p) ? p : 0;
                if (idPeriodo == 0)
                {
                    await _dialogService.ShowError("No se pudo obtener el período vigente.");
                    return;
                }

                var movimientoCaja = new moMovimientosCaja
                {
                    IdSocio = Socio.IdSocio,
                    IdPeriodo = idPeriodo,
                    IdConceptoCaja = Movimiento.IdConceptoCaja,
                    ImporteMovimiento = Movimiento.ImporteMovimiento,
                    CantidadMovimiento = 0,
                    PorcentajeMovimiento = 0,
                    PlazoMovimiento = 0,
                    FechaAlta = DateTime.Today,
                    IdUsuarioAlta = Socio.IdSocio,
                    FechaModificacion = DateTime.Today,
                    IdUsuarioModifica = 0,
                    PreliminarDefinitivo = "DE",
                    EstatusAutorizacion = "AL",
                    Estatus = true,
                    IdAval = 0
                };

                var folioJson = await _movimientosCajaService.InsertMovimiento(movimientoCaja); // <-- usar el correcto
                var cNoSol = JsonConvert.DeserializeObject<List<cNoSolicitud>>(folioJson ?? "[]");

                if (cNoSol?.Count > 0 && !string.IsNullOrWhiteSpace(cNoSol[0].NoSolicitud.ToString()))
                {
                    var folio = cNoSol[0].NoSolicitud.ToString();
                    NavegarFolioRegistroSocio?.Invoke(this, new FolioGeneradoRegistroArgs(folio, Socio, false, false));
                }
                else
                {
                    await _dialogService.ShowError("Ocurrió un error al crear el movimiento (sin folio).");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _dialogService.ShowError("Ocurrió un error, favor de reportar a sistemas");
            }
        }


        #endregion


    }
}
