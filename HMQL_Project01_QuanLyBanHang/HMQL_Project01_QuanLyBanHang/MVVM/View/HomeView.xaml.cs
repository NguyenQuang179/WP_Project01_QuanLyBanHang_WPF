using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HMQL_Project01_QuanLyBanHang.MVVM.View
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
        }

        public void clicked(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Clicked");
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Clicked");
        }
    }
}
