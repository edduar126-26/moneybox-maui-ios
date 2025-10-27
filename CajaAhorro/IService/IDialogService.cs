using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Money_Box.IService
{
    public interface IDialogService
    {
        Task ShowMessage(string title, string message);
        Task ShowError(string message);
    }
}
