using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Money_Box.Events;
using Money_Box.Helpers;
using Money_Box.IService;
using Money_Box.Models;
using Money_Box.Services;
using Money_Box.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Money_Box.ViewModels
{
    public partial class PromesasViewModel : ObservableObject
    {

        #region Servicios
        private readonly IPromesasService _promesasService;
        private readonly IDialogService _dialogService;
        public event EventHandler<FolioGeneradoRegistroArgs> OnFolioGeneradoRegistro;
        public event EventHandler<SocioNavigationArgs> OnNavegarMenuSocio;
        #endregion
        #region Commands
        public IRelayCommand GetConceptosPromesasCommand { get; private set; }
        public IRelayCommand InsertPromesaCommand { get; private set; }

        #endregion

        #region Variables

        [ObservableProperty]
        private ObservableCollection<ConceptoPromesa> listaPromesas;

        [ObservableProperty]
        private ObservableCollection<ConceptoPromesa> listaPromesasPago;

        #endregion

        #region Constructores

        public PromesasViewModel(IPromesasService promesasService, IDialogService dialogService)
        {
            _promesasService = promesasService;
            _dialogService = dialogService;
            ListaPromesas = new();
            ListaPromesasPago = new();
            
        }

        #endregion

        #region Inicializacion Comandos
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
        #endregion

        #region Funciones
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
                            promesa.Cantidad, promesa.IdConceptoPromesa , socio.IdSocio, promesa.Anio);

                        if (!Item.Estado)
                        {
                            error = true;
                            break;
                        }
                    }

                    if (!error)
                    {
                     
                        OnFolioGeneradoRegistro?.Invoke(this, new FolioGeneradoRegistroArgs("", socio, true, false));

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
                 
                        //esto para despues manejar el handler navegar a menusocio en la vista
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
