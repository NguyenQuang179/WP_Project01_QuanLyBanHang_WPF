using HMQL_Project01_QuanLyBanHang.Core;
using HMQL_Project01_QuanLyBanHang.MVVM.View;
using System.Windows;

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

        public HomeViewModel HomeVM { get; set; }

        public DashboardViewModel DashboardVM { get; set; }

        public SalesReportViewModel SalesReportVM { get; set; }

        public ProductListViewModel ProductListVM { get; set; }

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

            ProductListVM = new ProductListViewModel();

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
            });

            SalesReportViewCommand = new RelayCommand(o =>
            {
                CurrentView = SalesReportVM;
            });

            ProductListViewCommand = new RelayCommand(o =>
            {
                CurrentView = ProductListVM;
            });

            LogOutCommand = new RelayCommand(o =>
            {
                AuthenticationWindow authenticationWindow = new AuthenticationWindow();
                authenticationWindow.Show();
                Application.Current.Windows[0].Close();
            });
        }
    }
}