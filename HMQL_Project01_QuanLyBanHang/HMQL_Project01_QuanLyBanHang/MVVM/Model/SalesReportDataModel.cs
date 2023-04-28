using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMQL_Project01_QuanLyBanHang.MVVM.Model
{
    class SalesID
    {
        public string date {  get; set; }
    }

    class RootObject
    {
       public List<SalesReportDataModel> incomeReport { get; set; }
    }

    class SalesReportDataModel
    {
        public SalesID _id { get; set; }
        public double totalIncome { get; set; }
    }

    class BookSalesID
    {
        public string date { get; set; }
        public List<Book> book { get; set; }
    }

    class BookSalesIDWeek
    {
        public string week { get; set; }
        public string year { get; set; }
        public List<Book> book { get; set;}
    }

    class BookSales
    {
        public BookSalesID _id { get; set; }
        public int totalQuantity { get; set; }
    }

    class BookSalesWeek
    {
        public BookSalesIDWeek _id { get; set; }
        public int totalQuantity { get; set; }
    }

    class ListOfBookSales
    {
        public List<BookSales> saleReport { get; set; }
    }

    class ListOfBookSalesWeek
    {
        public List<BookSalesWeek> saleReport { get; set; }
    }
}
