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
using System.Windows.Data;

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


        private string deliDate;
        public string DeliDate
        {
            get { return deliDate; }
            set
            {
                deliDate = value;
                OnPropertyChanged(nameof(DeliDate));
            }
        }

        private long totalPrice;
        public long TotalPrice
        {
            get { return totalPrice; }
            set
            {
                totalPrice = value;
                OnPropertyChanged(nameof(TotalPrice));
            }
        }

        public RelayCommand CallOrderDetailData { get; set; }
        public RelayCommand OrderAddBookCreateViewCommand { get; set; }

        public RelayCommand ConfirmOrderDetailData { get; set; }
        public RelayCommand DeleteBookDetailData { get; set; }

        public RelayCommand CancelCommand { get; set; }
        public RelayCommand UpdatePageDataCommand { get; set; }

        public OrderDetailViewModel(MainViewModel MainVM, String OrderID)
        { 

            ConfirmOrderDetailData = new RelayCommand(async o =>
            {
                NewListOfBook newListOfBook = new NewListOfBook();
                foreach (var bookInfo in OrderD.order.listOfBook)
                {
                    newListOfBook.listOfBook.Add(new BookInOrder(bookInfo.book._id, bookInfo.quantity));
                }


                try
                {
                    var uri = new Uri($"{ConnectionString.connectionString}/order/update/{OrderD.order._id}");
                    var client = new HttpClient();
                    var jsonSended = JsonConvert.SerializeObject(newListOfBook);
                    //MessageBox.Show(jsonSended);
                    var content = new StringContent(jsonSended, Encoding.UTF8, "application/json");
                    // Send the request and get the response
                    var response = await client.PutAsync(uri, content);
                    // Check if the upload was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Handle the successful upload
                        var json = await response.Content.ReadAsStringAsync();

                        MessageBox.Show("Edit Changes Successfully");
                        MainVM.OrderManagementVM.OrderDetailVM = null;
                        MainVM.OrderManagementVM.UpdatePageDataCommand.Execute(null);
                        MainVM.OrderManagementVM.TotalOrder++;
                        MainVM.CurrentView = MainVM.OrderManagementVM;
                    }
                    else
                    {
                        // Handle the failed upload
                        var json = await response.Content.ReadAsStringAsync();
                        MessageBox.Show(json);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });

            DeleteBookDetailData = new RelayCommand((param) =>
            {
                List<BookInOrderForDetails> curList = OrderD.order.listOfBook;
                // Find the index of the book to remove

                string bookId = param as string;
                if (bookId == null)
                {
                    //show error
                    MessageBox.Show("Invalid Book ID");
                    return;
                }
                // Find the index of the book to remove
                int indexToRemove = curList.FindIndex(b => b.book._id == bookId);


                if (indexToRemove != -1)
                {
                    TotalPrice = TotalPrice - (curList[indexToRemove].quantity * curList[indexToRemove].book.price);
                   // Remove the book from curList
                   curList.RemoveAt(indexToRemove);
                    MessageBox.Show("Book has been Deleted, Please Refresh");
                   
                }
                else
                {
                    //show error
                    MessageBox.Show("No Book Selected");
                }
            });

            OrderAddBookCreateViewCommand = new RelayCommand(o =>
            {
                MainVM.OrderAddBookVM.IsAddBookForEditingOrder = true;
                MainVM.OrderAddBookViewCommand.Execute(MainVM);
            });
            CancelCommand = new RelayCommand(o => {
                MainVM.OrderManagementVM.OrderCreateVM = null;
                MainVM.CurrentView = MainVM.OrderManagementVM;
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
                            DeliDate = newdate;
                            TotalPrice = OrderD.order.totalPrice;

                        }
                        else { MessageBox.Show($"Fail To Call Data"); }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                });

            CallOrderDetailData.Execute(null);

            UpdatePageDataCommand = new RelayCommand(async(o) =>
            {
                var uri = new Uri($"{ConnectionString.connectionString}/order//order/detail/" + OrderID);
               
                try
                {
                    using var client = new HttpClient();
                    var response = await client.GetAsync(uri);


                    // Check if the upload was successful
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        OrderD = JsonConvert.DeserializeObject<OrderDetails>(json);
                        

                   
                    }
                    else { MessageBox.Show($"Fail To Call Data"); }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
        }


    }
}


