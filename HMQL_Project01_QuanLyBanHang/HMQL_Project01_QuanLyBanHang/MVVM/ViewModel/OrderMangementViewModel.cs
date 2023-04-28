using HMQL_Project01_QuanLyBanHang.Core;
using HMQL_Project01_QuanLyBanHang.MVVM.Model;
using LiveCharts.Wpf;
using LiveCharts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.Specialized;
using System.Windows.Controls;
using HMQL_Project01_QuanLyBanHang.MVVM.View;
using System.Security.Policy;

namespace HMQL_Project01_QuanLyBanHang.MVVM.ViewModel
{

    class OrderMangementViewModel : ObservableObject
    {

        public string id { get; set; }
        public RelayCommand OrderDetailViewCommand { get; set; }

        public OrderDetailViewModel? OrderDetailVM { get; set; }


        public RelayCommand OrderCreateViewCommand { get; set; }
        public OrderCreateViewModel OrderCreateVM { get; set; }

        public String SelectedOrderID { get; set; }
        public Order SelectedOrder { get; set; }
        private ListOfOrder orders;
        public ListOfOrder Orders
        {
            get => orders;
            set
            {
                orders = value;
                OnPropertyChanged(nameof(Orders));
            }
        }

        public RelayCommand CallOrderData;
        public RelayCommand DeleteOrderData;


        public OrderMangementViewModel(MainViewModel MainVM)
        {
            orders = new ListOfOrder();

            SelectedOrder = null;
            OrderDetailVM = null;




            OrderDetailViewCommand = new RelayCommand((param) =>
            {
                id = param.ToString();
                MessageBox.Show("ID IS:" + id);
                OrderDetailVM = new OrderDetailViewModel(MainVM, id);
                //MessageBox.Show("No Selected Order");

                MainVM.CurrentView = OrderDetailVM;

            });

            OrderCreateViewCommand = new RelayCommand(o =>
            {
                OrderCreateVM = new OrderCreateViewModel(MainVM);
                MainVM.CurrentView = OrderCreateVM;
            });


            CallOrderData = new RelayCommand(async o =>
            {
                var uri = new Uri($"{ConnectionString.connectionString}/order/search?");
                //add count for page
                try
                {
                    using var client = new HttpClient();
                    var response = await client.GetAsync(uri);


                    // Check if the upload was successful
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        Orders = JsonConvert.DeserializeObject<ListOfOrder>(json);
                        string newdate = "";
                        DateTime datetime = DateTime.Now;
                        for (int i = 0; i < Orders.listOfOrder.Count; i++)
                        {
                            datetime = DateTime.Parse(Orders.listOfOrder[i].date);
                            newdate = datetime.ToString("dd/MM/y hh:mm tt");
                            Orders.listOfOrder[i].date = newdate;
                        }
                        //MessageBox.Show($"{Orders.listOfOrder.Count} {newdate}");
                    }
                    else { MessageBox.Show($"Fail To Call Data"); }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });

            DeleteOrderData = new RelayCommand(async (param) =>
            {
                MessageBox.Show("Hello");
                List<Order> curList = Orders.listOfOrder;
                // Find the index of the book to remove

                string orderId = param as string;
                if (orderId == null)
                {
                    //show error
                    MessageBox.Show("Invalid Order ID");
                    return;
                }
                // Find the index of the book to remove
                int indexToRemove = curList.FindIndex(b => b._id == orderId);


                if (indexToRemove != -1)
                {
                    // Remove the book from curList
                    using var client = new HttpClient();
                    var uri = new Uri($"{ConnectionString.connectionString}/order/delete/{orderId}");
                    var response = await client.DeleteAsync(uri);


                    // Check if the upload was successful
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        MessageBox.Show(json);
                        //MessageBox.Show($"{Orders.listOfOrder.Count} {newdate}");
                    }
                    else { MessageBox.Show($"Fail To Delete Order"); }
                    curList.RemoveAt(indexToRemove);

                }
                else
                {
                    //show error
                    MessageBox.Show("No Order Selected");
                }
            });

            CallOrderData.Execute(null);
        }
    }
}
