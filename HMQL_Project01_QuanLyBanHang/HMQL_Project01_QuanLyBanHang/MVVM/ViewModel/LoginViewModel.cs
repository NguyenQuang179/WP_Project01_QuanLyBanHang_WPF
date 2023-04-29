using System;
using System.Collections.Generic;
using System.Configuration;
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

        private Token userToken;
        public Token UserToken
        {
           get { return userToken; }
           set
            {
                userToken = value;
                OnPropertyChanged(nameof(UserToken));
            }
        }

        private bool rememberCheckboxSelected;
        public bool RememberCheckboxSelected
        {
            get => rememberCheckboxSelected;
            set
            {
                rememberCheckboxSelected = value;
                OnPropertyChanged(nameof(RememberCheckboxSelected));
            }
        }

        public RelayCommand LoginCommand { get; set; }

        public RelayCommand SignUpCommand { get; set; }

        public RelayCommand RecoverPassCommand { get; set; }

        public LoginViewModel(AuthenticationViewModel authenticationVM)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            account = new Account();
            UserToken = new Token();
            RememberCheckboxSelected = false;
            UserToken.token = ConfigurationManager.AppSettings["Token"];
            Username = ConfigurationManager.AppSettings["Username"];
            Password = ConfigurationManager.AppSettings["Password"];

            LoginCommand = new RelayCommand(async o =>
            {
                if (UserToken.token != "")
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    Application.Current.Windows[0].Close();
                    return;
                }  
                var uri = new Uri($"{ConnectionString.connectionString}/user/login");
                try
                {
                    using var client = new HttpClient();
                    var jsonMsg = JsonConvert.SerializeObject(account);
                    var data = new StringContent(jsonMsg, Encoding.UTF8, "application/json");

                    // Send the request and get the response
                    var response = await client.PostAsync(uri, data);
                    //var response = await client.GetAsync(uri);

                    // Check if the upload was successful
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        UserToken = JsonConvert.DeserializeObject<Token>(json);
                        // Handle the successful upload
                        if(RememberCheckboxSelected)
                        {
                            config.AppSettings.Settings["Username"].Value = Username;
                            config.AppSettings.Settings["Password"].Value = Password;
                            config.AppSettings.Settings["Token"].Value = UserToken.token;
                        }
                        config.Save(ConfigurationSaveMode.Full);
                        ConfigurationManager.RefreshSection("appSettings");
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        Application.Current.Windows[0].Close();
                    }
                    else
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Fail: {json}");
                    }
                    Username = "";
                    Password = "";
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