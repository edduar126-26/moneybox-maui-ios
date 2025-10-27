using Money_Box.Models;
using Money_Box.ViewModels;
using Money_Box.Views;
using System;
using System.Linq;
using Microsoft.Maui.Controls;
using SkiaSharp.Views.Maui;
using SkiaSharp;

namespace Money_Box.Views
{
    public partial class FirmarPagareSocio : ContentPage
    {
        private readonly FirmarPagareSocioViewModel viewModel;
        private SKImageInfo lastCanvasSize;
        private readonly int _folioSolicitud;
        private SKPath path;

        public FirmarPagareSocio(int folioSolicitud)
        {
            InitializeComponent();
            canvasView.EnableTouchEvents = true; 
            _folioSolicitud = folioSolicitud;
            path = new SKPath();
           

            var factory = App.ServiceProvider.GetRequiredService<Func<int, FirmarPagareSocioViewModel>>();
            viewModel = factory(_folioSolicitud);

            
            BindingContext = viewModel;
        }

        // Dibuja sobre el canvas


        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            lastCanvasSize = e.Info; // Guarda tamaño real del canvas en pixeles

            var canvas = e.Surface.Canvas;
            canvas.Clear();

            var paint = new SKPaint
            {
                Color = SKColors.Black,
                StrokeWidth = 5,
                IsAntialias = true,
                Style = SKPaintStyle.Stroke
            };

            canvas.DrawPath(path, paint);
        }

        // Maneja los toques del usuario en el canvas
        private void OnTouchEffectAction(object sender, SKTouchEventArgs e)
        {
            Console.WriteLine($"Touch: {e.ActionType} at {e.Location}"); // <-- log de prueba

            switch (e.ActionType)
            {
                case SKTouchAction.Pressed:
                    path.MoveTo(e.Location);
                    break;
                case SKTouchAction.Moved:
                    path.LineTo(e.Location);
                   break;
                case SKTouchAction.Released:
                    break;
            }

            canvasView.InvalidateSurface();
            e.Handled = true;
        }

        // Evento para el botón Firmar
        private async void OnFirmarClicked(object sender, EventArgs e)
        {
            if (path.IsEmpty)
            {
                await DisplayAlert("Atención", "Por favor, realiza tu firma antes de continuar.", "OK");
                return;
            }

            var imageInfo = new SKImageInfo(lastCanvasSize.Width, lastCanvasSize.Height);
            using (var surface = SKSurface.Create(imageInfo))
            {
                var canvas = surface.Canvas;
                canvas.Clear();

               var paint = new SKPaint
                {
                    Color = SKColors.Black,
                    StrokeWidth = 5,
                    IsAntialias = true,
                    Style = SKPaintStyle.Stroke
                };

                canvas.DrawPath(path, paint);

                var skImage = surface.Snapshot();
                var skData = skImage.Encode();
                var imageBytes = skData.ToArray();

                var firmaBase64 = Convert.ToBase64String(imageBytes);
                viewModel.FirmaBase64 = firmaBase64;

                bool exito = await viewModel.EnviarFirmaAsync();

                if (exito)
                {
                    var acceso = App.ServiceProvider.GetRequiredService<Func<caSocio?, UsersViewModel>>();
                    Application.Current.MainPage = new NavigationPage(new Acceso(acceso));
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo enviar la firma", "OK");
                }
            }
        }


       
        private void OnClearClicked(object sender, EventArgs e)
        {
            path = new SKPath();  
            canvasView.InvalidateSurface();
        }
    }
}
