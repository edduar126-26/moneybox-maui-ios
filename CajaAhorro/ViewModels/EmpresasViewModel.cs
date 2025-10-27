using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Networking;
using Money_Box.Helpers;
using Money_Box.IService;
using Money_Box.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace Money_Box.ViewModels
{
    public partial class EmpresasViewModel : ObservableObject
    {
        private readonly IEmpresaService _empresaService;
        private readonly ITiposEmpleadosService _tiposEmpleadoService;
        private readonly IDialogService _dialogService;

       

        [ObservableProperty]
        private ObservableCollection<caEmpresa> caEmpresas = new();

        [ObservableProperty]
        private ObservableCollection<caTiposEmpleado> caTiposEmpleados = new();

        [ObservableProperty]
        private caEmpresa empresaSeleccionada;

        [ObservableProperty]
        private caTiposEmpleado tiposEmpleadosSeleccionado;

        public IAsyncRelayCommand GetEmpresasCommand { get; private set; }
        public IAsyncRelayCommand GetTiposEmpleadosCommand { get; private set; }
        public IAsyncRelayCommand GetTodoTipoEmpleadoCommand { get; private set; }

        public caSocio _socio { get; }
        public EmpresasViewModel(
            IEmpresaService empresaService,
            ITiposEmpleadosService tiposEmpleadoService,
            IDialogService dialogService,
            caSocio socio)
        {
            _empresaService = empresaService;
            _tiposEmpleadoService = tiposEmpleadoService;
            _dialogService = dialogService;
            _socio = socio;
        }

        public void InicializarComandos()
        {
            GetEmpresasCommand = new AsyncRelayCommand(() => GetEmpresas(_socio?.NoEmpleado ?? 0));
            GetTiposEmpleadosCommand = new AsyncRelayCommand(() => GetTiposEmpleados(0));
            GetTodoTipoEmpleadoCommand = new AsyncRelayCommand(GetTodoTipoEmpleado);
        }

        private async Task GetEmpresas(int noEmpleado)
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

                if (Items.Estado && Items.Resultado is List<caSocio> socios && socios.Count > 0)
                {
                    CaEmpresas.Clear();
                    foreach (var s in socios)
                    {
                        CaEmpresas.Add(new caEmpresa
                        {
                            IdEmpresa = s.IdEmpresa,
                            NombreCorto = s.NombreCorto
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _dialogService.ShowError("Ocurrió un error al obtener empresas");
            }
        }

        private async Task GetTiposEmpleados(int idTipoEmpleado)
        {
            try
            {
                var connection = await ConnectivityHelper.CheckInternetAsync();
                if (!connection.IsConnected)
                {
                    await _dialogService.ShowError(connection.ErrorMessage);
                    return;
                }

                var Items = await _tiposEmpleadoService.GetTiposEmpleado();

                CaTiposEmpleados.Clear();

                foreach (var tipo in Items)
                {
                    CaTiposEmpleados.Add(tipo);
                    if (tipo.idTipoEmpleado == idTipoEmpleado)
                        TiposEmpleadosSeleccionado = tipo;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _dialogService.ShowError("Ocurrió un error al obtener tipos de empleado");
            }
        }

        private async Task GetTodoTipoEmpleado()
        {
            try
            {
                var connection = await ConnectivityHelper.CheckInternetAsync();
                if (!connection.IsConnected)
                {
                    await _dialogService.ShowError(connection.ErrorMessage);
                    return;
                }

                var Items = await _tiposEmpleadoService.GetTiposEmpleado();
                CaTiposEmpleados.Clear();

                foreach (var tipo in Items)
                {
                    CaTiposEmpleados.Add(tipo);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _dialogService.ShowError("Ocurrió un error al cargar todos los tipos de empleado");
            }
        }
    }
}
