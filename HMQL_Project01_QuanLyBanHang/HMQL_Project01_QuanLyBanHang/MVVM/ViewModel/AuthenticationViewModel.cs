using HMQL_Project01_QuanLyBanHang.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMQL_Project01_QuanLyBanHang.MVVM.ViewModel
{
    class AuthenticationViewModel : ObservableObject
    {
        public RelayCommand LoginViewCommand { get; set; }

        public RelayCommand SignUpViewCommand { get; set; }

        public RelayCommand RecoverPassViewCommand { get; set; }

        public LoginViewModel LoginVM { get; set; }

        public SignUpViewModel SignUpVM { get; set; }

        public RecoverPasswordViewModel RecoverPasswordVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public AuthenticationViewModel()
        {
            LoginVM = new LoginViewModel(this);

            SignUpVM = new SignUpViewModel(this);

            RecoverPasswordVM = new RecoverPasswordViewModel(this);

            CurrentView = LoginVM;

            LoginViewCommand = new RelayCommand(o =>
            {
                CurrentView = LoginVM;
            });

            SignUpViewCommand = new RelayCommand(o =>
            {
                CurrentView = SignUpVM;
            });

            RecoverPassViewCommand = new RelayCommand(o =>
            {
                CurrentView = RecoverPasswordVM;
            });
        }
    }
}
