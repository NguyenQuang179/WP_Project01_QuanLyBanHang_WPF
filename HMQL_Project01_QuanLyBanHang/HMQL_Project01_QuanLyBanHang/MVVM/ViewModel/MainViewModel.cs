using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using HMQL_Project01_QuanLyBanHang.Core;
using HMQL_Project01_QuanLyBanHang.MVVM.View;

namespace HMQL_Project01_QuanLyBanHang.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        public RelayCommand DragWindowCommand { get; set; }

        public RelayCommand MinimizedWindowCommand { get; set; }

        public RelayCommand WindowStateCommand { get; set; }

        public RelayCommand ExitWindowCommand { get; set; }

        public RelayCommand HomeViewCommand { get; set; }

        public RelayCommand DashboardViewCommand { get; set; }

        public RelayCommand SalesReportViewCommand { get; set; }

        public RelayCommand ProductListViewCommand { get; set; }

        public RelayCommand LogOutCommand { get; set; }

        //Order
        public RelayCommand OrderManagementViewCommand { get; set; }

        public RelayCommand OrderDetailViewCommand { get; set; }
        public RelayCommand OrderAddBookViewCommand { get; set; }
        public RelayCommand OrderAddViewCommand { get; set; }

        //Category
        public RelayCommand CategoryManagementViewCommand { get; set; }

        //Product
        public RelayCommand ProductAddViewCommand { get; set; }

        public RelayCommand ProductViewCommand { get; set; }

        public HomeViewModel HomeVM { get; set; }

        public DashboardViewModel DashboardVM { get; set; }

        public SalesReportViewModel SalesReportVM { get; set; }

        //COrder
        public OrderMangementViewModel OrderManagementVM { get; set; }

        public OrderDetailViewModel ORderDetailVM { get; set; }
        public OrderAddBookViewModel OrderAddBookVM { get; set; }

        //Category
        public CategoryManagementViewModel CategoryManagementVM { get; set; }

        //Product
        public ProductListViewModel ProductListVM { get; set; }


        public ProductAddViewModel ProductAddVM { get; set; }

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

        public MainViewModel()
        {
            HomeVM = new HomeViewModel();

            DashboardVM = new DashboardViewModel(this);

            SalesReportVM = new SalesReportViewModel();
            //ORder
            OrderManagementVM = new OrderMangementViewModel(this);
            OrderAddBookVM = new OrderAddBookViewModel(this);
            //Category
            CategoryManagementVM = new CategoryManagementViewModel(this);
            //Product
            ProductListVM = new ProductListViewModel(this);

            //ProductAddVM = new ProductAddViewModel(this);

            //ProductViewVM = new ProductViewModel(this);

            CurrentView = DashboardVM;

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

            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomeVM;
            });

            DashboardViewCommand = new RelayCommand(o =>
            {
                CurrentView = DashboardVM;
                DashboardVM.CallData.Execute(null);
            });

            SalesReportViewCommand = new RelayCommand(o =>
            {
                CurrentView = SalesReportVM;
            });
            //Order
            OrderManagementViewCommand = new RelayCommand(o =>
            {
                CurrentView = OrderManagementVM;
            });
            OrderAddBookViewCommand = new RelayCommand(o =>
            {
                CurrentView = OrderAddBookVM;
            });
            //Category
            CategoryManagementViewCommand = new RelayCommand(o =>
            {
                CurrentView = CategoryManagementVM;
            });
            //Product
            ProductListViewCommand = new RelayCommand(o =>
            {
                CurrentView = ProductListVM;
            });

            //ProductAddViewCommand = new RelayCommand(o =>
            //{
            //    CurrentView = ProductAddVM;
            //});

            //ProductViewCommand = new RelayCommand(o =>
            //{
            //    CurrentView = ProductViewVM;
            //});

            LogOutCommand = new RelayCommand(o =>
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["Token"].Value = "";
                config.AppSettings.Settings["Username"].Value = "";
                config.AppSettings.Settings["Password"].Value = "";
                config.Save(ConfigurationSaveMode.Full);
                ConfigurationManager.RefreshSection("appSettings");
                AuthenticationWindow authenticationWindow = new AuthenticationWindow();
                authenticationWindow.Show();
                Application.Current.Windows[0].Close();
            });
        }
    }
}