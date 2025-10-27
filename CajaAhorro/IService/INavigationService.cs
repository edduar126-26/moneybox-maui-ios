using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Money_Box.IService
{
    public interface INavigationService
    {
        Task PushAsync(Page page);
        Task PopAsync();
        Task PushModalAsync(Page page);
        Task PopToRootAsync();
        Task NavigateToRoot(Page rootPage);
    }
}
