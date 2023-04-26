using HMQL_Project01_QuanLyBanHang.Core;
using System.Windows;

namespace HMQL_Project01_QuanLyBanHang.MVVM.ViewModel
{
    class AuthenticationViewModel : ObservableObject
    {
        public RelayCommand DragWindowCommand { get; set; }

        public RelayCommand MinimizedWindowCommand { get; set; }

        public RelayCommand WindowStateCommand { get; set; }

        public RelayCommand ExitWindowCommand { get; set; }

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

            DragWindowCommand = new RelayCommand(o =>
            {
                Application.Current.Windows[0].DragMove();
            });

            MinimizedWindowCommand = new RelayCommand(o =>
            {
                Application.Current.Windows[0].WindowState = WindowState.Minimized;
            });

            WindowStateCommand = new RelayCommand(o =>
            {
                if (Application.Current.Windows[0].WindowState != WindowState.Maximized)
                {
                    Application.Current.Windows[0].WindowState = WindowState.Maximized;
                }
                else
                {
                    Application.Current.Windows[0].WindowState = WindowState.Normal;
                }
            });

            ExitWindowCommand = new RelayCommand(o =>
            {
                Application.Current.Shutdown();
            });

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
