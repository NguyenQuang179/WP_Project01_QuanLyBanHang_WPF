using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HMQL_Project01_QuanLyBanHang.MVVM.View
{
    /// <summary>
    /// Interaction logic for ProductListView.xaml
    /// </summary>
    public partial class ProductListView : UserControl
    {
        //private ObservableCollection<Book> _book = null;

        //public ProductListView()
        //{
        //    InitializeComponent();
        //}

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BookDAO bookDAO = new BookDAO();
            _book = bookDAO.GetAll();
            bookList.ItemsSource = _book;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = searchTextBox.Text;
            // Perform search logic here
        }

        private void searchTextBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            searchTextBox.Text = "";
        }

        private void searchTB_MouseEnter(object sender, MouseEventArgs e)
        {
        }

        private void searchTB_MouseLeave(object sender, MouseEventArgs e)
        {
        }

        private void eraseButton_Click(object sender, RoutedEventArgs e)
        {
            searchTextBox.Text = "";
            Keyboard.ClearFocus();
        }

        private void sortButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private static readonly Regex _regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text

        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }
    }
}