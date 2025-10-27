using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Money_Box.Helpers;
using Money_Box.IService;
using Money_Box.Models;
using Money_Box.Services;
using Money_Box.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Money_Box.ViewModels
{
    public partial class TablaAmortizacionPrestamoViewModel : ObservableObject
    {
        #region Servicios
        private readonly ITablaAmortizacionService _tablaAmortizacionService;
        private readonly IDialogService _dialogService;

        #endregion

        #region Commands
        public IRelayCommand GetTablaAmortizacionPrestamoCommand { get;private set; }

        #endregion


        #region VariablesObservables

        [ObservableProperty]
        private ObservableCollection<caTablaAmortizacionPrestamo> tablaAmortizacionPrestamo = new ObservableCollection<caTablaAmortizacionPrestamo>();

        [ObservableProperty]
        private string nombreSocio;

        [ObservableProperty]
        private string descripcionPrestamo;

        [ObservableProperty]
        private string tasaInteresTexto;

        [ObservableProperty]
        private string sTotalSaldoInsoluto;
  

        [ObservableProperty]
        private string pagoMensualCapital;
   

        [ObservableProperty]
        private string sTotalPagoMensual;


        [ObservableProperty]
        private string sTotalCapital;
   

        [ObservableProperty]
        private string sTotalExtraordinario;
    

        [ObservableProperty]
        private string sTotalIntereses;



        [ObservableProperty]
        private string sTotalIva;


        [ObservableProperty]
        private string sTotalPagos;
        #endregion


        #region Constructores
        public Prestamo Prestamo { get; }
        public caSocio Socio { get; }
        public List<ConceptoPromesa> ConceptoPromesas { get; }

        public TablaAmortizacionPrestamoViewModel(
          ITablaAmortizacionService tablaAmortizacionService,
          IDialogService dialogService,
          Prestamo prestamo,
          caSocio socio,
          List<ConceptoPromesa> conceptoPromesas)
            {
                _tablaAmortizacionService = tablaAmortizacionService;
                _dialogService = dialogService;

                Prestamo = prestamo;
                Socio = socio;
                ConceptoPromesas = conceptoPromesas;

                // Asignas las propiedades observables:
                NombreSocio = $"{socio.NoEmpleado} - {socio.Nombre.Trim()}";
                DescripcionPrestamo = prestamo.Concepto?.Trim();
                TasaInteresTexto = $"Tasa de Interés: {prestamo.TasaInteresPrestamo?.ToString("F2") ?? "0"}%";

                TablaAmortizacionPrestamo = new();
            }

        #endregion


        #region Inicializacion Commandos
        public void InicializaConsultaTablaPrestamo()
        {
            List<Models.PromesasPago> promesas = new();

            foreach (var promesa in ConceptoPromesas)
            {
                promesas.Add(new Models.PromesasPago
                {
                    Cantidad = promesa.Cantidad,
                    Mes = promesa.Mes
                });
            }

            GetTablaAmortizacionPrestamoCommand = new AsyncRelayCommand(() => GetTablaAmortizacionPrestamo(Prestamo, Socio, promesas));
        }
        #endregion


        #region Functions
        async Task GetTablaAmortizacionPrestamo(Prestamo prestamo, caSocio socio, List<Models.PromesasPago> promesas)
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

                EntRespuesta respuesta = await _tablaAmortizacionService.GetTablaAmortizacionPrestamo(socio.IdSocio,
                        prestamo.Clave, prestamo.Monto.Value.ToString(), promesas);

                if (respuesta.Estado)
                {
                    var mx = new CultureInfo("es-MX");

                    var items = (List<caTablaAmortizacionPrestamo>)respuesta.Resultado;


                    var totales = items.FirstOrDefault(x => x.NoRenglon == 9999);


                    var detalle = items.Where(x => x.NoRenglon != 9999).ToList();


                    TablaAmortizacionPrestamo.Clear();

                    foreach (var r in detalle)
                    {


                        try
                        {
                            
                            var fechaPago = DateTime.ParseExact(r.FechaPago, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
                            Debug.WriteLine($"Fecha convertida correctamente: {fechaPago}");

                            TablaAmortizacionPrestamo.Add(new caTablaAmortizacionPrestamo
                            {
                                NoRenglon = r.NoRenglon,
                                FechaPago = fechaPago,
                                SaldoInsoluto = r.SaldoInsoluto,
                                PagoMensual = r.PagoMensual,
                                Capital = r.Capital,
                                Intereses = r.Intereses,
                                IVA = r.IVA,
                                AmortizacionExtraordinaria = r.AmortizacionExtraordinaria,
                            });
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Error al procesar el NoRenglon {r.NoRenglon}: {ex.Message}");
                        }
                    }



                    if (totales != null) 
                    {
                        STotalPagos = $"Total de pagos: {detalle.Count - 1 :n0}";
                        STotalPagoMensual = totales.PagoMensual.ToString("C", mx);
                        STotalIntereses = totales.Intereses.ToString("C", mx);
                        STotalIva = totales.IVA.ToString("C", mx);
                        STotalExtraordinario = 0m.ToString("C", mx);
                        STotalCapital = "Monto del Préstamo: " + prestamo.Monto.Value.ToString("C", mx);
                    }
                    else
                    {
                        STotalPagoMensual = 0m.ToString("C", mx);
                        STotalIntereses = 0m.ToString("C", mx);
                        STotalIva = 0m.ToString("C", mx);
                        STotalExtraordinario = 0m.ToString("C", mx);
                        STotalCapital = "Monto del Préstamo: " + prestamo.Monto.Value.ToString("C", mx);

                    }
                }
                else
                {
                    await _dialogService.ShowError("Ocurrió un error, favor de reportar a sistemas 1");
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _dialogService.ShowError("Ocurrió un error, favor de reportar a sistemas 2");
            }

            return;
        }

        #endregion
    }
}

