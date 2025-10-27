using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Networking;
using Money_Box.Data;
using Money_Box.Events;
using Money_Box.Helpers;
using Money_Box.IService;
using Money_Box.Models;
using Money_Box.Models.Local;
using Money_Box.Services;
using Money_Box.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;



namespace Money_Box.ViewModels
{
    public partial class PrestamosViewModel : ObservableObject
    {
        #region Navegacion
        public event EventHandler OnNavegarAcceso;
        public event EventHandler<FolioGeneradoRegistroArgs> NavegarFolioGeneradoRegistro;
        public event EventHandler<SocioNavigationArgs> OnNavegarMenuSocio;
        #endregion


        #region Servicios
        private readonly IPrestamosService _prestamoService;
        private readonly IUserService _userService;
        private readonly IMovimientosCajaService _movimientosCajaService;
        private readonly IClienteService _clienteService;
        private readonly IEmpresaService _empresaService;
        private readonly IPromesasService _promesasService;
        private readonly IExpedienteSocioService _expedienteSocioService;
        private readonly IDialogService _dialogService;
        #endregion


        #region Variables
        [ObservableProperty] private ObservableCollection<CaPrestamosEspeciales> caPrestamosEspeciales = new();
        [ObservableProperty] private CaPrestamosEspeciales prestamosSeleccionada;
        [ObservableProperty] private int idConceptoCaja;
        [ObservableProperty] private string importePagarBaja;
        [ObservableProperty] private string ahorroAcumulado;
        [ObservableProperty] private string deudaActual;
        [ObservableProperty] private string importeMaximoPrestamoAntiguedad;
        [ObservableProperty] private string tasaInteresMensual;
        [ObservableProperty] private string montoSolicitado;
        [ObservableProperty] private string importeMaximoDisponiblePrestamo;
        [ObservableProperty] private string importePagarReciprocidad;
        [ObservableProperty] private string importeDescuento;
        [ObservableProperty] private string importePrestamoActualSinReciprocidad;
        [ObservableProperty] private bool muestraPromesas;

        private CancellationTokenSource? _cts;
      

        public List<CaPrestamosEspeciales> lstPrestEsp = new(); 

        
        [ObservableProperty]
        private ObservableCollection<Prestamo> listaPrestamos;


        [ObservableProperty]
        private ObservableCollection<caEmpresa> listaEmpresas = new();


        [ObservableProperty]
        private string numeroEmpleadoAval;

        [ObservableProperty]
        private caEmpresa empresaSeleccionada;

        [ObservableProperty]
        private caSocio socioAval;

        [ObservableProperty]
        private bool aceptarHabilitado;




        #endregion


        #region Declaracion Comandos
        public IAsyncRelayCommand GetCalAhorroAcumuladoCommand { get; private set; }
        public IAsyncRelayCommand GetCalDeudaActualCommand { get; private set; }
        public IAsyncRelayCommand GetCalImportePagarBajaCommand { get; private set; }
        public IAsyncRelayCommand GetCalImporteMaximoPrestamoAntiguedadCommand { get; private set; }
        public IAsyncRelayCommand GetCalTasaInteresMensualCommand { get; private set; }
        public IAsyncRelayCommand GetCalImportePagarReciprocidadCommand { get; private set; }
        public IAsyncRelayCommand GetCalImporteMaximoDisponiblePrestamoCommand { get; private set; }
        public IAsyncRelayCommand GetCalImporteDescuentoCommand { get; private set; }
        public IAsyncRelayCommand GetCalImportePrestamoActualSinReciprocidadCommand { get; private set; }
        public IAsyncRelayCommand GetCatPrestamosEspecialesCommand { get; private set; }
        public IAsyncRelayCommand GetCallIdIdConceptoCajaCommand { get; private set; }
        public IAsyncRelayCommand InsertMovimientoCommand { get; private set; }

        public IAsyncRelayCommand GetTipoPrestamoCommand { get; private set; }
        public IAsyncRelayCommand GetListaEmpresasCommand { get; private set; }

        public IAsyncRelayCommand ValidaAvalCommand { get; private set; }

        public IAsyncRelayCommand ValidaMovimientoCommand { get; private set; }

     
        #endregion


        #region Constructor

        //se pasa un caSocio
        private caSocio _socio {  get; }
        private Prestamo _prestamo { get; set; }

        public PrestamosViewModel(
                IPrestamosService prestamosServices,
                IUserService userservices,
                IMovimientosCajaService movimientosCajaService,
                IPromesasService promesasService,
                IDialogService dialogService,
                IClienteService clienteService,
                IExpedienteSocioService expedienteSocioService,
                IEmpresaService empresaService,
                caSocio Socio,
                Prestamo prestamo)
        {
            _userService = userservices;
            _prestamoService = prestamosServices;
            _movimientosCajaService = movimientosCajaService;
            _promesasService = promesasService;
            _dialogService = dialogService;
            _clienteService = clienteService;
            _expedienteSocioService = expedienteSocioService;
            _empresaService = empresaService;
            _socio = Socio;
            _prestamo = prestamo;

           

        }
        #endregion

        #region Inicializar Comandos

        //Se Agrega esta
        public void InicializarConsultaPrestamos()
        {
            GetTipoPrestamoCommand = new AsyncRelayCommand(() => GetPrestamos(_socio.IdEmpresa, _socio.IdTipoEmpleado));
        }

        public void InicializarValidaAval()
        {
              ValidaAvalCommand = new AsyncRelayCommand(() => ValidaAval());
        }
    
        // y esta tambien
        public void InicializarConsultaEmpresa()
        {
            GetListaEmpresasCommand = new AsyncRelayCommand(() => GetEmpresas());
        }

        public void InicializarValidacionMoviemiento()
        {
            ValidaMovimientoCommand = new AsyncRelayCommand(() => ValidaMovimiento());
        }
        public void InicializarParaConsulta(Prestamo prestamo)
        {
            GetCalAhorroAcumuladoCommand = new AsyncRelayCommand(() => GetCalAhorroAcumulado(prestamo.IdSocio.Value));
            GetCalDeudaActualCommand = new AsyncRelayCommand(() => GetCalDeudaActual(prestamo.IdSocio.Value));
            GetCalImportePagarBajaCommand = new AsyncRelayCommand(() => GetCalImportePagerBaja(prestamo.IdSocio.Value));
            GetCalImporteMaximoPrestamoAntiguedadCommand = new AsyncRelayCommand(() => GetCalImporteMaximoPrestamoAntiguedad(prestamo.IdSocio.Value));
            GetCalTasaInteresMensualCommand = new AsyncRelayCommand(() => GetCalTasaInteresMensual(prestamo.Clave));
            GetCalImporteMaximoDisponiblePrestamoCommand = new AsyncRelayCommand(() => GetCalImporteMaximoDisponiblePrestamo(prestamo.IdSocio.Value, prestamo.Clave));
            GetCalImportePagarReciprocidadCommand = new AsyncRelayCommand(() => GetCalImportePagarReciprocidad(prestamo.IdSocio.Value, prestamo.Clave, prestamo.Monto.Value.ToString()));
            GetCalImporteDescuentoCommand = new AsyncRelayCommand(() => GetCalImporteDescuento(prestamo.IdSocio.Value, prestamo.Clave, prestamo.Monto.Value.ToString()));
            GetCalImportePrestamoActualSinReciprocidadCommand = new AsyncRelayCommand(() => GetCalImportePrestamoActualSinReciprocidad(prestamo.IdSocio.Value, prestamo.Clave));
            GetCatPrestamosEspecialesCommand = new AsyncRelayCommand(() => GetCatPrestamosEspeciales(prestamo.Clave));
            GetCallIdIdConceptoCajaCommand = new AsyncRelayCommand(() => GetCallIdIdConceptoCaja(prestamo.Clave));
        }

        public void InicializarParaInsertar(moMovimientosCaja movimientosCaja, caSocio socio, List<ConceptoPromesa>? promesas = null)
        {
            InsertMovimientoCommand = new AsyncRelayCommand(() => InsertMovimiento(movimientosCaja, socio, promesas));
        }




        #endregion


        #region Funciones


        public void ConfigurarSegunPrestamo(Prestamo p)
        {
      
            MuestraPromesas = p.RequierePromesa && (p.PlazoPrestamo >= 12);
        }

        async Task GetCalImportePrestamoActualSinReciprocidad(int IdSocio, string ClaveConcepto)
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

                var Item = await _prestamoService.GetCalImportePrestamoActualSinReciprocidad(IdSocio, ClaveConcepto);
                ImportePrestamoActualSinReciprocidad = string.Format("{0:C2}", Convert.ToDecimal(Item));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _dialogService.ShowError("Ocurrió un error, favor de reportar a sistemas");
            }
        }

        private async Task GetCalImportePagerBaja(int idSocio)
        {
            var connection = await ConnectivityHelper.CheckInternetAsync();
            if (!connection.IsConnected)
                if (!connection.IsConnected)
                {
                    await _dialogService.ShowError(connection.ErrorMessage);
                    return;
                }

            var result = await _userService.GetCalImporteBajaCajaAhorro(idSocio);
            ImportePagarBaja = string.Format("{0:C2}", Convert.ToDecimal(result));

        }

        private async Task GetCalAhorroAcumulado(int idSocio)
        {
            var connection = await ConnectivityHelper.CheckInternetAsync();
            if (!connection.IsConnected)
                if (!connection.IsConnected)
                {
                    await _dialogService.ShowError(connection.ErrorMessage);
                    return;
                }

            var result = await _prestamoService.GetCalAhorroAcumulado(idSocio);
            AhorroAcumulado = string.Format("{0:C2}", Convert.ToDecimal(result));


        }

        private async Task GetCalDeudaActual(int idSocio)
        {
            var connection = await ConnectivityHelper.CheckInternetAsync();
            if (!connection.IsConnected)
                if (!connection.IsConnected)
                {
                    await _dialogService.ShowError(connection.ErrorMessage);
                    return;
                }

            var result = await _prestamoService.GetCalDeudaActual(idSocio);
            DeudaActual = string.Format("{0:C2}", Convert.ToDecimal(result));

        }

        async Task GetCalImporteMaximoPrestamoAntiguedad(int idSocio)
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

                var Item = await _prestamoService.GetCalImporteMaximoPrestamoAntiguedad(idSocio);
                ImporteMaximoPrestamoAntiguedad = string.Format("{0:C2}", Convert.ToDecimal(Item));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _dialogService.ShowError("Ocurrió un error, favor de reportar a sistemas");
            }
        }

        async Task GetCalTasaInteresMensual(string claveConcepto)
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

                var Item = await _prestamoService.GetCalTasaInteresMensual(claveConcepto);
                TasaInteresMensual = Item + "%";

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Shell.Current.DisplayAlert("Error!", "Ocurrió un error, favor de reportar a sistemas", "OK");
            }
        }

        async Task GetCalImporteMaximoDisponiblePrestamo(int idSocio, string claveConcepto)
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

                var Item = await _prestamoService.GetCalImporteMaximoDisponiblePrestamo(idSocio, claveConcepto);
                ImporteMaximoDisponiblePrestamo = string.Format("{0:C2}", Convert.ToDecimal(Item));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _dialogService.ShowError("Ocurrió un error, favor de reportar a sistemas");
            }
        }

        async Task GetCalImportePagarReciprocidad(int idSocio, string claveConcepto, string monto)
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

                var Item = await _prestamoService.GetCalImportePagarReciprocidad(idSocio, claveConcepto, monto);
                ImportePagarReciprocidad = string.Format("{0:C2}", Convert.ToDecimal(Item));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _dialogService.ShowError("Ocurrió un error, favor de reportar a sistemas");
            }
        }


        private static string LimpiarMonto(string? texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return "";
            // Elimina $ y comas y espacios
            texto = texto.Replace("$", "").Replace(",", "").Trim();
            return texto;
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        private async Task CalcularDescuentoAsync(string? texto)
        {
           
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            var ct = _cts.Token;

            try
            {
                var limpio = LimpiarMonto(texto);
                if (string.IsNullOrWhiteSpace(limpio)) { ImporteDescuento = ""; return; }

            
                if (!decimal.TryParse(limpio, NumberStyles.Number, CultureInfo.InvariantCulture, out var monto) || monto <= 0)
                {
                    ImporteDescuento = "";
                    return;
                }

             
                if (GetCalImportePagarReciprocidadCommand is IAsyncRelayCommand cmdRec)
                    await cmdRec.ExecuteAsync(null);

             
                var montoInvariant = monto.ToString(CultureInfo.InvariantCulture);

                await GetCalImporteDescuento(_socio.IdSocio, _prestamo.Clave, montoInvariant);

                
            }
            catch (OperationCanceledException) { }
        }

        async Task GetCalImporteDescuento(int idSocio, string claveConcepto, string monto)
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

                var Item = await _prestamoService.GetCalImporteDescuento(idSocio, claveConcepto, monto);
                ImporteDescuento = string.Format("{0:C2}", Convert.ToDecimal(Item));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _dialogService.ShowError("Ocurrió un error, favor de reportar a sistemas");
            }
        }

        async Task GetCatPrestamosEspeciales(string cveConceptos)
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

                var Items = await _prestamoService.GetCatPrestamosEspeciales();

                foreach (var prestamo in Items)
                {
                    CaPrestamosEspeciales.Add(prestamo);

                    CaPrestamosEspeciales prestamoEspecial = new CaPrestamosEspeciales
                    {
                        IdConceptoCaja = prestamo.IdConceptoCaja,
                        ClaveConceptoCaja = prestamo.ClaveConceptoCaja,
                        ConceptoCaja = prestamo.ConceptoCaja,
                        PlazoPrestamo = prestamo.PlazoPrestamo,
                        TasaInteresesPrestamo = prestamo.TasaInteresesPrestamo
                    };

                    lstPrestEsp.Add(prestamoEspecial);

                    if (prestamo.ClaveConceptoCaja == cveConceptos)
                        prestamosSeleccionada = prestamo;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _dialogService.ShowError("Ocurrió un error, favor de reportar a sistemas");
            }
        }

        async Task GetCallIdIdConceptoCaja(string claveConcepto)
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

                IdConceptoCaja = await _prestamoService.GetCallIdIdConceptoCaja(claveConcepto);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _dialogService.ShowError("Ocurrió un error, favor de reportar a sistemas");
            }
        }

        async Task InsertMovimiento(moMovimientosCaja movimiento, caSocio socio, List<ConceptoPromesa>? promesas)
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

                var IdPeriodo = await _userService.GetPeriodoVigente(movimiento.IdSocio);

                var movimientoCaja = new moMovimientosCaja
                {
                    IdSocio = movimiento.IdSocio,
                    IdPeriodo = Convert.ToInt32(IdPeriodo),
                    IdConceptoCaja = movimiento.IdConceptoCaja,
                    ImporteMovimiento = movimiento.ImporteMovimiento,
                    CantidadMovimiento = 0,
                    PorcentajeMovimiento = 0,
                    PlazoMovimiento = 0,
                    FechaAlta = DateTime.Today,
                    IdUsuarioAlta = movimiento.IdSocio,
                    FechaModificacion = DateTime.Today,
                    IdMovimiento = 0,
                    IdUsuarioModifica = 1,
                    PreliminarDefinitivo = "DE",
                    EstatusAutorizacion = "AL",
                    Estatus = true,
                    IdAval = movimiento.IdAval
                };

                string Folio = await _movimientosCajaService.InsertMovimiento(movimientoCaja);
                List<cNoSolicitud> cNoSol = JsonConvert.DeserializeObject<List<cNoSolicitud>>(Folio);

                string folio = cNoSol[0].NoSolicitud.ToString();
                bool error = false;

                if(promesas != null)
                {
                    foreach (ConceptoPromesa promesa in promesas)
                    {
                        var Item = await _promesasService.InsertPromesa(int.Parse(folio),
                            promesa.Cantidad, promesa.IdConceptoPromesa, socio.IdSocio, promesa.Anio);

                        if (!Item.Estado)
                        {
                            error = true;
                            break;
                        }
                    }
                }
               

                if (!error)
                {

                    NavegarFolioGeneradoRegistro?.Invoke(this, new FolioGeneradoRegistroArgs(folio, socio, true, false)); 
                   
                }
                else
                {
                    await _dialogService.ShowError(
                    "Ocurrió un error al registrar las promesas");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _dialogService.ShowError("Ocurrió un error, favor de reportar a sistemas");
            }
        }


        async Task GetEmpresas()
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

             
                int idCliente = 1;

                var Items = await _empresaService.GetEmpresas(idCliente);

                if (Items.Estado)
                {
                    foreach (var empresa in (List<caEmpresa>)Items.Resultado)
                    {
                        ListaEmpresas.Add(empresa);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _dialogService.ShowError("Ocurrió un error, favor de reportar a sistemas 1");
            }
            return;
        }



        async Task GetPrestamos(int idEmpresa, int? IdTipoEmpleado)
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

                var Items = await _prestamoService.GetListaPrestamos(idEmpresa,IdTipoEmpleado);

                if (Items.Estatus)
                {
                    foreach (var prestamo in (List<Prestamo>)Items.Resultado)
                    {
                        if (prestamo.Tipo == "P")
                            ListaPrestamos.Add(prestamo);
                    }
                }
                else
                    await _dialogService.ShowError("Ocurrió un error al consultar los datos");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _dialogService.ShowError("Ocurrió un error, favor de reportar a sistemas");
            }

            return;

        }



        private async Task ValidaAval()
        {
            if (EmpresaSeleccionada == null)
            {
                await _dialogService.ShowError("Debe seleccionar una empresa.");
                return;
            }

            if (string.IsNullOrWhiteSpace(NumeroEmpleadoAval))
            {
                await _dialogService.ShowError("Debe ingresar un número de empleado.");
                return;
            }

            var connection = await ConnectivityHelper.CheckInternetAsync();
            if (!connection.IsConnected)
            {
                await _dialogService.ShowError(connection.ErrorMessage);
                OnNavegarAcceso?.Invoke(this, EventArgs.Empty);
                return;
            }

            try
            {
                var resultado = await _prestamoService.ValidaAval(EmpresaSeleccionada.IdEmpresa, int.Parse(NumeroEmpleadoAval));

                if (resultado.Estado)
                {
                    var socio = ((List<caSocio>)resultado.Resultado).FirstOrDefault() ?? new caSocio();

                    if (socio.IdSocio == _socio.IdSocio)
                    {
                        await _dialogService.ShowError("El empleado ingresado no puede ser el que se encuentra usando la sesión.");
                        return;
                    }

                    if (socio.IdSocio == 0)
                    {
                        await _dialogService.ShowError("El empleado ingresado no existe.");
                        return;
                    }

                    if(_socio.IdEmpresa != socio.IdEmpresa)
                    {
                        await _dialogService.ShowError("El Aval debe pertenecer a la misma empresa del socio.");
                        return;
                    }

                    SocioAval = socio;
                    AceptarHabilitado = true;

                    await _dialogService.ShowMessage("Exito","Empleado válido, continúe con el proceso.");
                }
                else
                {
                    await _dialogService.ShowError("No se pudo validar el aval.");
                }
            }
            catch (Exception ex)
            {
                await _dialogService.ShowError($"Error al validar aval: {ex.Message}");
            }
        }



        private string ConvertirMonto(string montoRaw)
        {

            string limpio = montoRaw.Replace("$", "").Trim();


            string[] partes = limpio.Split('.');

            if (partes.Length > 1)
            {

                limpio = partes[0].Replace(",", "") + "." + partes[1];
            }
            else
            {

                limpio = partes[0].Replace(",", "");
            }

            return limpio;
        }


        async Task ValidaMovimiento()
        {
            try
            {
                var connection = await ConnectivityHelper.CheckInternetAsync();
                if (!connection.IsConnected)
                {
                    await _dialogService.ShowError(connection.ErrorMessage);
                    return;
                }

                Debug.WriteLine($"MontoSolicitado antes de validación: {MontoSolicitado}");

                var limpio = ConvertirMonto(MontoSolicitado);

                if (string.IsNullOrWhiteSpace(limpio) || !double.TryParse(limpio, out double monto) || monto <= 0)
                {
                    await _dialogService.ShowError("La cantidad del préstamo debe ser válida y mayor a 0 1 .");
                    return;
                }

                var resultado = await _expedienteSocioService.GetCalValidaMovimientos(_socio.IdSocio, monto, _prestamo.Clave.ToString());

                if (resultado.AplicaMovimiento)
                {
                    AceptarHabilitado = true;
                                              
                }
                else
                {
                    AceptarHabilitado = false;
                    await _dialogService.ShowError(resultado.MensajeAviso ?? "Movimiento no válido.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _dialogService.ShowError("Ocurrió un error al validar el movimiento. Favor de reportar a sistemas.");
            }
        }









        #endregion
        //tengo una region especial para promesas 
        #region Promesas 
        [ObservableProperty]
        private ObservableCollection<ConceptoPromesa> listaPromesas = new();

        [ObservableProperty]
        private ObservableCollection<ConceptoPromesa> listaPromesasPago = new();


        [ObservableProperty] private ConceptoPromesa promesaSeleccionada;
        [ObservableProperty] private string cantidadPromesaTexto;


        public IRelayCommand GetConceptosPromesasCommand { get; private set; }
        public IRelayCommand InsertPromesaCommand { get; private set; }

        public IRelayCommand AgregarPromesaCommand { get; private set; }


        public void InicializarConsultaPromesas(caSocio socio, int idConceptoCaja)
        {
            GetConceptosPromesasCommand = new AsyncRelayCommand(() => GetConceptosEmpresa(socio, idConceptoCaja));
        }

        public void InicializarInsertarPromesa(caSocio socio, List<ConceptoPromesa> promesas)
        {
            ListaPromesasPago.Clear();

            foreach (var promesa in promesas)
                ListaPromesasPago.Add(promesa);

            InsertPromesaCommand = new AsyncRelayCommand(() => InsertPromesa(socio, promesas));
        }

        public void InicializarAgregarPromesa()
        {
            AgregarPromesaCommand = new RelayCommand(async () => await AgregarPromesaSeleccionada());
        }


        async Task InsertPromesa(caSocio socio, List<ConceptoPromesa> promesas)
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


                bool error = false;

                foreach (ConceptoPromesa promesa in promesas)
                {
                    var Item = await _promesasService.InsertPromesa(0,
                        promesa.Cantidad, promesa.IdConceptoPromesa, socio.IdSocio, promesa.Anio);

                    if (!Item.Estado)
                    {
                        error = true;
                        break;
                    }
                }

                if (!error)
                {
                    NavegarFolioGeneradoRegistro?.Invoke(this, new FolioGeneradoRegistroArgs("", socio, true, false));

                    await _dialogService.ShowMessage("Éxito!",
                        "Las promesas se registraron de forma éxitosa");
                }
                else
                {
                    await _dialogService.ShowError(
                    "Ocurrió un error al registrar las promesas");
                }



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _dialogService.ShowError("Ocurrió un error, favor de reportar a sistemas");
            }

            return;
        }


        private async Task AgregarPromesaSeleccionada()
        {
            if (PromesaSeleccionada is null || string.IsNullOrWhiteSpace(CantidadPromesaTexto))
            {
                await _dialogService.ShowError("Debe seleccionar una promesa y capturar la cantidad.");
                return;
            }

            if (!decimal.TryParse(CantidadPromesaTexto, NumberStyles.Number, CultureInfo.InvariantCulture, out var cantidad) || cantidad <= 0)
            {
                await _dialogService.ShowError("Debe ingresar una cantidad válida.");
                return;
            }

           
            var limpio = LimpiarMonto(MontoSolicitado);
            if (!decimal.TryParse(limpio, NumberStyles.Number, CultureInfo.InvariantCulture, out var montoPrestamo) || montoPrestamo <= 0)
            {
                await _dialogService.ShowError("El monto del préstamo debe ser válido.");
                return;
            }

            var nuevaPromesa = new ConceptoPromesa
            {
                Mes = PromesaSeleccionada.Mes,
                Anio = PromesaSeleccionada.Anio,
                IdConceptoPromesa = PromesaSeleccionada.IdConceptoPromesa,
                Descripcion = PromesaSeleccionada.Descripcion,
                Cantidad = cantidad
            };

            bool existe = false;
            decimal suma = cantidad;

            foreach (var p in ListaPromesasPago)
            {
                if (!existe && p.IdConceptoPromesa == nuevaPromesa.IdConceptoPromesa && p.Anio == nuevaPromesa.Anio)
                    existe = true;

                suma += p.Cantidad;
            }

            if (existe)
            {
                await _dialogService.ShowError("El concepto de promesa ya existe.");
                return;
            }

            if (suma > montoPrestamo)
            {
                await _dialogService.ShowError("El monto total de promesas no puede superar el préstamo solicitado.");
                return;
            }

            ListaPromesasPago.Add(nuevaPromesa);

            
            PromesaSeleccionada = null;
            CantidadPromesaTexto = string.Empty;

            await _dialogService.ShowMessage("Éxito", "Promesa agregada con éxito.");
        }



        async Task GetConceptosEmpresa(caSocio socio, int idConceptoCaja)
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

                var Item = await _promesasService.GetConceptos(idConceptoCaja);

                if (Item.Estado)
                {
                    List<ConceptoPromesa> conceptoPromesas = (List<ConceptoPromesa>)Item.Resultado;
                    if (conceptoPromesas.Count > 0)
                    {
                        conceptoPromesas = conceptoPromesas.OrderBy(x => x.Mes).ToList();

                        int mes = DateTime.Now.Month;
                        int ano = DateTime.Now.Year;

                        string mesDos = "";
                        if (mes < 10)
                            mesDos = "0" + mes.ToString();
                        else
                            mesDos = mes.ToString();

                        DateTime fechaActual = DateTime.ParseExact(ano.ToString() + "-" + mesDos + "-" + "01"
                            , "yyyy-MM-dd", CultureInfo.InvariantCulture);

                        foreach (ConceptoPromesa promesa in conceptoPromesas)
                        {
                            string mesDosDigitos = "";
                            if (promesa.Mes < 10)
                                mesDosDigitos = "0" + promesa.Mes.ToString();
                            else
                                mesDosDigitos = promesa.Mes.ToString();

                            DateTime result = DateTime.ParseExact(promesa.Anio.ToString() + "-" + mesDosDigitos + "-" + "01",
                             "yyyy-MM-dd", CultureInfo.InvariantCulture);

                            int diferencia = Math.Abs((result.Month - fechaActual.Month) + 12 * (result.Year - fechaActual.Year));
                            promesa.Mes = diferencia + 1;

                        }

                        foreach (ConceptoPromesa promesa in conceptoPromesas)
                        {
                            promesa.Descripcion = promesa.Descripcion + " " + promesa.Anio.ToString();
                            ListaPromesas.Add(promesa);
                        }
                    }
                }
                else
                {

                  
                    OnNavegarMenuSocio?.Invoke(this, new SocioNavigationArgs(socio));

                    await _dialogService.ShowError(
                            "Ocurrió un error al consultar los datos");
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _dialogService.ShowError("Ocurrió un error, favor de reportar a sistemas");
            }

            return;
        }



        #endregion

    }
}
