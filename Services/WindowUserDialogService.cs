using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Services
{
    class WindowUserDialogService : IUserDialogService
    {
        public bool Confirm(string Message, string Caption, bool Exclamation = false)
        {
            throw new NotImplementedException();
        }

        public bool Edit(object item)
        {
            throw new NotImplementedException();
        }

        public void ShowError(string Message, string Caption)
        {
            throw new NotImplementedException();
        }

        public bool ShowInformation(string Information, string Caption)
        {
            throw new NotImplementedException();
        }

        public void ShowWorning(string Message, string Caption)
        {
            throw new NotImplementedException();
        }
    }
 
}
