using HMQL_Project01_QuanLyBanHang.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMQL_Project01_QuanLyBanHang.MVVM.ViewModel
{
    class RecoverPasswordViewModel : ObservableObject
    {
        public RelayCommand LoginCommand { get; set; }

        public RecoverPasswordViewModel(AuthenticationViewModel authenticationVM)
        {
            LoginCommand = new RelayCommand(o =>
            {
                authenticationVM.LoginViewCommand.Execute(null);
            });
        }
    }
}
