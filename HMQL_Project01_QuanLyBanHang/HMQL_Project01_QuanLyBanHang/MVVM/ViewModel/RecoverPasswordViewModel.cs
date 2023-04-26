using HMQL_Project01_QuanLyBanHang.Core;

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
