using HMQL_Project01_QuanLyBanHang.Core;
using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using HMQL_Project01_QuanLyBanHang.MVVM.Model;
using Newtonsoft.Json;
using System.Net.Http;
using System.Windows;

namespace HMQL_Project01_QuanLyBanHang.MVVM.ViewModel
{
    internal class SalesReportViewModel : ObservableObject
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

        private RootObjectMonthYear dataMonthYear;

        public RootObjectMonthYear DataMonthYear
        {
            get => dataMonthYear;
            set
            {
                dataMonthYear = value;
                OnPropertyChanged(nameof(DataMonthYear));
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

        private List<string> listDataMode;

        public List<string> ListDataMode
        {
            get => listDataMode;
            set
            {
                listDataMode = value;
                OnPropertyChanged(nameof(ListDataMode));
            }
        }

        private int selectedModeIndex;

        public int SelectedModeIndex
        {
            get => selectedModeIndex;
            set
            {
                selectedModeIndex = value;
                OnPropertyChanged(nameof(SelectedModeIndex));
                if (CallData != null) CallData.Execute(null);
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
                if (BookSales != null) PageComboboxChangeCommand.Execute(null);
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

        private int chartColumnSelectedModeIndex;

        public int ChartColumnSelectedModeIndex
        {
            get => chartColumnSelectedModeIndex;
            set
            {
                chartColumnSelectedModeIndex = value;
                OnPropertyChanged(nameof(ChartColumnSelectedModeIndex));
                if (CallData != null) CallData.Execute(null);
            }
        }

        private List<BookSales> top5Book;

        public List<BookSales> Top5Book
        {
            get => top5Book;
            set
            {
                top5Book = value;
                OnPropertyChanged(nameof(Top5Book));
            }
        }

        //Product Series
        private SeriesCollection productSeries;

        public SeriesCollection ProductSeries
        {
            get => productSeries;
            set
            {
                productSeries = value;
                OnPropertyChanged(nameof(ProductSeries));
            }
        }

        private List<string> productLabels;

        public List<string> ProductLabels
        {
            get => productLabels;
            set
            {
                productLabels = value;
                OnPropertyChanged(nameof(ProductLabels));
            }
        }

        private Func<double, string> productFormatter;

        public Func<double, string> ProductFormatter
        {
            get => productFormatter;
            set
            {
                productFormatter = value;
                OnPropertyChanged(nameof(ProductFormatter));
            }
        }

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
            ListDataMode = new List<string> { "Day", "Week", "Month", "Year" };
            SelectedModeIndex = 0;
            ChartColumnSelectedModeIndex = 0;
            BookSales = new List<BookSales>();
            SearchValue = "";
            MinDate = "";
            MaxDate = DateTime.Now.ToString("MM/dd/yyyy");

            CallData = new RelayCommand(async o =>
            {
                var uri = new Uri($"{ConnectionString.connectionString}/order/report?minDate={MinDate}&maxDate={MaxDate}&mode={ListDataMode[ChartColumnSelectedModeIndex].ToLower()}");
                var salesReportUri = new Uri($"{ConnectionString.connectionString}/book/report?minDate={MinDate}&maxDate={MaxDate}&mode={ListDataMode[selectedModeIndex].ToLower()}");
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
                        if (ChartColumnSelectedModeIndex == 0 || ChartColumnSelectedModeIndex == 1)
                        {
                            Data = JsonConvert.DeserializeObject<RootObject>(json);
                            SaleSeries = new SeriesCollection() { };
                            SaleSeries.Add(new ColumnSeries
                            {
                                Title = "Doanh thu",
                                Values = new ChartValues<double>()
                            });
                            SaleLabels = new List<string>();
                            for (int i = 0; i < Data.incomeReport.Count; i++)
                            {
                                SaleSeries[0].Values.Add(Data.incomeReport[i].totalIncome);
                                if (ChartColumnSelectedModeIndex == 1) Data.incomeReport[i]._id.date = $"{Data.incomeReport[i]._id.week} - {Data.incomeReport[i]._id.year}";
                                SaleLabels.Add(Data.incomeReport[i]._id.date);
                            }
                            SaleFormatter = value => value.ToString("N");
                        }
                        else
                        {
                            DataMonthYear = JsonConvert.DeserializeObject<RootObjectMonthYear>(json);
                            SaleSeries = new SeriesCollection() { };
                            SaleSeries.Add(new ColumnSeries
                            {
                                Title = "Doanh thu",
                                Values = new ChartValues<double>()
                            });
                            SaleLabels = new List<string>();
                            for (int i = 0; i < DataMonthYear.incomeReport.Count; i++)
                            {
                                SaleSeries[0].Values.Add(DataMonthYear.incomeReport[i].totalIncome);
                                SaleLabels.Add(DataMonthYear.incomeReport[i]._id);
                            }
                            SaleFormatter = value => value.ToString("N");
                        }
                        ListBookReport = JsonConvert.DeserializeObject<ListOfBookSales>(listBookJson);
                        ListBookReport.saleReport.RemoveAll(s => s._id.book.Count == 0);
                        Top5Book = (from book in ListBookReport.saleReport
                                    orderby book.totalQuantity descending
                                    select book).Take(5).ToList<BookSales>();
                        ProductSeries = new SeriesCollection() { };
                        ProductSeries.Add(new RowSeries
                        {
                            Title = "Sold Quantity",
                            Values = new ChartValues<double>()
                        });
                        ProductLabels = new List<string>();
                        for (int i = 0; i < Top5Book.Count; i++)
                        {
                            ProductSeries[0].Values.Add((double)Top5Book[i].totalQuantity);
                            ProductLabels.Add(Top5Book[i]._id.book[0].name);
                        }
                        ProductFormatter = value => value.ToString("N");

                        // Update Paging
                        UpdatePagingCommand.Execute(null);
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
                BookSales = ListBookReport.saleReport.Where(s => s._id.book[0].name.Contains(SearchValue)).ToList();
                for (int i = 0; i < BookSales.Count; i++)
                {
                    if (BookSales[i]._id.date == null)
                    {
                        BookSales[i]._id.date = $"Week {BookSales[i]._id.week} - In {BookSales[i]._id.year}";
                    }
                }
                CurPage = 1;
                TotalBook = BookSales.Count;
                TotalPage = TotalBook / RowPerPage + (TotalBook % RowPerPage == 0 ? 0 : 1);
                ListPagesSelectedIndex = CurPage - 1;
                var pages = new List<int>();
                for (int i = 1; i <= TotalPage; i++)
                {
                    pages.Add(i);
                }
                if (pages.Count > 0)
                {
                    ListPages = pages;
                }
                else ListPages.Clear();
                UpdatePageDataCommand.Execute(null);
            });

            UpdatePageDataCommand = new RelayCommand(o =>
            {
                int starIndex = (CurPage - 1) * RowPerPage;
                if (CurPage < TotalPage)
                {
                    CurPageData = BookSales.GetRange(starIndex, RowPerPage);
                }
                else
                {
                    if (TotalBook == 0) CurPageData = BookSales.GetRange(starIndex, 0);
                    else CurPageData = BookSales.GetRange(starIndex, TotalBook - ((TotalPage - 1) * RowPerPage));
                }
            });

            PageComboboxChangeCommand = new RelayCommand(o =>
            {
                CurPage = ListPagesSelectedIndex + 1;
                UpdatePageDataCommand.Execute(null);
            });

            NextPageCommand = new RelayCommand(o =>
            {
                if (CurPage == TotalPage) { return; }
                ListPagesSelectedIndex++;
                UpdatePageDataCommand.Execute(null);
            });

            PrevPageCommand = new RelayCommand(o =>
            {
                if (CurPage == 1) { return; }
                ListPagesSelectedIndex--;
                UpdatePageDataCommand.Execute(null);
            });

            SearchCommand = new RelayCommand(o =>
            {
                MessageBox.Show($"{SearchValue}");
            });
        }
    }
}