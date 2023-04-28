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
using System.Windows.Media.Media3D;

namespace HMQL_Project01_QuanLyBanHang.MVVM.ViewModel
{
    class SalesReportViewModel : ObservableObject
    {
        //Data From API For Chart
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
        private string dataMode;
        public string DataMode
        {
            get => dataMode;
            set
            {
                dataMode = value;
                OnPropertyChanged(nameof(DataMode));
            }
        }

        // Data From API For Data Grid
        private ListOfBookSales listBookReport;
        public ListOfBookSales ListBookReport
        {
            get => listBookReport;
            set
            {
                listBookReport = value;
                OnPropertyChanged(nameof(ListBookReport));
            }
        }
        private ListOfBookSalesWeek listBookReportWeek;
        public ListOfBookSalesWeek ListBookReportWeek
        {
            get => listBookReportWeek;
            set
            {
                listBookReportWeek = value;
                OnPropertyChanged(nameof(ListBookReportWeek));
            }
        }
        private List<BookSales> bookSales;
        public List<BookSales> BookSales
        {
            get => bookSales;
            set
            {
                bookSales = value;
                OnPropertyChanged(nameof(BookSales));
            }
        }
        private List<BookSalesWeek> bookSalesWeek;
        public List<BookSalesWeek> BookSalesWeek
        {
            get => bookSalesWeek;
            set
            {
                bookSalesWeek = value;
                OnPropertyChanged(nameof(BookSalesWeek));
            }
        }


        // Relay Command To Call Data From API
        public RelayCommand CallData { get; set; }
        
        // Value For Search Bar
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
        private string listBookDataMode;
        public string ListBookDataMode
        {
            get => listBookDataMode;
            set
            {
                listBookDataMode = value;
                OnPropertyChanged(nameof(ListBookDataMode));
            }
        }
        private string minDate;
        public string MinDate
        {
            get => minDate;
            set
            {
                minDate = value;
                OnPropertyChanged(nameof(MinDate));
            }
        }
        private string maxDate;
        public string MaxDate
        {
            get => maxDate;
            set
            {
                maxDate = value;
                OnPropertyChanged(nameof(MaxDate));
            }
        }
        // Paging 
        private int rowPerPage;
        public int RowPerPage
        {
            get => rowPerPage;
            set
            {
                rowPerPage = value;
               OnPropertyChanged(nameof(RowPerPage));
            }
        }
        private int totalPage;
        public int TotalPage
        {
            get => totalPage;
            set
            {
                totalPage = value;
                OnPropertyChanged(nameof(TotalPage));
            }
        }
        private int curPage;
        public int CurPage
        {
            get => curPage; 
            set
            {
                curPage = value;
                OnPropertyChanged(nameof(CurPage));
            }
        }
        private int totalBook;
        public int TotalBook
        {
            get => totalBook; 
            set
            {
                totalBook = value;
                OnPropertyChanged(nameof(TotalBook));
            }
        }

        // Data For Paging Combobox
        private List<int> listPages;
        public List<int> ListPages
        {
            get => listPages;
            set
            {
                listPages = value;
                OnPropertyChanged(nameof(ListPages));
            }
        }
        private int listPagesSelectedIndex;
        public int ListPagesSelectedIndex
        {
            get => listPagesSelectedIndex;
            set
            {
                listPagesSelectedIndex = value;
                OnPropertyChanged(nameof(ListPagesSelectedIndex));
            }
        }
        private List<BookSales> curPageData;
        public List<BookSales> CurPageData
        {
            get => curPageData;
            set
            {
                curPageData = value;
                OnPropertyChanged(nameof(CurPageData));
            }
        }

        //Relay Command To Update Data When Changing Page
        public RelayCommand UpdatePagingCommand { get; set; }
        public RelayCommand UpdatePageDataCommand { get; set; } 
        public RelayCommand PageComboboxChangeCommand { get; set; }
        public RelayCommand NextPageCommand { get; set; }
        public RelayCommand PrevPageCommand { get; set; }

        // Command To Search
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
            //Set Default Value
            CurPage = 1;
            ListPagesSelectedIndex = CurPage - 1;
            RowPerPage = 10;
            TotalBook = 0;
            TotalPage = 1;
            ListPages = new List<int>();
            CurPageData = new List<BookSales>();
            ListBookReport = new ListOfBookSales();
            ListBookDataMode = "Week";
            BookSales = new List<BookSales>();
            BookSalesWeek = new List<BookSalesWeek>();
            SearchValue = "";

            CallData = new RelayCommand(async o =>
            {
                var uri = new Uri($"{ConnectionString.connectionString}/order/report?minDate=2023-03-27&maxDate=2023-05-02&mode=day");
                var salesReportUri = new Uri($"{ConnectionString.connectionString}/book/report?minDate=2023-03-27&maxDate=2023-05-02&mode={ListBookDataMode.ToLower()}");
                try
                {
                    using var client = new HttpClient();
                    using var listClient = new HttpClient();
                    var response = await client.GetAsync(uri);
                    var listBookResponse = await listClient.GetAsync(salesReportUri);
                    // Check if the upload was successful
                    if (response.IsSuccessStatusCode && listBookResponse.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var listBookJson = await listBookResponse.Content.ReadAsStringAsync();
                        Data = JsonConvert.DeserializeObject<RootObject>(json);
                        ListBookReport = JsonConvert.DeserializeObject<ListOfBookSales>(listBookJson);
                        ListBookReportWeek = JsonConvert.DeserializeObject<ListOfBookSalesWeek>(listBookJson);
                        //MessageBox.Show($"{ListBookReport.saleReport.Count}");

                        // Update Paging
                        UpdatePagingCommand.Execute(null);
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

            UpdatePagingCommand = new RelayCommand(o =>
            {
                BookSalesWeek = ListBookReportWeek.saleReport.Where(s => s._id.book[0].name.Contains(SearchValue)).ToList();
                BookSales = ListBookReport.saleReport.Where(s => s._id.book[0].name.Contains(SearchValue)).ToList();
                TotalBook = BookSales.Count;
                TotalPage = TotalBook / RowPerPage + (TotalBook % RowPerPage == 0 ? 0 : 1);
                ListPages = new List<int>();
                for (int i = 1; i <= TotalPage; i++)
                {
                    ListPages.Add(i);
                }
                ListPagesSelectedIndex = CurPage - 1;
                UpdatePageDataCommand.Execute(null);
            });

            UpdatePageDataCommand = new RelayCommand(o =>
            {
                int starIndex = (CurPage - 1) * RowPerPage;
                if (CurPage < TotalPage)
                {
                    CurPageData = BookSales.GetRange(starIndex, RowPerPage);
                }
                else CurPageData = BookSales.GetRange(starIndex, TotalBook - ((TotalPage - 1) * RowPerPage));
            });

            PageComboboxChangeCommand = new RelayCommand(o =>
            {
                CurPage = ListPagesSelectedIndex + 1;
                UpdatePageDataCommand.Execute(null);
            });

            NextPageCommand = new RelayCommand(o =>
            {
                if(CurPage == TotalPage) { return; }
                ListPagesSelectedIndex++;
                CurPage++;
                UpdatePageDataCommand.Execute(null);
            });

            PrevPageCommand = new RelayCommand(o =>
            {
                if(CurPage == 1) { return; }
                ListPagesSelectedIndex--;
                CurPage--;
                UpdatePageDataCommand.Execute(null);
            });

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
