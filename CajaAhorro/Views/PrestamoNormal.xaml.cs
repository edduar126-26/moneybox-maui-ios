using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Internals;
using Money_Box.Models;
using Money_Box.Models.Local;
using Money_Box.ViewModels;
using Money_Box.Views;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace Money_Box.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrestamoNormal : ContentPage
    {
        caSocio socio;
        Prestamo prestamo;
        caSocio aval = new caSocio();
        private bool _isChanging = false;
        private string _rawMonto;
        List<ConceptoPromesa> ListaPromesas = new List<ConceptoPromesa>(); 

        private readonly PrestamosViewModel _viewModel;
        public PrestamoNormal(Prestamo Prestamo, caSocio Socio, List<ConceptoPromesa>? listaPromesas)
        {
            InitializeComponent();
            NombreSociolbl.Text = Socio.NoEmpleado + " - " + Socio.Nombre.Trim();
            NavigationPage.SetHasBackButton(this, false);

            socio = Socio;
            prestamo = Prestamo;
            ListaPromesas = listaPromesas;

         


            var factory = App.ServiceProvider.GetRequiredService<Func<caSocio,Prestamo, PrestamosViewModel>>();
            _viewModel = factory(socio,Prestamo);


            if (!prestamo.RequiereAval)
            {
                aval = new caSocio
                {
                    IdSocio = 0,
                };
                txtNoEmpleadoAval.IsVisible = false;
                lblAval.IsVisible = false;
                lblEmpresaAval.IsVisible = false;
                PckrEmpresas.IsVisible = false;
                ValidaAvalBtn.IsVisible = false;
                _viewModel.AceptarHabilitado = true;
               
            }

            _viewModel.MontoSolicitado = prestamo.Monto?.ToString("0.##") ?? "0";

         

            _viewModel.NavegarFolioGeneradoRegistro += (s, e) =>
              Navigation.PushAsync(new FolioGeneradoRegistroSocio(e.Folio, e.Socio, true, false));

            _viewModel.ConfigurarSegunPrestamo(prestamo);
            _viewModel.InicializarConsultaPromesas(socio, prestamo.IdConceptoCaja);
            _viewModel.GetConceptosPromesasCommand.Execute(null);
            _viewModel.InicializarAgregarPromesa();

            _viewModel.InicializarParaConsulta(prestamo);
            _viewModel.InicializarConsultaEmpresa();

            BindingContext = _viewModel;

         
            ConfigViewModel();
            GetEmpresas();

        }

        private async void ConfigViewModel()
        {
       
            await _viewModel.GetCalAhorroAcumuladoCommand.ExecuteAsync(null);
            await _viewModel.GetCalDeudaActualCommand.ExecuteAsync(null);
            await _viewModel.GetCalTasaInteresMensualCommand.ExecuteAsync(null);
            await _viewModel.GetCalImporteMaximoPrestamoAntiguedadCommand.ExecuteAsync(null);
            await _viewModel.GetCalImporteMaximoDisponiblePrestamoCommand.ExecuteAsync(null);
            await _viewModel.GetCalImportePrestamoActualSinReciprocidadCommand.ExecuteAsync(null);
            await _viewModel.GetCatPrestamosEspecialesCommand.ExecuteAsync(null);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is PrestamosViewModel vm)
            {
               
                vm.ConfigurarSegunPrestamo(prestamo);

                if (vm.MuestraPromesas)
                {
                   
                    if (vm.ListaPromesas == null || vm.ListaPromesas.Count == 0)
                    {
                        vm.InicializarConsultaPromesas(socio, prestamo.IdConceptoCaja);
                        vm.GetConceptosPromesasCommand.Execute(null);
                    }

                   
                    vm.InicializarAgregarPromesa();
                }
            }
        }


        private async void GetEmpresas()
        {
            await _viewModel.GetListaEmpresasCommand.ExecuteAsync(null);
        }

        private async void RegresarBtn_Clicked(object sender, EventArgs e)
        {
            await ((NavigationPage)Parent).PushAsync(new MovimientosPrestamos(socio));
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

        private async void TablaAmortizacionBtn_Clicked(object sender, EventArgs e)
        {
          
            _isChanging = true;

           
            string raw = MontoPrestamoAsolicitartxt.Text ?? "";
            Debug.WriteLine($"Valor de MontoPrestamoAsolicitartxt antes de formatear: {raw}"); 

          
            string limpio = ConvertirMonto(raw);
            Debug.WriteLine($"Valor limpio después de convertir: {limpio}");

         
            if (!decimal.TryParse(limpio, NumberStyles.Number, CultureInfo.InvariantCulture, out var monto) || monto <= 0)
            {
                await DisplayAlert("Aviso", "La cantidad del préstamo debe ser mayor a 0", "OK");
                _isChanging = false; 
                return;
            }

        
            ApplyCurrencyFormat();
            _isChanging = false;

       
            string tasaRaw = (TasaInteresMensualtxt.Text ?? "").Replace("%", "").Trim();
            string tasaNorm = new string(tasaRaw.Where(c => char.IsDigit(c) || c == '.' || c == ',').ToArray())
                                 .Replace(",", ".");
            if (!double.TryParse(tasaNorm, NumberStyles.Number, CultureInfo.InvariantCulture, out var tasaMensual))
            {
                await DisplayAlert("Aviso", "No se pudo leer la tasa de interés mensual.", "OK");
                return;
            }

            var prestamos = new Prestamo
            {
                IdConceptoCaja = prestamo.IdConceptoCaja,
                Monto = (double)monto, 
                Clave = prestamo.Clave,
                Concepto = prestamo.Concepto,
                RequiereAval = prestamo.RequiereAval,
                IdSocio = socio.IdSocio,
                PlazoPrestamo = prestamo.PlazoPrestamo,
                TasaInteresPrestamo = tasaMensual,
                RequierePromesa = prestamo.RequierePromesa
            };

            await ((NavigationPage)Parent).PushAsync(
                new TablaAmortizacionPrestamo(prestamos, socio, ListaPromesas));
        }


        protected override bool OnBackButtonPressed()
        {
            ((NavigationPage)Parent).PushAsync(new MovimientosPrestamos(socio));
            return true;
        }


        string Strip(string s)
        {
            if (s == null) return "";
           
            var only = new string(s.Where(c => char.IsDigit(c) || c == '.' || c == ',').ToArray());
        
            return only.Replace(',', '.');
        }
        void Monto_Focused(object sender, FocusEventArgs e)
        {
            if (_isChanging) return;
            _isChanging = true;
            _rawMonto = Strip(MontoPrestamoAsolicitartxt.Text);
            MontoPrestamoAsolicitartxt.Text = string.IsNullOrEmpty(_rawMonto)
                ? ""
                : decimal.Parse(_rawMonto, CultureInfo.InvariantCulture).ToString("0.##", CultureInfo.InvariantCulture);
            MontoPrestamoAsolicitartxt.CursorPosition = MontoPrestamoAsolicitartxt.Text?.Length ?? 0;
            _isChanging = false;
        }



        void ApplyCurrencyFormat()
        {
            if (_isChanging) return;

            _isChanging = true;

     
            if (decimal.TryParse(MontoPrestamoAsolicitartxt.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out var valor))
            {
                MontoPrestamoAsolicitartxt.Text = valor.ToString("C", CultureInfo.GetCultureInfo("es-MX"));
            }
            else
            {
                MontoPrestamoAsolicitartxt.Text = "";
            }

            _isChanging = false;
        }

        void Monto_FormatOnUnfocused(object sender, FocusEventArgs e)
        {
            if (decimal.TryParse(MontoPrestamoAsolicitartxt.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out var monto))
            {
                ApplyCurrencyFormat();
            }


            _viewModel.CalcularDescuentoCommand.Execute(MontoPrestamoAsolicitartxt.Text);
        }
        void Monto_FormatOnCompleted(object sender, EventArgs e)
        {
            if (decimal.TryParse(MontoPrestamoAsolicitartxt.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out var monto))
            {
                ApplyCurrencyFormat();
            }
            _viewModel.CalcularDescuentoCommand.Execute(MontoPrestamoAsolicitartxt.Text);
        }

        //Este bnt no reaciona AceptarBtn_Clicked
        private async void AceptarBtn_Clicked(object sender, EventArgs e)
        {
            _isChanging = true;

            string raw = MontoPrestamoAsolicitartxt.Text ?? "";
            Debug.WriteLine($"Valor de MontoPrestamoAsolicitartxt antes de formatear: {raw}");

            string limpio = ConvertirMonto(raw);
            Debug.WriteLine($"Valor limpio después de convertir: {limpio}");

            if (!decimal.TryParse(limpio, NumberStyles.Number, CultureInfo.InvariantCulture, out var monto) || monto <= 0)
            {
                await DisplayAlert("Aviso", "La cantidad del préstamo debe ser mayor a 0", "OK");
                _isChanging = false;
                return;
            }

            ApplyCurrencyFormat();
            _isChanging = false;

            var idAval = prestamo.RequiereAval
            ? (_viewModel.SocioAval?.IdSocio ?? 0)
            : 0;

            var movimiento = new moMovimientosCaja
            {
                IdSocio = socio.IdSocio,
                ImporteMovimiento = monto,
                IdConceptoCaja = prestamo.IdConceptoCaja,
                IdAval = idAval
            };

            try
            {


                var promesasParaInsertar = (_viewModel.MuestraPromesas && _viewModel.ListaPromesasPago?.Count > 0)
                ? new System.Collections.Generic.List<ConceptoPromesa>(_viewModel.ListaPromesasPago)
                : null;


                _viewModel.InicializarValidacionMoviemiento();

           
                await _viewModel.ValidaMovimientoCommand.ExecuteAsync(null);

             
                if (_viewModel.AceptarHabilitado) 
                {
                    _viewModel.InicializarParaInsertar(movimiento, socio, promesasParaInsertar);
                    await _viewModel.InsertMovimientoCommand.ExecuteAsync(null);

                 
                    
                }
                else
                {
                    await DisplayAlert("Aviso", "El movimiento no es válido. No se puede completar la solicitud.", "OK");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                await DisplayAlert("Error", "No se pudo completar la solicitud.", "OK");
                return;
            }
        }


        private async void ValidaAvalBtn_Clicked(object sender, EventArgs e)
        {
            _viewModel.InicializarValidaAval();
            await _viewModel.ValidaAvalCommand.ExecuteAsync(null);
        }

    
    }
}