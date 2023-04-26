using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMQL_Project01_QuanLyBanHang.MVVM.Model
{
    public class DashboardDataModel
    {
        public int numOfBooks { get; set; }
        public int numOfOrderThisWeek { get; set; }
        public int numOfOrderThisMonth { get; set; }
        public List<Book> listOfBookOutOfStock { get; set; }

    }
}
