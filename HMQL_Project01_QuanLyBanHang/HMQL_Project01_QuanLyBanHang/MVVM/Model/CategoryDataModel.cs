using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMQL_Project01_QuanLyBanHang.MVVM.Model
{
    class CategoryName
    {
        public string name { get; set; }
    }
    class Category
    {
        public string _id { get; set; }
        public string name { get; set; }
        public List<string> listOfBook { get; set; }
    }

    class ListCategory
    {
        public List<Category> listOfCategory { get; set; }
        public int numOfCategory { get; set; }
        public int numOfPage { get; set; }
    }

    class ListBookCategory
    {
        public List<BookDetail> listOfBook { get; set; }
        public int numOfBooks { get; set; }
        public int numOfPage { get; set; }
    }
}
