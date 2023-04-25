using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMQL_Project01_QuanLyBanHang.MVVM.Model
{
    public class Order
    {
        public int OrderId { get; set; }
        public string Customer { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string PaymentType { get; set; }
        public string Status { get; set; }
        public int ItemsOrdered { get; set; }
        public decimal TotalAmount { get; set; }

        public Order(int orderId, string customer, DateTime deliveryDate, string paymentType, string status, int itemsOrdered, decimal totalAmount)
        {
            OrderId = orderId;
            Customer = customer;
            DeliveryDate = deliveryDate;
            PaymentType = paymentType;
            Status = status;
            ItemsOrdered = itemsOrdered;
            TotalAmount = totalAmount;
        }
    }
}