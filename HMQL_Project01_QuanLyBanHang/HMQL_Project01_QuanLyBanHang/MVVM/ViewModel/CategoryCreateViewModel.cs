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
    internal class CategoryCreateViewModel : ObservableObject
    {
        private ListBookCategory categoryD;
        public ListBookCategory CategoryD
        {
            get => categoryD;
            set
            {
                categoryD = value;
                OnPropertyChanged(nameof(CategoryD));
            }
        }

        private string categoryName;
        public string CategoryName
        {
            get { return categoryName; }
            set
            {
                categoryName = value;
                OnPropertyChanged(nameof(CategoryName));
            }
        }
        public RelayCommand CallOrderDetailData { get; set; }
        public RelayCommand OrderAddBookCreateViewCommand { get; set; }

        public RelayCommand ConfirmNewCategoryData { get; set; }
        public RelayCommand DeleteBookDetailData { get; set; }

        public RelayCommand CancelCommand { get; set; }
        public RelayCommand UpdatePageDataCommand { get; set; }

        public CategoryCreateViewModel(MainViewModel MainVM)
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

            ConfirmNewCategoryData = new RelayCommand(async o =>
            {
                string name = CategoryName;
                MessageBox.Show(CategoryName);
                ////Create Category Detail API
                //MessageBox.Show(CategoryD.listOfBook.Count.ToString());
                //NewListOfBook newListOfBook = new NewListOfBook();
                //foreach (var bookInfo in CategoryD.listOfBook)
                //{
                //    newListOfBook.listOfBook.Add(new BookInOrder(bookInfo.book._id));
                //}
                //MessageBox.Show(newListOfBook.listOfBook.Count.ToString());


                //try
                //{
                //    var uri = new Uri($"{ConnectionString.connectionString}/order/update/{CategoryD._id}");
                //    var client = new HttpClient();
                //    var jsonSended = JsonConvert.SerializeObject(newListOfBook);
                //    MessageBox.Show(jsonSended);
                //    var content = new StringContent(jsonSended, Encoding.UTF8, "application/json");
                //    // Send the request and get the response
                //    var response = await client.PutAsync(uri, content);
                //    // Check if the upload was successful
                //    if (response.IsSuccessStatusCode)
                //    {
                //        // Handle the successful upload
                //        var json = await response.Content.ReadAsStringAsync();

                //        MessageBox.Show(json);
                //        MainVM.OrderManagementVM.OrderDetailVM = null;
                //        MainVM.OrderManagementVM.UpdatePageDataCommand.Execute(null);
                //        MainVM.OrderManagementVM.TotalOrder++;
                //        MainVM.CurrentView = MainVM.OrderManagementVM;
                //    }
                //    else
                //    {
                //        // Handle the failed upload
                //        var json = await response.Content.ReadAsStringAsync();
                //        MessageBox.Show(json);
                //    }
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show(ex.Message);
                //}

                //Bỏ cái này vô sau khi chạy API thành công
                MainVM.CategoryManagementVM.CallData.Execute(null);
                MainVM.CurrentView = MainVM.CategoryManagementVM;
            });

           

            //OrderAddBookCreateViewCommand = new RelayCommand(o =>
            //{
            //    MainVM.OrderAddBookVM.IsAddBookForEditingOrder = true;
            //    MainVM.OrderAddBookViewCommand.Execute(MainVM);
            //});
            CancelCommand = new RelayCommand(o => {
                MainVM.CategoryManagementVM.CategoryCreateVM = null;
                MainVM.CurrentView = MainVM.CategoryManagementVM;
            });


            CallOrderDetailData = new RelayCommand(async o =>
            {
              
            });

            CallOrderDetailData.Execute(null);

            UpdatePageDataCommand = new RelayCommand(async (o) =>
            {
                
            });
        }
    }
}
