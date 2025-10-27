using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;        
using Android.Views;          
using AndroidX.Core.View;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using System;
using Color = Android.Graphics.Color;



namespace Money_Box
{
    [Activity(
         Theme = "@style/Maui.SplashTheme",
         MainLauncher = true,
         LaunchMode = LaunchMode.SingleTop,
         Exported = true,
         ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
                                ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    // Deep links:
    [IntentFilter(
    new[] { Intent.ActionView },
    Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
    DataScheme = "moneybox", DataHost = "firmarcontrato", DataPathPrefix = "/inicio")]
    [IntentFilter(
    new[] { Intent.ActionView },
    Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
    DataScheme = "moneybox", DataHost = "contratonormativo", DataPathPrefix = "/inicio")]

    [IntentFilter(
    new[] { Intent.ActionView },
    Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
    DataScheme = "moneybox", DataHost = "reset-password")]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

   
            WindowCompat.SetDecorFitsSystemWindows(Window, false);
            Window.SetStatusBarColor(Color.Transparent);
            Window.SetNavigationBarColor(Color.Transparent);

           
            var decor = Window.DecorView;
            var controller = new WindowInsetsControllerCompat(Window, decor);
          

            
            ViewCompat.SetOnApplyWindowInsetsListener(decor, new InsetsListener());

            HandleIntent(Intent);
        }


        protected override void OnNewIntent(Intent? intent)
        {
            base.OnNewIntent(intent);
            HandleIntent(intent);
        }

   
        void HandleIntent(Intent? intent)
        {
                if (intent?.Action != Intent.ActionView || intent.Data == null)
                    return;

                var auri = intent.Data;
                var host = (auri.Host ?? string.Empty).ToLowerInvariant();

                if (host == "reset-password")
                {
                    var token = auri.GetQueryParameter("token");
                    var uidStr = auri.GetQueryParameter("uid");

                    if (string.IsNullOrWhiteSpace(token) || !int.TryParse(uidStr, out var uid) || uid <= 0) 
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            WeakReferenceMessenger.Default.Send(
                                new ResetPasswordInvalidLinkMessage("No fue posible leer el token o el usuario.")
                            );
                        });
                        return;
                    }

                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        WeakReferenceMessenger.Default.Send(new ResetPasswordLinkMessage(token, uid));
                    });
                    return;
                }

            
                var folio = auri.GetQueryParameter("folio") ?? string.Empty;
                if (string.IsNullOrWhiteSpace(folio))
                {
                    var segs = auri.PathSegments;
                    if (segs != null && segs.Count >= 2) folio = segs[1];
                }

                var target = host == "firmarcontrato" ? "FirmarContrato" : "ContratoNormativo";
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    WeakReferenceMessenger.Default.Send(new DeepLinkMessage(target, folio));
                });
        }

        class InsetsListener : Java.Lang.Object, AndroidX.Core.View.IOnApplyWindowInsetsListener
        {
            
            public WindowInsetsCompat OnApplyWindowInsets(Android.Views.View v, WindowInsetsCompat insets)
            {
                var bars = insets.GetInsets(WindowInsetsCompat.Type.SystemBars());
                v.SetPadding(bars.Left, bars.Top, bars.Right, bars.Bottom);
                return insets; 
            }
        }

    }
}
