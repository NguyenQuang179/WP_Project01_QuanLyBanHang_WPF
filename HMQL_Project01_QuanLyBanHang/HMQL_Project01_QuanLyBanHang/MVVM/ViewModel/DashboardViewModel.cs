using HMQL_Project01_QuanLyBanHang.Core;
using HMQL_Project01_QuanLyBanHang.MVVM.Model;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace HMQL_Project01_QuanLyBanHang.MVVM.ViewModel
{
    class DashboardViewModel : ObservableObject
    {
        private List<Top5Book> top5Books;

        // COLUMN CHART
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

        public RelayCommand SalesReportCommand { get; set; }

        public RelayCommand ItemClickCommand { get; set; }

        public List<Top5Book> Top5Books
        {
            get => top5Books;
            set
            {
                top5Books = value;
                OnPropertyChanged(nameof(Top5Books));
            }
        }

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

            Top5Books = origin.ToList();

            ItemClickCommand = new RelayCommand((param) =>
            {
                string id = param.ToString();
                MessageBox.Show($"Item Clicked {id}");
            });

            SalesReportCommand = new RelayCommand(o =>
            {
                MainVM.SalesReportViewCommand.Execute(null);
            });
        }
    }
}
