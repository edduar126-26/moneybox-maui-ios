using Money_Box.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Money_Box.Services
{
        public class DialogService : IDialogService
        {
            public Task ShowMessage(string title, string message) =>
                Application.Current.MainPage.DisplayAlert(title, message, "OK");

            public Task ShowError(string message) =>
                ShowMessage("Error!", message);
        }
    
}
