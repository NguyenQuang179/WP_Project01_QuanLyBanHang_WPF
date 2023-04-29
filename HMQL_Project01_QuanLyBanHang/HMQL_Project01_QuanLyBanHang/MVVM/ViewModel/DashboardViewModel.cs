using HMQL_Project01_QuanLyBanHang.Core;
using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using HMQL_Project01_QuanLyBanHang.MVVM.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Net.Http;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace HMQL_Project01_QuanLyBanHang.MVVM.ViewModel
{
    class DashboardViewModel : ObservableObject
    {
        private DashboardDataModel data;
        public DashboardDataModel Data
        {
            get => data;
            set
            {
                data = value;
                OnPropertyChanged(nameof(Data));
            }
        }

        private bool monthOrderIsSelected;
        public bool MonthOrderIsSelected
        {
            get => monthOrderIsSelected;
            set
            {
                monthOrderIsSelected = value;
                OnPropertyChanged(nameof(MonthOrderIsSelected));
            }
        }

        private bool weekOrderIsSelected;
        public bool WeekOrderIsSelected
        {
            get => weekOrderIsSelected;
            set
            {
                weekOrderIsSelected = value;
                OnPropertyChanged(nameof(WeekOrderIsSelected));
            }
        }

        private int numOrder;
        public int NumOrder
        {
            get => numOrder;
            set
            {
                numOrder = value;
                OnPropertyChanged(nameof(NumOrder));
            }
        }

        public RelayCommand ChooseWeek { get; set; }
        public RelayCommand ChooseMonth { get; set; }
        public RelayCommand ProductViewCommand { get; set; }
        public RelayCommand CategoryViewCommand { get; set; }

        // COLUMN CHART
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

        public RelayCommand CallData { get; set; }

        public RelayCommand SalesReportCommand { get; set; }

        public DashboardViewModel(MainViewModel MainVM)
        {

            // COLUMN
            SeriesCollection = new SeriesCollection() { };

            //adding series will update and animate the chart automatically
            SeriesCollection.Add(new ColumnSeries
            {
                Title = "Order",

                Values = new ChartValues<double> { 10, 50, 39, 50 }
            });

            //also adding values updates and animates the chart automatically
            SeriesCollection[0].Values.Add(48d);

            Labels = new[] { "01/04", "02/04", "03/04", "04/04", "05/04" };
            Formatter = value => value.ToString("N");

            var origin = new ObservableCollection<Top5Book>() {
                new Top5Book() { id="001", title="Book 01", author="Author 01", stock=4 },
                new Top5Book() { id="002", title="Book 02", author="Author 02", stock=3 },
                new Top5Book() { id="003", title="Book 03", author="Author 03", stock=2 },
                new Top5Book() { id="004", title="Book 04", author="Author 04", stock=1 },
                new Top5Book() { id="005", title="Book 05", author="Author 05", stock=4 },
                new Top5Book() { id="006", title="Book 01", author="Author 01", stock=4 },
                new Top5Book() { id="007", title="Book 02", author="Author 02", stock=3 },
                new Top5Book() { id="008", title="Book 03", author="Author 03", stock=2 },
                new Top5Book() { id="009", title="Book 04", author="Author 04", stock=1 },
                new Top5Book() { id="010", title="Book 05", author="Author 05", stock=4 },
            };

            //Top5Books = origin.ToList();

            CallData = new RelayCommand(async o =>
            {
                var uri = new Uri($"{ConnectionString.connectionString}/dashboard");

                try
                {
                    using var client = new HttpClient();
                    var response = await client.GetAsync(uri);

                    // Check if the upload was successful
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        Data = JsonConvert.DeserializeObject<DashboardDataModel>(json);
                        ChooseWeek.Execute(null);
                        // Handle the successful upload
                        //MessageBox.Show($"Success Call Data {Data.listOfBookOutOfStock.Count}");
                    }
                    else { 
                        //MessageBox.Show($"Fail To Call Data");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });

            CallData.Execute(null);

            ChooseWeek = new RelayCommand(o =>
            {
                MonthOrderIsSelected = false;
                WeekOrderIsSelected = true;
                NumOrder = Data.numOfOrderThisWeek;
                //MessageBox.Show("Đã cập nhật số đơn hàng trong tuần");
            });

            ChooseMonth = new RelayCommand(o =>
            {
                WeekOrderIsSelected = false;
                MonthOrderIsSelected = true;
                NumOrder = Data.numOfOrderThisMonth;
                //MessageBox.Show("Đã cập nhật số đơn hàng trong tháng");
            });

            CategoryViewCommand = new RelayCommand(o => 
            {
                MainVM.CategoryManagementViewCommand.Execute(null);
            });

            ProductViewCommand = new RelayCommand(o =>
            {
                MainVM.ProductListViewCommand.Execute(null);
            });

            SalesReportCommand = new RelayCommand(o =>
            {
                MainVM.SalesReportViewCommand.Execute(null);
            });
        }
    }
}
