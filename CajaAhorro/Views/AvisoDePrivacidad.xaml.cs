using Money_Box.Models;
using Money_Box.ViewModels;
using Money_Box.Views;
using System;
using System.Linq;
using Microsoft.Maui.Controls;

namespace Money_Box.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AvisoDePrivacidad : ContentPage
    {

        public AvisoDePrivacidad()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }
        private async void RegresarBtn_Clicked(object sender, EventArgs e)
        {
            await ((NavigationPage)this.Parent).PopAsync();
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private void WebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            loader.IsRunning = false;
            loader.IsVisible = false;
        }
    }
}