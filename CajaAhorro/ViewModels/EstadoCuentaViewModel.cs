using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Money_Box.IService;
using Money_Box.Models;
using Money_Box.Services;
using Money_Box.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;
using Money_Box.Helpers;

namespace Money_Box.ViewModels
{
    public partial class EstadoCuentaViewModel : ObservableObject
    {
        #region Commands

        public IAsyncRelayCommand GetTotalesPrestamosVigentesSocioCommand { get; private set; }
        public IAsyncRelayCommand GetDetallePrestamoVigenteSocioCommand { get; private set; }
        public IAsyncRelayCommand GetPromesasXPrestamoCommand { get; private set; }

        #endregion

        #region Variables

        [ObservableProperty]
        private ObservableCollection<DetallePrestamoVigente> detallePrestamoVigente;

        [ObservableProperty]
        private string ahorroAcumulado;

        [ObservableProperty]
        private string cantidadPrestamosActivos;

        [ObservableProperty]
        private string montoTotalPrestamo;

        [ObservableProperty]
        private string saldoTotalPendiente;

        [ObservableProperty]
        private ObservableCollection<ConceptoPromesa> promesas;

        [ObservableProperty]
        private string totalDescuentosPrestamo;
        #endregion

        #region Constructores
     
        private IEstadoCuentaService _estadoCuentaService;
        private IPromesasService _promesasService;
        private IDialogService _dialogoService;
        public EstadoCuentaViewModel(
              IEstadoCuentaService estadoCuentaService,
              IPromesasService promesasService,
              IDialogService dialogoService)
        {
            _estadoCuentaService = estadoCuentaService;
            _promesasService = promesasService;
            _dialogoService = dialogoService;

            DetallePrestamoVigente = new();
            Promesas = new();
           
        }


        public void InicializarParaConsultaTotales(int idSocio)
        {
            GetTotalesPrestamosVigentesSocioCommand = new AsyncRelayCommand(() => GetTotalesPrestamosVigentesSocio(idSocio));
            GetDetallePrestamoVigenteSocioCommand = new AsyncRelayCommand(() => GetDetallePrestamoVigenteSocio(idSocio));
        }

        public void InicializarParaConsultaPromesas(int idSocio, long? idMovimiento = null)
        {
            if(idMovimiento.HasValue)
               GetPromesasXPrestamoCommand = new AsyncRelayCommand(() => GetPromesasXPrestamo(idSocio, idMovimiento.Value));
        }

        #endregion

        #region Funciones
        async Task GetTotalesPrestamosVigentesSocio(int idSocio)
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

                    List<PrestamoVigenteSocio> item = await _estadoCuentaService 
                        .GetTotalesPrestamosVigentesSocio(idSocio);

                    if (item.Count > 0)
                    {
                        AhorroAcumulado = Convert.ToDecimal(item[0].AhorroAcumulado).ToString("C2", new CultureInfo("es-MX"));
                        CantidadPrestamosActivos = item[0].CantidadPrestamosActivos.ToString(CultureInfo.CurrentCulture);
                        MontoTotalPrestamo = item[0].MontoTotalPrestamo.ToString("C2", new CultureInfo("es-MX"));
                        SaldoTotalPendiente = item[0].SaldoTotalPendiente.ToString("C2", new CultureInfo("es-MX"));
                        TotalDescuentosPrestamo = item[0].TotalDescuentosPrestamo.ToString("C2", new CultureInfo("es-MX"));

                    }
                    else
                        await _dialogoService.ShowError("Ocurrió un error al consultar los datos");
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _dialogoService.ShowError("Ocurrió un error, favor de reportar a sistemas");
            }


            return;
        }

        async Task GetDetallePrestamoVigenteSocio(int idSocio)
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


                    List<DetallePrestamoVigente> items =
                        await _estadoCuentaService.GetDetallePrestamoVigenteSocio(idSocio);


                    for (int i = 0; i < items.Count; i++)
                    {
                        DetallePrestamoVigente prestamoVigente = new DetallePrestamoVigente
                        {
                            IdSocio = items[i].IdSocio,
                            NoEmpleado = items[i].NoEmpleado,
                            Nombre = items[i].Nombre,
                            IdMovimiento = items[i].IdMovimiento,
                            FolioSolicitud = items[i].FolioSolicitud,
                            TipoPrestamo = items[i].TipoPrestamo,
                            FechaPrestamo = Convert.ToDateTime(items[i].FechaPrestamo.Substring(0, 10)).ToString("dd/MM/yyyy"),
                            TasaPrestamo = items[i].TasaPrestamo,
                            ImportePrestamo = string.Format("{0:C2}", decimal.Parse(items[i].ImportePrestamo)),
                            ImporteDescuento = string.Format("{0:C2}", decimal.Parse(items[i].ImporteDescuento)),
                            SaldoPrestamo = string.Format("{0:C2}", decimal.Parse(items[i].SaldoPrestamo))
                        };

                        DetallePrestamoVigente.Add(prestamoVigente);
                    }

               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _dialogoService.ShowError("Ocurrió un error, favor de reportar a sistemas");
            }

            return;
        }

        async Task GetPromesasXPrestamo(int idSocio, long idMovimiento)
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

                    EntRespuesta respuesta =
                        await _promesasService.GetPromesasXPrestamo(idMovimiento); 

                    if (respuesta.Estado)
                    {
                        List<ConceptoPromesa> items = (List<ConceptoPromesa>)respuesta.Resultado;

                        foreach(ConceptoPromesa promesaPago in items)
                        {
                            promesaPago.Descripcion = promesaPago.Descripcion + " " + promesaPago.Anio.ToString();
                            Promesas.Add(promesaPago);
                        }
                    }

                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _dialogoService.ShowError("Ocurrió un error, favor de reportar a sistemas");
            }

            return;
        }

        #endregion

    }
}

