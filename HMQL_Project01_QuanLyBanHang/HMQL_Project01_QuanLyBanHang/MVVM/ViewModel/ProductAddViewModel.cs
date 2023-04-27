using HMQL_Project01_QuanLyBanHang.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using BookDataBinding;
using System.Collections.ObjectModel;

namespace HMQL_Project01_QuanLyBanHang.MVVM.ViewModel
{
    internal class ProductAddViewModel : ObservableObject
    {
        public ProductAddViewModel()
        {
            //ObservableCollection<Book> _book = null;
            //// this.DataSource = sv;
            //BookDAO bookDAO = new BookDAO();
            //_book = bookDAO.GetAll();
            //bookList.ItemsSource = _book;
        }
    }
}