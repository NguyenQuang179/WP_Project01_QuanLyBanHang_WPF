using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMQL_Project01_QuanLyBanHang.MVVM.Model
{
    
    public class BookInOrder
    {
        public string _id {  get; set; }
        public string book { get; set; }
        public int quantity { get; set; }
    }

    public class Order
    {
        public string _id { get; set; }
        public string date { get; set; }
        public List<BookInOrder> listOfBook { get; set; }
        public long totalPrice { get; set; }
    }

    public class ListOfOrder
    {
        public List<Order> listOfOrder { get; set; }
    }
}