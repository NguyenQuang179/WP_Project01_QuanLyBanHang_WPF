using HMQL_Project01_QuanLyBanHang.Core;
using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using HMQL_Project01_QuanLyBanHang.MVVM.Model;
using Newtonsoft.Json;
using System.Net.Http;
using System.Windows;

namespace HMQL_Project01_QuanLyBanHang.MVVM.ViewModel
{
    class SalesReportViewModel : ObservableObject
    {
        private RootObject data;
        public RootObject Data
        {
            get => data;
            set
            {
                data = value;
                OnPropertyChanged(nameof(Data));
            }
        }
        public RelayCommand CallData { get; set; }

        private string searchValue;
        public string SearchValue 
        { 
            get => searchValue;
            set {
                searchValue = value;
                OnPropertyChanged(nameof(SearchValue));
            }
        }
        public RelayCommand SearchCommand { get; set; }

        //Sale Series
        private SeriesCollection saleSeries;
        public SeriesCollection SaleSeries
        {
            get => saleSeries;
            set
            {
                saleSeries = value;
                OnPropertyChanged(nameof(SaleSeries));
            }
        }
        private List<string> saleLabels;
        public List<string> SaleLabels
        {
            get => saleLabels;
            set
            {
                saleLabels = value;
                OnPropertyChanged(nameof(SaleLabels));
            }
        }
        private Func<double, string> saleFormatter;
        public Func<double, string> SaleFormatter
        {
            get => saleFormatter;
            set
            {
                saleFormatter = value;
                OnPropertyChanged(nameof(SaleFormatter));
            }
        }

        //Product Series
        public SeriesCollection ProductSeries { get; set; }
        public string[] ProductLabels { get; set; }
        public Func<double, string> ProductFormatter { get; set; }

        public SalesReportViewModel()
        {
            CallData = new RelayCommand(async o =>
            {
                var uri = new Uri($"{ConnectionString.connectionString}/order/report?minDate=2023-03-27&maxDate=2023-05-02&mode=day");

                try
                {
                    using var client = new HttpClient();
                    var response = await client.GetAsync(uri);

                    // Check if the upload was successful
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        Data = JsonConvert.DeserializeObject<RootObject>(json);
                        // Handle the successful upload
                        //MessageBox.Show($"Success Call Data {Data.incomeReport[0]._id.date}");
                        SaleSeries = new SeriesCollection() { };
                        SaleSeries.Add(new LineSeries
                        {
                            Title = "Doanh thu",
                            Values = new ChartValues<double>()
                        });
                        SaleLabels = new List<string>();
                        for (int i = 0; i < Data.incomeReport.Count; i++)
                        {
                            SaleSeries[0].Values.Add(Data.incomeReport[i].totalIncome);
                            SaleLabels.Add(Data.incomeReport[i]._id.date);
                        }
                        SaleFormatter = value => value.ToString("N");
                    }
                    else { MessageBox.Show($"Fail To Call Data"); }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });

            CallData.Execute(null);

            SearchCommand = new RelayCommand(o =>
            {
                MessageBox.Show($"{SearchValue}");
            });

            ProductSeries = new SeriesCollection() { };
            ProductSeries.Add(new RowSeries
            {
                Title = "Number of product",
                Values = new ChartValues<double> { 2, 7, 12, 5, 8 }
            });
            ProductLabels = new[] { "New Book 1", "New Book 2", "New Book 3", "New Book 4", "New Book 5" };
            ProductFormatter = value => value.ToString("N");


        }
    }
}
