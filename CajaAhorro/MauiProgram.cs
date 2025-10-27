using CommunityToolkit.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Money_Box.Data;
using Money_Box.IService;
using Money_Box.Models;
using Money_Box.Models.Local;
using Money_Box.Platforms;
using Money_Box.Services;
using Money_Box.ViewModels;
using Money_Box.Views;
using SkiaSharp.Views.Maui.Controls.Hosting;
using SQLitePCL;

namespace Money_Box
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            SQLitePCL.Batteries_V2.Init();
            var builder = MauiApp.CreateBuilder();


      
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                })
                .UseMauiCommunityToolkit()
                .UseSkiaSharp();

         



            // Servicios HTTP / navegación / utilidades
            builder.Services.AddSingleton<INavigationService, NavigationService>();
            builder.Services.AddHttpClient<IBancosService, BancosService>();
            builder.Services.AddHttpClient<ICamaraService, CamaraService>();
            builder.Services.AddHttpClient<IClienteService, ClienteService>();
            builder.Services.AddHttpClient<IEmpresaService, EmpresaService>();
            builder.Services.AddHttpClient<IEstadoCuentaService, EstadoCuentaService>();
            builder.Services.AddHttpClient<IExpedienteSocioService, ExpedienteSocioService>();
            builder.Services.AddHttpClient<ILogInService, LogInService>();
            builder.Services.AddHttpClient<IModificacionAhorroService, ModificacionAhorroService>();
            builder.Services.AddHttpClient<IMovimientosCajaService, MovimientosCajaService>();
            builder.Services.AddHttpClient<IPrestamosService, PrestamosService>();
            builder.Services.AddHttpClient<IPromesasService, PromesasService>();
            builder.Services.AddHttpClient<ISocioService, SocioService>();
            builder.Services.AddHttpClient<ITablaAmortizacionService, TablaAmortizacionService>();
            builder.Services.AddHttpClient<ITiposEmpleadosService, TiposEmpleadosService>();
            builder.Services.AddHttpClient<IUserService, UserService>();
            builder.Services.AddHttpClient<IVentaAccionesService, VentaAccionesService>();
            builder.Services.AddTransient<IServicioFoto, ServicioFotoService>();
            builder.Services.AddSingleton<IDialogService, DialogService>();
            builder.Services.AddSingleton<IFilePickerService, FilePickerService>();
            builder.Services.AddSingleton<INotificacionPreferenceStorage, NotificacionPreferenceStorage>();
            builder.Services.AddSingleton<IDocumentosService, DocumentosService>();

            // ViewModels
            builder.Services.AddTransient<AccesoViewModel>();
            builder.Services.AddTransient<BajaCajaViewModel>();
            builder.Services.AddTransient<BancosViewModel>();
            builder.Services.AddTransient<ClientesViewModel>();
            builder.Services.AddTransient<EmpresasViewModel>();
            builder.Services.AddTransient<EstadoCuentaViewModel>();
            builder.Services.AddTransient<ExpedienteSocioViewModel>();
            builder.Services.AddTransient<FirmaContratoNormativoViewModel>();
            builder.Services.AddTransient<FirmarPagareSocioViewModel>();
            builder.Services.AddTransient<FirmarPagareViewModel>();
            builder.Services.AddTransient<InsertUserViewModel>();
            builder.Services.AddTransient<ModificacionAhorroViewModel>();
            builder.Services.AddTransient<MovimientosPrestamosViewModel>();
            builder.Services.AddTransient<NotificacionesSQLiteViewModel>();
            builder.Services.AddTransient<PrestamosViewModel>();
            builder.Services.AddTransient<PromesasViewModel>();
            builder.Services.AddTransient<SocioViewModel>();
            builder.Services.AddTransient<TablaAmortizacionPrestamoViewModel>();
            builder.Services.AddTransient<UsersViewModel>();
            builder.Services.AddTransient<VentaAccionesViewModel>();

            // Factories
            builder.Services.AddTransient<Func<caSocio?, UsersViewModel>>(sp => socio =>
                new UsersViewModel(
                    sp.GetRequiredService<ILogInService>(),
                    sp.GetRequiredService<IEmpresaService>(),
                    sp.GetRequiredService<IUserService>(),
                    sp.GetRequiredService<IDialogService>(),
                    socio));

            builder.Services.AddTransient<Func<NotificacionesSQLiteViewModel>>(sp =>
                () => new NotificacionesSQLiteViewModel(
                    sp.GetRequiredService<IAppDatabase>(),
                    sp.GetRequiredService<IUserService>()));

            builder.Services.AddTransient<Func<caSocio, Prestamo, PrestamosViewModel>>(sp => (socio, prestamo) =>
                new PrestamosViewModel(
                    sp.GetRequiredService<IPrestamosService>(),
                    sp.GetRequiredService<IUserService>(),
                    sp.GetRequiredService<IMovimientosCajaService>(),
                    sp.GetRequiredService<IPromesasService>(),
                    sp.GetRequiredService<IDialogService>(),
                    sp.GetRequiredService<IClienteService>(),
                    sp.GetRequiredService<IExpedienteSocioService>(),
                    sp.GetRequiredService<IEmpresaService>(),
                    socio, prestamo));

            builder.Services.AddTransient<Func<ExpedienteSocioViewModel>>(sp => () =>
                new ExpedienteSocioViewModel(
                    sp.GetRequiredService<IExpedienteSocioService>(),
                    sp.GetRequiredService<IDialogService>()));

            builder.Services.AddTransient<Func<caSocio, moMovimientosCaja, BajaCajaViewModel>>(sp => (socio, movimiento) =>
                new BajaCajaViewModel(
                    sp.GetRequiredService<IExpedienteSocioService>(),
                    sp.GetRequiredService<IUserService>(),
                    sp.GetRequiredService<IMovimientosCajaService>(),
                    sp.GetRequiredService<IPrestamosService>(),
                    sp.GetRequiredService<IDialogService>(),
                    socio, movimiento));

            // Pages con dependencias
            builder.Services.AddTransient<Acceso>();
            builder.Services.AddTransient<Acceso>(sp =>
                new Acceso(sp.GetRequiredService<Func<caSocio?, UsersViewModel>>()));

            builder.Services.AddTransient<SocioRegistroDocumentos>();
            builder.Services.AddTransient<Func<caSocio, string?, SocioRegistroDocumentos>>(sp => (socio, folio) =>
                new SocioRegistroDocumentos(socio, folio, sp.GetRequiredService<IFilePickerService>()));

            builder.Services.AddTransient<DocumentosSocioActualizacion>();
            builder.Services.AddTransient<Func<caSocio, DocumentosSocioActualizacion>>(sp => socio =>
                new DocumentosSocioActualizacion(socio, sp.GetRequiredService<IFilePickerService>()));

            builder.Services.AddTransient<Func<RegistroSocioDto, InsertUserViewModel>>(sp => dto =>
                new InsertUserViewModel(dto,
                    sp.GetRequiredService<IUserService>(),
                    sp.GetRequiredService<IDialogService>()));

            builder.Services.AddTransient<Func<caSocio, EmpresasViewModel>>(sp => socio =>
                new EmpresasViewModel(
                    sp.GetRequiredService<IEmpresaService>(),
                    sp.GetRequiredService<ITiposEmpleadosService>(),
                    sp.GetRequiredService<IDialogService>(),
                    socio));

            builder.Services.AddTransient<Func<caSocio, moMovimientosCaja?, ModificacionAhorroViewModel>>(sp => (socio, mov) =>
                new ModificacionAhorroViewModel(
                    sp.GetRequiredService<IModificacionAhorroService>(),
                    sp.GetRequiredService<IPrestamosService>(),
                    sp.GetRequiredService<IUserService>(),
                    sp.GetRequiredService<IDialogService>(),
                    sp.GetRequiredService<IMovimientosCajaService>(),
                    socio, mov));

            builder.Services.AddTransient<DocumentosSocioActualizacionViewModel>();

            builder.Services.AddTransient<Func<int, FirmarPagareSocioViewModel>>(sp => folio =>
                new FirmarPagareSocioViewModel(
                    folio,
                    sp.GetRequiredService<IUserService>(),
                    sp.GetRequiredService<IDialogService>()));

            builder.Services.AddTransient<Func<caSocio, moMovimientosCaja?, VentaAccionesViewModel>>(sp => (socio, mov) =>
                new VentaAccionesViewModel(
                    sp.GetRequiredService<IVentaAccionesService>(),
                    sp.GetRequiredService<IMovimientosCajaService>(),
                    sp.GetRequiredService<IUserService>(),
                    sp.GetRequiredService<IPrestamosService>(),
                    sp.GetRequiredService<IDialogService>(),
                    socio, mov));

            builder.Services.AddTransient<Func<Prestamo, caSocio, List<ConceptoPromesa>, TablaAmortizacionPrestamoViewModel>>(sp =>
                (prestamo, socio, conceptos) =>
                    new TablaAmortizacionPrestamoViewModel(
                        sp.GetRequiredService<ITablaAmortizacionService>(),
                        sp.GetRequiredService<IDialogService>(),
                        prestamo, socio, conceptos));

            builder.Services.AddTransient<Func<caSocio, SocioViewModel>>(sp => socio =>
                new SocioViewModel(
                    sp.GetRequiredService<ISocioService>(),
                    sp.GetRequiredService<IDialogService>(),
                    socio));

            builder.Services.AddTransient<Func<int, FirmaContratoNormativoViewModel>>(sp => idUsuario =>
                new FirmaContratoNormativoViewModel(
                    idUsuario,
                    sp.GetRequiredService<IUserService>(),
                    sp.GetRequiredService<IDialogService>()));


            

            // Base de datos SQLite (con ruta local)
            builder.Services.AddSingleton<IDatabasePathProvider, DatabasePathProvider>();
            builder.Services.AddSingleton<IAppDatabase>(sp =>
            {
                var pathProvider = sp.GetRequiredService<IDatabasePathProvider>();
                var dbPath = pathProvider.GetLocalFilePath("MoneyBox.db3");
                return new AppDatabase(dbPath);
            });

            var app = builder.Build();


            
            Money_Box.App.ServiceProvider = app.Services;

            return app;
        }
    }
}
