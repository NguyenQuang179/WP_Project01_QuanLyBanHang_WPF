using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMQL_Project01_QuanLyBanHang.MVVM.Model
{
    public class ProductListDataModel
    {
        public List<Book> listOfBook { get; set; }

        public ProductListDataModel()
        {
          //Do nothing
        }
        public  ProductListDataModel(List<Book> listBook)
        {
            listOfBook = listBook;
        }
    }
}