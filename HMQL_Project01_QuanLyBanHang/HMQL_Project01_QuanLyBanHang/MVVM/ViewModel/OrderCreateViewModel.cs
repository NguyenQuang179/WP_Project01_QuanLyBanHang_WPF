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
using System.Net.Mail;

namespace HMQL_Project01_QuanLyBanHang.MVVM.ViewModel
{

    class OrderCreateViewModel : ObservableObject
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

        private long bookQuantity;
        public long BookQuantity
        {
            get { return bookQuantity; }
            set
            {
                bookQuantity = value;
                OnPropertyChanged(nameof(bookQuantity));
            }
        }
        public RelayCommand CancelCommand { get; set; }

        public RelayCommand CallOrderDetailData { get; set; }
        public RelayCommand OrderAddBookCreateViewCommand { get; set; }

        public RelayCommand ConfirmOrderDetailData { get; set; }
        public RelayCommand DeleteBookDetailData { get; set; }

        public OrderCreateViewModel(MainViewModel MainVM)
        {
            void UpdateBookQuantityCommand(List<BookInOrderForDetails> curList)
            {

                int count = 0;
                foreach(var books in curList)
                {
                    count += books.quantity;
                }
                BookQuantity = count;
            }

            ConfirmOrderDetailData = new RelayCommand(async o =>
            {
                //Create Order Detail API

                //MessageBox.Show(OrderD.order.listOfBook.Count.ToString());
                NewListOfBook newListOfBook = new NewListOfBook();
                foreach (var bookInfo in OrderD.order.listOfBook)
                {
                    newListOfBook.listOfBook.Add(new BookInOrder(bookInfo.book._id, bookInfo.quantity));
                }
                //MessageBox.Show(newListOfBook.listOfBook.Count.ToString());


                try
                {
                    var uri = new Uri($"{ConnectionString.connectionString}/order/add/");
                    var client = new HttpClient();
                    var jsonSended = JsonConvert.SerializeObject(newListOfBook);
                    //MessageBox.Show(jsonSended);
                    var content = new StringContent(jsonSended, Encoding.UTF8, "application/json");
                    // Send the request and get the response
                    var response = await client.PostAsync(uri, content);
                    // Check if the upload was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Handle the successful upload
                        var json = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Order Create Successfully");

                        MainVM.OrderManagementVM.OrderCreateVM = null;
                        MainVM.OrderManagementVM.UpdatePageDataCommand.Execute(null);
                        MainVM.OrderManagementVM.TotalOrder++;
                        MainVM.CurrentView = MainVM.OrderManagementVM;
                        //add count for page
                       
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

            CancelCommand = new RelayCommand(o => {
                MainVM.OrderManagementVM.OrderCreateVM = null;
                MainVM.CurrentView = MainVM.OrderManagementVM;
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
                    // Remove the book from curList
                    TotalPrice = TotalPrice - (curList[indexToRemove].quantity * curList[indexToRemove].book.price);
                    curList.RemoveAt(indexToRemove);
                    UpdateBookQuantityCommand(curList);
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
                MainVM.OrderAddBookVM.IsAddBookForEditingOrder = false;
                
                MainVM.OrderAddBookViewCommand.Execute(MainVM);
            });

            CallOrderDetailData = new RelayCommand(async o =>
            {
                //Get Data
                OrderD = new OrderDetails();
                OrderD.order.totalPrice = 0;
                TotalPrice = totalPrice;

            });

            CallOrderDetailData.Execute(null);


        }

    }

}


