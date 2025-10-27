
using Microsoft.Maui.Controls;
using Money_Box.Views;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
namespace Money_Box
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; set; }
        public App(Acceso mainPage, IServiceProvider serviceProvider)
        {

            InitializeComponent();
            MainPage = new NavigationPage(mainPage);
            ServiceProvider = serviceProvider;
            WeakReferenceMessenger.Default.Register<DeepLinkMessage>(this, async (r, msg) =>
            {
                if (!int.TryParse(msg.Folio, out var id) || id <= 0)
                {
                    await MainPage.DisplayAlert("Enlace inválido", "El folio no es numérico.", "OK");
                    return;
                }

                if (msg.Target == "FirmarContrato")
                    await MainPage.Navigation.PushAsync(new Autenticar(id));
                else if (msg.Target == "ContratoNormativo")
                    await MainPage.Navigation.PushAsync(new ContratoNormativo(id));
            });

         
            WeakReferenceMessenger.Default.Register<ResetPasswordLinkMessage>(this, async (r, msg) =>
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                   
                    await MainPage.Navigation.PushAsync(new ReiniciarContraseña(msg.Token, msg.Uid));

                  
                });
            });

          
            WeakReferenceMessenger.Default.Register<ResetPasswordInvalidLinkMessage>(this, async (r, msg) =>
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                    await MainPage.DisplayAlert("Enlace inválido", msg?.Reason ?? "No fue posible leer el token o el usuario.", "OK"));
            });
        }
    }
}
