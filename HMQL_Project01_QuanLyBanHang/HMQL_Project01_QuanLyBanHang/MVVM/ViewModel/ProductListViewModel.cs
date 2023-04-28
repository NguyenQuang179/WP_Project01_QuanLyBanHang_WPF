using HMQL_Project01_QuanLyBanHang.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using HMQL_Project01_QuanLyBanHang.MVVM.Model;
using LiveCharts;
using LiveCharts.Wpf;
using Newtonsoft.Json;
using System.Net.Http;
using System.Windows;

namespace HMQL_Project01_QuanLyBanHang.MVVM.ViewModel
{
    internal class ProductListViewModel : ObservableObject
    {
        public RelayCommand SearchCommand { get; set; }
        public RelayCommand ApplySortCommand { get; set; }
        public RelayCommand AddBookCommand { get; set; }
        public RelayCommand DeleteBookCommand { get; set; }

        private ProductListDataModel data;

        public ProductListDataModel Data
        {
            get => data;
            set
            {
                data = value;
                OnPropertyChanged(nameof(Data));
            }
        }

        public RelayCommand CallDataCommand { get; set; }

        public RelayCommand ItemClickCommand { get; set; }

        public ProductViewModel ProductViewVM { get; set; }

        public ProductListViewModel(MainViewModel mainVM)
        {
            Data = new ProductListDataModel();
            CallDataCommand = new RelayCommand(async o =>
            {
                var uri = new Uri($"{ConnectionString.connectionString}/book/search");

                try
                {
                    using var client = new HttpClient();
                    var response = await client.GetAsync(uri);

                    // Check if the upload was successful
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        Data = JsonConvert.DeserializeObject<ProductListDataModel>(json);
                        // Handle the successful upload
                        //MessageBox.Show($"Success Call Data {Data.listOfBook.Count}");
                    }
                    else { MessageBox.Show($"Fail To Call Data"); }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
            CallDataCommand.Execute(null);

            ItemClickCommand = new RelayCommand((param) =>
            {
                string id = param.ToString();
                //MessageBox.Show($"Item Clicked {id}");

                //mainVM.ProductViewCommand.Execute(null);
                //ProductViewVM.CallDataFromListView.Execute(id);
                ProductViewVM = new ProductViewModel(mainVM, id);
                mainVM.CurrentView = ProductViewVM;
            });

            AddBookCommand = new RelayCommand(o =>
            {
                mainVM.ProductAddViewCommand.Execute(null);
            });
        }
    }
}