using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Money_Box.Events;
using Money_Box.Helpers;
using Money_Box.IService;
using Money_Box.Models;
using Money_Box.Models.Local;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Money_Box.ViewModels
{
    public partial class ExpedienteSocioViewModel : ObservableObject
    {
        #region Servicios
        private readonly IExpedienteSocioService _expedienteService;
        private readonly IDialogService _dialogService;
        #endregion


        #region Navegacion
        public event EventHandler<SocioNavigationArgs> NavegarBeneficiarios;
        public event EventHandler<VentaAccionesNavigationArgs> NavegarVentaAcciones;
        public event EventHandler<FolioNavigationArgs> NavegarFolio;
        #endregion


        #region Comandos
        public IAsyncRelayCommand<caSocio> GetCalValidaImporteAccionesCommand { get; private set; }
        public IAsyncRelayCommand<VentaAccionesArgs> GetCalImporteMaximoVentaAccionesCommand { get; private set; }
        public IAsyncRelayCommand<FotoArgs> InsertaFotoCommand { get; private set; }
        #endregion
        


        public record VentaAccionesArgs(Prestamo Prestamo, caSocio Socio);
        public record FotoArgs(EnviarFoto Foto, caSocio Socio);

        #region Constructor
   
        public ExpedienteSocioViewModel(IExpedienteSocioService expedienteService, IDialogService dialogService)
        {
            _expedienteService = expedienteService;
            _dialogService = dialogService;
          
        }

        #endregion

        #region Inicializar Comandos
        public void InicializarComandos()
        {
            GetCalValidaImporteAccionesCommand = new AsyncRelayCommand<caSocio>(GetCalValidaImporteAcciones);
            GetCalImporteMaximoVentaAccionesCommand = new AsyncRelayCommand<VentaAccionesArgs>(args =>
                GetCalImporteMaximoVentaAcciones(args.Prestamo, args.Socio));
            InsertaFotoCommand = new AsyncRelayCommand<FotoArgs>(args =>
                InsertaFoto(args.Foto, args.Socio));
        }
        #endregion

        #region Funciones
        private async Task GetCalValidaImporteAcciones(caSocio socio)
        {
            try
            {
                var connection = await ConnectivityHelper.CheckInternetAsync();
                if (!connection.IsConnected)
                {
                    await _dialogService.ShowError(connection.ErrorMessage);
                    return;
                }

                var respuesta = await _expedienteService.GetCalValidaImporteAcciones(
                    socio.IdSocio,
                    socio.IdTipoEmpleado ?? 0,
                    socio.SueldoBrutoMensual?.ToString() ?? "0",
                    socio.ImporteDescuentoLP?.ToString() ?? "0");

                if (respuesta.AplicaMovimiento)
                    NavegarBeneficiarios?.Invoke(this, new SocioNavigationArgs(socio));
                else
                    await _dialogService.ShowError(respuesta.MensajeAviso);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _dialogService.ShowError("Error al validar importe de acciones.");
            }
        }

        private async Task GetCalImporteMaximoVentaAcciones(Prestamo prestamo, caSocio socio)
        {
            try
            {
                var connection = await ConnectivityHelper.CheckInternetAsync();
                if (!connection.IsConnected)
                {
                    await _dialogService.ShowError(connection.ErrorMessage);
                    return;
                }

                var respuesta = await _expedienteService.GetCalImporteMaximoVentaAcciones(socio.IdSocio, prestamo.Monto ?? 0);

                if (respuesta.AplicaMovimiento)
                {
                    var response = await _expedienteService.GetCalValidaMovimientos(socio.IdSocio, prestamo.Monto ?? 0, prestamo.Clave);

                    if (response.AplicaMovimiento)
                        NavegarVentaAcciones?.Invoke(this, new VentaAccionesNavigationArgs(socio, prestamo));
                    else
                        await _dialogService.ShowError(response.MensajeAviso);
                }
                else
                    await _dialogService.ShowError(respuesta.MensajeAviso);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _dialogService.ShowError("Ocurrió un error, favor de reportar a sistemas");
            }
        }

        private async Task<bool> InsertaFoto(EnviarFoto foto, caSocio socio)
        { 
            try
            {
                var connection = await ConnectivityHelper.CheckInternetAsync();
                if (!connection.IsConnected)
                {
                    await _dialogService.ShowError(connection.ErrorMessage);
                    return false;
                }

                var respuesta = await _expedienteService.InsertaFoto(foto);

                if (respuesta)
                {
                    await _dialogService.ShowMessage("Expediente", "Se agregó el comprobante");
                    NavegarFolio?.Invoke(this, new FolioNavigationArgs(foto.NoSolicitud.ToString(), socio, false, false));
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _dialogService.ShowError("Ocurrió un error, favor de reportar a sistemas");
                return false;
            }
            #endregion
            return false;
        }
    }
}
