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
    internal class CategoryDetailViewModel : ObservableObject
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
        private string searchValue;
        public string SearchValue
        {
            get => searchValue;
            set
            {
                searchValue = value;
                OnPropertyChanged(nameof(SearchValue));
            }
        }
        public RelayCommand CallOrderDetailData { get; set; }
        public RelayCommand OrderAddBookCreateViewCommand { get; set; }

        public RelayCommand ConfirmCategoryDetailData { get; set; }
        public RelayCommand DeleteBookDetailData { get; set; }

        public RelayCommand CancelCommand { get; set; }
        public RelayCommand UpdatePageDataCommand { get; set; }

        public CategoryDetailViewModel(MainViewModel MainVM, String CategoryID)
        {
           

            ConfirmCategoryDetailData = new RelayCommand(async o =>
            {
                string name = CategoryName;
                //MessageBox.Show(CategoryName);
                if (categoryName != null)
                {
                    try
                    {
                        var uri = new Uri($"{ConnectionString.connectionString}/category/update/{CategoryID}");
                        var client = new HttpClient();
                        CategoryName newCategory = new CategoryName();
                        newCategory.name = categoryName;
                        var jsonSended = JsonConvert.SerializeObject(newCategory);   
                        var content = new StringContent(jsonSended, Encoding.UTF8, "application/json");
                        // Send the request and get the response
                        var response = await client.PutAsync(uri, content);
                        // Check if the upload was successful
                        if (response.IsSuccessStatusCode)
                        {
                            // Handle the successful upload
                            var json = await response.Content.ReadAsStringAsync();
                            MessageBox.Show("Category Edit Successfully");
                            MainVM.CategoryManagementVM.CallData.Execute(null);
                            MainVM.CurrentView = MainVM.CategoryManagementVM;
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
                    
                }
                else
                {
                    MessageBox.Show("Bạn phải nhập tên thể loại mới!");
                }
            });

            DeleteBookDetailData = new RelayCommand((param) =>
            {
                List<BookDetail> curList = CategoryD.listOfBook;
                // Find the index of the book to remove

                string bookId = param as string;
                if (bookId == null)
                {
                    //show error
                    MessageBox.Show("Invalid Book ID");
                    return;
                }
                // Find the index of the book to remove
                int indexToRemove = curList.FindIndex(b => b._id == bookId);


                if (indexToRemove != -1)
                {
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
            CancelCommand = new RelayCommand(o =>
            {
                MainVM.CategoryManagementVM.CategoryDetailVM = null;
                MainVM.CurrentView = MainVM.CategoryManagementVM;
            });


            CallOrderDetailData = new RelayCommand(async o =>
            {
                var uri = new Uri($"{ConnectionString.connectionString}/category/showbook/{CategoryID}?name={SearchValue}");
                //add count for page
                try
                {
                    using var client = new HttpClient();
                    var response = await client.GetAsync(uri);


                    // Check if the upload was successful
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        CategoryD = JsonConvert.DeserializeObject<ListBookCategory>(json);
                        CategoryName = CategoryD.categoryDetail.name;

                    }
                    else { MessageBox.Show($"Fail To Call Data"); }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });

            CallOrderDetailData.Execute(null);

            UpdatePageDataCommand = new RelayCommand(async (o) =>
            {
                var uri = new Uri($"{ConnectionString.connectionString}/order//order/detail/" + CategoryID);
               
  
                try
                {
                    using var client = new HttpClient();
                    var response = await client.GetAsync(uri);


                    // Check if the upload was successful
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        CategoryD = JsonConvert.DeserializeObject<ListBookCategory>(json);
                        CategoryName = CategoryD.categoryDetail.name;

                        
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
