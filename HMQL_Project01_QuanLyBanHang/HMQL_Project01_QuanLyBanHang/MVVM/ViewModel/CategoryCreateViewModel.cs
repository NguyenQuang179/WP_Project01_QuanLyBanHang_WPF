﻿using HMQL_Project01_QuanLyBanHang.Core;
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
           

            ConfirmNewCategoryData = new RelayCommand(async o =>
            {
                string name = CategoryName;
             
                if (CategoryName != null)
                {
                    try
                    {
                        var uri = new Uri($"{ConnectionString.connectionString}/category/add/");
                        var client = new HttpClient();
                        CategoryName newCategory = new CategoryName();
                        newCategory.name = categoryName;
                        var jsonSended = JsonConvert.SerializeObject(newCategory);
                        var content = new StringContent(jsonSended, Encoding.UTF8, "application/json");
                        // Send the request and get the response
                        var response = await client.PostAsync(uri, content);
                        // Check if the upload was successful
                        if (response.IsSuccessStatusCode)
                        {
                            // Handle the successful upload
                            var json = await response.Content.ReadAsStringAsync();
                            MessageBox.Show("Category Created Successfully");
                            //add count for page
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
                    MessageBox.Show("Bạn phải nhập tên thể loại.");
                }
            });




            CancelCommand = new RelayCommand(o =>
            {
                MainVM.CategoryManagementVM.CategoryCreateVM = null;
                MainVM.CurrentView = MainVM.CategoryManagementVM;
            });



        }
    }
}
