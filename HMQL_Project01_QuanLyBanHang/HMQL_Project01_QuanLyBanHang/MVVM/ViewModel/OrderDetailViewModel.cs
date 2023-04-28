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
using System.ComponentModel;

namespace HMQL_Project01_QuanLyBanHang.MVVM.ViewModel
{
    class bookTotalPrice : INotifyPropertyChanged
    {
        private int m_price = 0;
        private int m_quantity = 0;

        //NotifyPropertyChangedObject
        public int Price
        {
            get
            {
                return m_price;
            }
            set
            {
                if (value != m_price)
                {
                    m_price = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Price)));
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Sum)));


                }

            }
        }
        public int Quantity
        {
            get
            {
                return m_quantity;
            }
            set
            {
                if (value != m_quantity)
                {
                    m_price = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Quantity)));
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Sum)));


                }

            }
        }
        public int Sum
        {
            get
            {
                return m_price * m_quantity;
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
    }
        class OrderDetailViewModel : ObservableObject
        {

            private OrderDetails orderD;
            public OrderDetails OrderD
        {
            get => orderD;
            set
            {
                orderD = value;
                OnPropertyChanged(nameof(OrderD));
            }
        }

            public RelayCommand CallOrderDetailData { get; set; }
            public RelayCommand OrderAddBookCreateViewCommand { get; set; }

            public RelayCommand ConfirmOrderDetailData { get; set; }
            public OrderDetailViewModel(MainViewModel MainVM, String OrderID)
            {
            //orders = new ListOfOrder();

            //SelectedOrder = null;
            //BookDetailVM = null;

            //BookAddVM = new OrderCreateViewModel();


            //List<Book> ListOfBook;
            //OrderDetailViewCommand = new RelayCommand((param) =>
            //{
            //    string id = param.ToString();
            //    MessageBox.Show("ID IS:" + id);
            //    OrderDetailVM = new OrderDetailViewModel(id);
            //    MessageBox.Show("No Selected Order");
            //    if (OrderDetailVM != null)
            //        MainVM.CurrentView = OrderDetailVM;

            //});

            ConfirmOrderDetailData = new RelayCommand(o =>
            {
                //Update Order Detail API
            });
            

            OrderAddBookCreateViewCommand = new RelayCommand(o =>
            {
                MainVM.OrderAddBookViewCommand.Execute(MainVM);
            });

            CallOrderDetailData = new RelayCommand(async o =>
                {
                    var uri = new Uri($"{ConnectionString.connectionString}/order/detail/" + OrderID);
                    //add count for page
                    try
                    {
                        using var client = new HttpClient();
                        var response = await client.GetAsync(uri);


                        // Check if the upload was successful
                        if (response.IsSuccessStatusCode)
                        {
                            var json = await response.Content.ReadAsStringAsync();
                            OrderD = JsonConvert.DeserializeObject<OrderDetails>(json);
                            string newdate = "";
                            DateTime datetime = DateTime.Now;
                            datetime = DateTime.Parse(OrderD.order.date);
                            newdate = datetime.ToString("dd/MM/y");
                            OrderD.order.date = newdate;
                            MessageBox.Show($"{OrderD.order.date}");

                            //Process Price
                            //for (int i = 0; i < orderD.order.listOfBook.Count; i++)
                            //{
                            //    bookTotalPrice bTP = new bookTotalPrice();
                            //    bTP.Price = orderD.order.listOfBook[i].book.price;
                            //    bTP.Quantity = orderD.order.listOfBook[i].quantity;
                            //}
                        }
                        else { MessageBox.Show($"Fail To Call Data"); }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                });

                CallOrderDetailData.Execute(null);

                
            }

           
        }
    }


