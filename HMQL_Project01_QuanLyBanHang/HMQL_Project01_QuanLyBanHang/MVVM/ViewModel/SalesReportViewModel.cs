using HMQL_Project01_QuanLyBanHang.Core;
using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace HMQL_Project01_QuanLyBanHang.MVVM.ViewModel
{
    class SalesReportViewModel : ObservableObject
    {
        //Sale Series
        public SeriesCollection SaleSeries { get; set; }
        public string[] SaleLabels { get; set; }
        public Func<double, string> SaleFormatter { get; set; }

        //Product Series
        public SeriesCollection ProductSeries { get; set; }
        public string[] ProductLabels { get; set; }
        public Func<double, string> ProductFormatter { get; set; }

        public SalesReportViewModel() {
            SaleSeries = new SeriesCollection() { };
            SaleSeries.Add(new LineSeries
            {
                Title = "Sales",
                Values = new ChartValues<double> { 2, 4, 8, 1, 10 }
            });
            SaleLabels = new[] { "01/04", "02/04", "03/04", "04/04", "05/04" };
            SaleFormatter = value => value.ToString("N");

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
