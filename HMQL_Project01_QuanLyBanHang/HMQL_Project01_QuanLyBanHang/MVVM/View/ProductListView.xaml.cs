using BookDataBinding;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace HMQL_Project01_QuanLyBanHang.MVVM.View
{
    /// <summary>
    /// Interaction logic for ProductListView.xaml
    /// </summary>
    public partial class ProductListView : UserControl
    {
        private ObservableCollection<Book> _book = null;

        public ProductListView()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BookDAO bookDAO = new BookDAO();
            _book = bookDAO.GetAll();
            bookList.ItemsSource = _book;
        }
    }
}