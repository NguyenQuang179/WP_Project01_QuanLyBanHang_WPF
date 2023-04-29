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

        private bool dashboardIsSelected;
        public bool DashboardIsSelected
        {
            get => dashboardIsSelected;
            set
            {
                dashboardIsSelected = value;
                OnPropertyChanged(nameof(DashboardIsSelected));
            }
        }
        private bool productIsSelected;
        public bool ProductIsSelected
        {
            get => productIsSelected;
            set
            {
                productIsSelected = value;
                OnPropertyChanged(nameof(ProductIsSelected));
            }
        }
        private bool categoryIsSelected;
        public bool CategoryIsSelected
        {
            get => categoryIsSelected;
            set
            {
                categoryIsSelected = value;
                OnPropertyChanged(nameof(CategoryIsSelected));
            }
        }
        private bool orderIsSelected;
        public bool OrderIsSelected
        {
            get => orderIsSelected;
            set
            {
                orderIsSelected = value;
                OnPropertyChanged(nameof(OrderIsSelected));
            }
        }
        private bool salesReportIsSelected;
        public bool SalesReportIsSelected
        {
            get => salesReportIsSelected;
            set
            {
                salesReportIsSelected = value;
                OnPropertyChanged(nameof(SalesReportIsSelected));
            }
        }

        public MainViewModel()
        {
            DashboardVM = new DashboardViewModel(this);

            SalesReportVM = new SalesReportViewModel();
            //ORder
            OrderManagementVM = new OrderMangementViewModel(this);
            OrderAddBookVM = new OrderAddBookViewModel(this);
            //Category
            CategoryManagementVM = new CategoryManagementViewModel(this);
            //Product
            ProductListVM = new ProductListViewModel(this);

            CurrentView = DashboardVM;

            DashboardIsSelected = true;
            ProductIsSelected = false; 
            CategoryIsSelected = false;
            OrderIsSelected = false;
            SalesReportIsSelected = false;

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

            DashboardViewCommand = new RelayCommand(o =>
            {
                CurrentView = DashboardVM;
                DashboardIsSelected = true;
                DashboardVM.CallData.Execute(null);
            });

            SalesReportViewCommand = new RelayCommand(o =>
            {
                CurrentView = SalesReportVM;
                SalesReportIsSelected = true;
                SalesReportVM.CallData.Execute(null);
            });
            //Order
            OrderManagementViewCommand = new RelayCommand(o =>
            {
                CurrentView = OrderManagementVM;
                OrderIsSelected = true;
            });
            OrderAddBookViewCommand = new RelayCommand(o =>
            {
                CurrentView = OrderAddBookVM;
                OrderIsSelected = true;
            });
            //Category
            CategoryManagementViewCommand = new RelayCommand(o =>
            {
                CurrentView = CategoryManagementVM;
                CategoryIsSelected = true;
            });
            //Product
            ProductListViewCommand = new RelayCommand(o =>
            {
                CurrentView = ProductListVM;
                ProductIsSelected = true;
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