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

        public RelayCommand UpdateDataListCommand { get; set; }

        public RelayCommand UpdatePageDataCommand { get; set; }

        public ProductViewModel ProductViewVM { get; set; }

        public ProductAddViewModel ProductAddVM { get; set; }

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
                        //UpdatePagingCommand.Execute(null);
                    }
                    else { MessageBox.Show($"Fail To Call Data"); }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
            CallDataCommand.Execute(null);

            UpdateDataListCommand = new RelayCommand(o =>
            {
                var uri = new Uri($"{ConnectionString.connectionString}/book/?name={SearchValue}");

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
                        //UpdatePagingCommand.Execute(null);
                    }
                    else { MessageBox.Show($"Fail To Call Data"); }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                //BookSales = ListBookReport.saleReport.Where(s => s._id.book[0].name.Contains(SearchValue)).ToList();
                //for (int i = 0; i < BookSales.Count; i++)
                //{
                //    if (BookSales[i]._id.date == null)
                //    {
                //        BookSales[i]._id.date = $"Week {BookSales[i]._id.week} - In {BookSales[i]._id.year}";
                //    }
                //}
                //CurPage = 1;
                //TotalBook = BookSales.Count;
                //TotalPage = TotalBook / RowPerPage + (TotalBook % RowPerPage == 0 ? 0 : 1);
                //ListPagesSelectedIndex = CurPage - 1;
                //var pages = new List<int>();
                //for (int i = 1; i <= TotalPage; i++)
                //{
                //    pages.Add(i);
                //}
                //ListPages = pages;
                //UpdatePageDataCommand.Execute(null);
            });

            //UpdatePageDataCommand = new RelayCommand(o =>
            //{
            //    int starIndex = (CurPage - 1) * RowPerPage;
            //    if (CurPage < TotalPage)
            //    {
            //        CurPageData = BookSales.GetRange(starIndex, RowPerPage);
            //    }
            //    else
            //    {
            //        if (TotalBook == 0) CurPageData = BookSales.GetRange(starIndex, 0);
            //        else CurPageData = BookSales.GetRange(starIndex, TotalBook - ((TotalPage - 1) * RowPerPage));
            //    }
            //});

            ItemClickCommand = new RelayCommand((param) =>
            {
                if (param != null)
                {
                    string id = param.ToString();
                    //MessageBox.Show($"Item Clicked {id}");

                    //mainVM.ProductViewCommand.Execute(null);
                    //ProductViewVM.CallDataFromListView.Execute(id);
                    ProductViewVM = new ProductViewModel(mainVM, id, this);
                    mainVM.CurrentView = ProductViewVM;
                }
                else
                {
                    //Do nothing
                }
            });

            AddBookCommand = new RelayCommand(o =>
            {
                ProductAddVM = new ProductAddViewModel(mainVM, this);
                mainVM.CurrentView = ProductAddVM;
            });
        }
    }
}