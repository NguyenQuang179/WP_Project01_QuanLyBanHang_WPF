using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMQL_Project01_QuanLyBanHang.MVVM.Model
{
    public class BookInOrderForDetails
    {
        public string _id { get; set; }
        public Book book { get; set; }
        public int quantity { get; set; }
    }

    public class OrderForDetails
    {
        public string _id { get; set; }
        public string date { get; set; }
        public List<BookInOrderForDetails> listOfBook { get; set; }
        public long totalPrice { get; set; }
    }

    public class OrderDetails
    {
        public OrderForDetails order { get; set; }
    }

    public class ListOfOrderForDetails
    {
        public List<OrderForDetails> listOfOrder { get; set; }
    }
}
