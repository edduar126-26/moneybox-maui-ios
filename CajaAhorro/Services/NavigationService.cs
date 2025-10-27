using Money_Box.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Money_Box.Services
{
    public class NavigationService : INavigationService
    {
        public Task PushAsync(Page page)
        {
            return (Application.Current.MainPage as NavigationPage)?.PushAsync(page)
                   ?? Task.CompletedTask;
        }

        public Task PopAsync()
        {
            return (Application.Current.MainPage as NavigationPage)?.PopAsync()
                   ?? Task.CompletedTask;
        }

        public Task PushModalAsync(Page page)
        {
            return Application.Current.MainPage?.Navigation.PushModalAsync(page)
                   ?? Task.CompletedTask;
        }

        public Task PopToRootAsync()
        {
            return (Application.Current.MainPage as NavigationPage)?.PopToRootAsync()
                   ?? Task.CompletedTask;
        }

        public Task NavigateToRoot(Page rootPage)
        {
            Application.Current.MainPage = new NavigationPage(rootPage);
            return Task.CompletedTask;
        }
    }

}
