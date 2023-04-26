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
}
