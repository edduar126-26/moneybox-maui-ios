using CommunityToolkit.Mvvm.Messaging;
using Foundation;
using UIKit;

namespace Money_Box
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            try
            {
                if (url.Scheme?.ToLower() == "moneybox")
                {
                    var host = url.Host?.ToLower();      
                    var path = url.Path?.ToLower();    
                    var query = url.Query;               

                  
                    var token = GetQueryParam(query, "token");
                    var uidStr = GetQueryParam(query, "uid");

                    
                    if (host == "reset-password")
                    {
                        
                        if (string.IsNullOrWhiteSpace(token) ||
                            !int.TryParse(uidStr, out var uid) || uid <= 0)
                        {
                            Microsoft.Maui.Controls.Application.Current?.Dispatcher.Dispatch(() =>
                            {
                                CommunityToolkit.Mvvm.Messaging.WeakReferenceMessenger.Default.Send(
                                    new ResetPasswordInvalidLinkMessage("No fue posible leer el token o el usuario.")
                                );
                            });
                            return true; 
                        }

                        Microsoft.Maui.Controls.Application.Current?.Dispatcher.Dispatch(() =>
                        {
                            
                            CommunityToolkit.Mvvm.Messaging.WeakReferenceMessenger.Default.Send(
                                new ResetPasswordLinkMessage(token!, uid)
                            );

                         
                        });

                        return true;
                    }

                   
                    Microsoft.Maui.Controls.Application.Current?.Dispatcher.Dispatch(async () =>
                    {
                        var app = IPlatformApplication.Current?.Application as Microsoft.Maui.Controls.Application;
                        if (app?.MainPage is Shell shell)
                        {
                            if (host == "firmarcontrato" && path.StartsWith("/inicio"))
                            {
                                var route = $"//FirmarContratoPage";
                                if (!string.IsNullOrEmpty(token))
                                    route += $"?token={Uri.EscapeDataString(token)}";
                                await shell.GoToAsync(route);
                                return;
                            }

                            if (host == "contratonormativo" && path.StartsWith("/inicio"))
                            {
                                var route = $"//ContratoNormativoPage";
                                if (!string.IsNullOrEmpty(token))
                                    route += $"?token={Uri.EscapeDataString(token)}";
                                await shell.GoToAsync(route);
                                return;
                            }
                        }
                        else if (app?.MainPage is NavigationPage nav)
                        {
                            if (host == "firmarcontrato" && path.StartsWith("/inicio"))
                            {
                                await nav.PushAsync(new Views.Autenticar(int.Parse(token!)));
                                return;
                            }

                            if (host == "contratonormativo" && path.StartsWith("/inicio"))
                            {
                                await nav.PushAsync(new Views.ContratoNormativo(int.Parse(token!)));
                                return;
                            }
                        }
                    });

                    return true; 
                }
            }
            catch
            {
                
            }

            return base.OpenUrl(app, url, options);
        }


        private static string? GetQueryParam(string? query, string key)
        {
            if (string.IsNullOrEmpty(query)) return null;
            var parts = query.Split('&', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            foreach (var p in parts)
            {
                var kv = p.Split('=', 2);
                if (kv.Length == 2 && kv[0].Equals(key, StringComparison.OrdinalIgnoreCase))
                    return Uri.UnescapeDataString(kv[1]);
            }
            return null;
        }

      
        public override bool ContinueUserActivity(UIApplication application, NSUserActivity userActivity, UIApplicationRestorationHandler completionHandler)
        {
            if (userActivity.ActivityType == NSUserActivityType.BrowsingWeb && userActivity.WebPageUrl is not null)
            {
                // Manejar universal link: https://tu-dominio/firmarcontrato/inicio
                // Requiere Associated Domains + apple-app-site-association en tu dominio.
                var url = userActivity.WebPageUrl;
                // Ruteo similar al de OpenUrl(...)
                return true;
            }
            return base.ContinueUserActivity(application, userActivity, completionHandler);
        }
    }
}
