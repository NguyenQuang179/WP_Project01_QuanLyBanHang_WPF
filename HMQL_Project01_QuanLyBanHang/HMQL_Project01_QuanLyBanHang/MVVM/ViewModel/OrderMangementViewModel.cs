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

namespace HMQL_Project01_QuanLyBanHang.MVVM.ViewModel
{
    
    class OrderMangementViewModel : ObservableObject
    {
        private ListOfOrder orders;
        public ListOfOrder Orders {
            get => orders;
            set
            {
                orders = value;
                OnPropertyChanged(nameof(Orders));
            }
        }

        public RelayCommand CallOrderData;

        public OrderMangementViewModel() { 
            orders = new ListOfOrder();

            CallOrderData = new RelayCommand(async o =>
            {
                var uri = new Uri($"{ConnectionString.connectionString}/order/search");

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
                        for(int i = 0; i < Orders.listOfOrder.Count; i++)
                        {
                            datetime = DateTime.Parse(Orders.listOfOrder[i].date);
                            newdate = datetime.ToString("dd/mm/y hh:mm tt");
                            Orders.listOfOrder[i].date = newdate;
                        }
                        MessageBox.Show($"{Orders.listOfOrder.Count} {newdate}");
                    }
                    else { MessageBox.Show($"Fail To Call Data"); }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });

            CallOrderData.Execute(null);
        }
    }
}
