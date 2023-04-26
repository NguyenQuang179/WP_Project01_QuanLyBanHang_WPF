using HMQL_Project01_QuanLyBanHang.Core;
using HMQL_Project01_QuanLyBanHang.MVVM.Model;
using System.Windows;

namespace HMQL_Project01_QuanLyBanHang.MVVM.ViewModel
{
    internal class LoginViewModel : ObservableObject
    {
        private Account account;

        public RelayCommand LoginCommand { get; set; }

        public RelayCommand SignUpCommand { get; set; }

        public RelayCommand RecoverPassCommand { get; set; }

        public LoginViewModel(AuthenticationViewModel authenticationVM)
        {
            account = new Account();

            LoginCommand = new RelayCommand(o =>
            {
                MessageBox.Show($"Username: {Username}\nPassword: {Password}");
                Username = "none";
                Password = "none";

                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                Application.Current.Windows[0].Close();
            });

            SignUpCommand = new RelayCommand(o =>
            {
                authenticationVM.SignUpViewCommand.Execute(null);
            });

            RecoverPassCommand = new RelayCommand(o =>
            {
                authenticationVM.RecoverPassViewCommand.Execute(null);
            });
        }

        public string Username
        {
            get => account.username;
            set
            {
                account.username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get => account.password;
            set
            {
                account.password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
    }
}