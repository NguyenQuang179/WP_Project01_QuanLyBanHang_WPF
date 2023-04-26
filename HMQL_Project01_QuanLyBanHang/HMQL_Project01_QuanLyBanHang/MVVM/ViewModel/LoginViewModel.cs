using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Security.Principal;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using HMQL_Project01_QuanLyBanHang.Core;
using HMQL_Project01_QuanLyBanHang.MVVM.Model;
using Newtonsoft.Json;

namespace HMQL_Project01_QuanLyBanHang.MVVM.ViewModel
{

    class LoginViewModel : ObservableObject
    {
        private Account account;

        public RelayCommand LoginCommand { get; set; }

        public RelayCommand SignUpCommand { get; set; }

        public RelayCommand RecoverPassCommand { get; set; }

        public LoginViewModel(AuthenticationViewModel authenticationVM)
        {
            account = new Account();

            LoginCommand = new RelayCommand(async o =>
            {
                //MessageBox.Show($"Username: {Username}\nPassword: {Password}");
                //Username = "none";
                //Password = "none";

                var uri = new Uri($"{ConnectionString.connectionString}/dashboard");

                try
                {
                    using var client = new HttpClient();
                    //var jsonMsg = JsonConvert.SerializeObject(account);
                    //var data = new StringContent(jsonMsg, Encoding.UTF8,"application/json");

                    // Send the request and get the response
                    //var response = await client.PostAsync(uri, data);
                    var response = await client.GetAsync(uri);

                    // Check if the upload was successful
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        // Handle the successful upload
                        MessageBox.Show($"Success {json}");
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        Application.Current.Windows[0].Close();
                    }
                    else
                    {
                        // Handle the failed upload
                        MessageBox.Show("Failed");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
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