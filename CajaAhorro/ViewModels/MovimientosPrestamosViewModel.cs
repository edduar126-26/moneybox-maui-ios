using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Money_Box.Models;
using Money_Box.Services;
using Money_Box.IService;
using Money_Box.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Money_Box.Data;
using Money_Box.Events;
using Money_Box.Helpers;


namespace Money_Box.ViewModels
{
    public partial class MovimientosPrestamosViewModel : ObservableObject
    {

        #region Servicios
        private readonly IPrestamosService _prestamoService;
        private readonly IEmpresaService _empresaService;
        private readonly IClienteService _clienteService;
        private readonly IDialogService _dialogService;
        public event EventHandler<PrestamoNormalArgs> OnNavegarPrestamoNormal;
        public event EventHandler OnNavegarAcceso; 
        #endregion

        #region Commands
        public IRelayCommand GetTipoPrestamoCommand { get; private set; }
        public IRelayCommand GetListaEmpresasCommand { get; private set; }
        public IRelayCommand ValidaMovimientoDuplicadoCommand { get; private set; }

        #endregion

        #region Variables
        [ObservableProperty]
        private ObservableCollection<Prestamo> listaPrestamos;

      
        [ObservableProperty]
        private ObservableCollection<caEmpresa> listaEmpresas;

        #endregion

        #region Constructores

        public MovimientosPrestamosViewModel(IClienteService clienteService,
            IEmpresaService empresaService,
            IPrestamosService prestamosServices,
            IDialogService dialogService)
        {
            ListaEmpresas = new();
            ListaPrestamos = new();
            _clienteService = clienteService;
            _empresaService = empresaService;
            _prestamoService = prestamosServices;
            _dialogService = dialogService;
        }

        #endregion

        #region InicializarCommandos
        public void InicializarConsultaPrestamos(caSocio socio)
        {
            GetTipoPrestamoCommand = new AsyncRelayCommand(() => GetPrestamos(socio.IdEmpresa, socio.IdTipoEmpleado));
        }

        public void InicializarConsultaValidaMovimientos(int idPrestamo, caSocio socio)
        {
            ValidaMovimientoDuplicadoCommand = new AsyncRelayCommand(() => ValidaMovimiento(idPrestamo, socio));
        }

        public void InicializarConsultaEmpresa()
        {
            GetListaEmpresasCommand = new AsyncRelayCommand(() => GetEmpresas());
        }
        #endregion

        #region Funciones
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

                var Items = await _prestamoService.GetListaPrestamos(idEmpresa, IdTipoEmpleado);

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

        async Task ValidaMovimiento(int idPrestamo, caSocio socio)
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

                    var Items = await _prestamoService.ValidaMovimiento(socio.IdSocio, idPrestamo);

                    if (!Items.Estatus) 
                    {
                        var datosPrestamo = await _prestamoService.GetDatosPrestamo(idPrestamo); 

                        if (datosPrestamo.Estado)
                        {
                            Prestamo datos = (Prestamo)datosPrestamo.Resultado;
                            datos.Monto = 0;
                            datos.IdSocio = socio.IdSocio;

                            //Handler a PrestamoNormal navegacion
                            OnNavegarPrestamoNormal?.Invoke(this,new PrestamoNormalArgs(datos, socio, new List<ConceptoPromesa>()));
                        }
                    }
                    else
                        await _dialogService.ShowError(
                            Items.Mensaje);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return;
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

                var Clientes = new AppDatabase(DependencyService.Get<IDatabasePathProvider>().GetLocalFilePath("MoneyBox.db3"));
                    var cliente = await _clienteService.GetClientes();
                   //int idCliente = cliente[0].IdCliente;
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
                await _dialogService.ShowError("Ocurrió un error, favor de reportar a sistemas");
            }
            return;
        }

        public async Task<caSocio> ValidaAval(int idEmpresa, int noEmpleado)
        {
            try
            {
                var connection = await ConnectivityHelper.CheckInternetAsync();
                if (!connection.IsConnected)
                    if (!connection.IsConnected)
                    {
                        await _dialogService.ShowError(connection.ErrorMessage);
                        OnNavegarAcceso?.Invoke(this, EventArgs.Empty);
                       
                    }

                    var Items = await _prestamoService.ValidaAval(idEmpresa, noEmpleado);

                    if (Items.Estado)
                    {
                        var socio = (List<caSocio>)Items.Resultado;
                        return socio.FirstOrDefault() ?? new caSocio();
                    }
                
         
            }
            catch (Exception ex)
            {
                await _dialogService.ShowError(ex.Message);
            }

            return new caSocio();
        }

        #endregion

    }
}
