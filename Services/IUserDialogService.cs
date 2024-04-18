using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Services
{
    interface IUserDialogService
    {
        bool Edit(object item, string title = "", int type = -1);
        bool ShowInformation(string Information, string Caption);
        void ShowWorning(string Message, string Caption);
        void ShowError(string Message, string Caption);
        bool Confirm(string Message, string Caption, bool Exclamation = false);
    }
}
