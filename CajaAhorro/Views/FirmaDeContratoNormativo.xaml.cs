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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FirmaDeContratoNormativo : ContentPage
    {

        private readonly FirmaContratoNormativoViewModel viewModel;
        private SKImageInfo lastCanvasSize;
        private SKPath path;

        public FirmaDeContratoNormativo(int IdUsuario)
        {
            InitializeComponent();
            canvasViewC.EnableTouchEvents = true;
            path = new SKPath();
            var factory = App.ServiceProvider.GetRequiredService<Func<int, FirmaContratoNormativoViewModel>>();
            viewModel = factory(IdUsuario);
            BindingContext = viewModel;
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e) 
        {
            lastCanvasSize = e.Info;

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


        private void OnTouchEffectAction(object sender, SKTouchEventArgs e)
        {
            Console.WriteLine($"Touch: {e.ActionType} at {e.Location}");

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

            canvasViewC.InvalidateSurface();
            e.Handled = true;
        }


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

                    await Navigation.PopToRootAsync();
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
            canvasViewC.InvalidateSurface();
        }
    }
}