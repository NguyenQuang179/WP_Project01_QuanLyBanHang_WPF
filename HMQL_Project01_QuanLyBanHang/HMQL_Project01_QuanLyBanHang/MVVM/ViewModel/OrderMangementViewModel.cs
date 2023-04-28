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
using System.Security.Policy;

namespace HMQL_Project01_QuanLyBanHang.MVVM.ViewModel
{

    class OrderMangementViewModel : ObservableObject
    {
        public string id { get; set; }
        public RelayCommand OrderDetailViewCommand { get; set; }

        public OrderDetailViewModel? OrderDetailVM { get; set; }

        public RelayCommand DeleteOrderDataCommand { get; set; }
        public RelayCommand OrderCreateViewCommand { get; set; }
        public OrderCreateViewModel OrderCreateVM { get; set; }

        public String SelectedOrderID { get; set; }
        public Order SelectedOrder { get; set; }

        private ListOfOrder orders;
        public ListOfOrder Orders
        {
            get => orders;
            set
            {
                orders = value;
                OnPropertyChanged(nameof(Orders));
            }
        }

        public RelayCommand CallOrderData;



        //Commanda Page
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
        private int totalOrder;
        public int TotalOrder
        {
            get => totalOrder;
            set
            {
                totalOrder = value;
                OnPropertyChanged(nameof(TotalOrder));
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
                if (Orders.listOfOrder != null) PageComboboxChangeCommand.Execute(null);
            }
        }
        private ListOfOrder curPageData;
        public ListOfOrder CurPageData
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
        public RelayCommand UpdatePagingBaseOnDateCommand { get; private set; }
        public RelayCommand UpdatePageDataCommand { get; set; }
        public RelayCommand PageComboboxChangeCommand { get; set; }
        public RelayCommand NextPageCommand { get; set; }
        public RelayCommand PrevPageCommand { get; set; }

        public OrderMangementViewModel(MainViewModel MainVM)
        {
            orders = new ListOfOrder();

            SelectedOrder = null;
            OrderDetailVM = null;

            //Set Default Value
            CurPage = 1;
            ListPagesSelectedIndex = CurPage - 1;
            RowPerPage = 10;
            TotalOrder = 0;
            TotalPage = 1;
            ListPages = new List<int>();
            //CurPageData = new List<BookSales>();
            //ListBookReport = new ListOfBookSales();
            //ListDataMode = new List<string> { "Day", "Week", "Month", "Year" };
            //SelectedModeIndex = 0;
            //BookSales = new List<BookSales>();
            //SearchValue = "";
            MinDate = "";
            MaxDate = DateTime.Now.ToString("MM/dd/yyyy");


            DeleteOrderDataCommand = new RelayCommand(async (param) =>
            {
                MessageBox.Show("Hello");
                List<Order> curList = Orders.listOfOrder;
                // Find the index of the book to remove

                string orderId = param as string;
                if (orderId == null)
                {
                    //show error
                    MessageBox.Show("Invalid Order ID");
                    return;
                }
                // Find the index of the book to remove
                int indexToRemove = curList.FindIndex(b => b._id == orderId);


                if (indexToRemove != -1)
                {
                    // Remove the book from curList
                    using var client = new HttpClient();
                    var uri = new Uri($"{ConnectionString.connectionString}/order/delete/{orderId}");
                    var response = await client.DeleteAsync(uri);

                    // Check if the upload was successful
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        MessageBox.Show(json);
                        //MessageBox.Show($"{Orders.listOfOrder.Count} {newdate}");
                    }
                    else { MessageBox.Show($"Fail To Delete Order"); }
                    TotalOrder--;
                    UpdatePageDataCommand.Execute(null);

                }
                else
                {
                    //show error
                    MessageBox.Show("No Order Selected");
                }
            });

            OrderDetailViewCommand = new RelayCommand((param) =>
            {
                if (param != null) { 
                    id = param.ToString();
                MessageBox.Show("ID IS:" + id);
                OrderDetailVM = new OrderDetailViewModel(MainVM, id);
                //MessageBox.Show("No Selected Order");

                MainVM.CurrentView = OrderDetailVM;
                }
            });

            OrderCreateViewCommand = new RelayCommand(o =>
            {
                OrderCreateVM = new OrderCreateViewModel(MainVM);
                MainVM.CurrentView = OrderCreateVM;
            });

            CallOrderData = new RelayCommand(async o =>
            {
                var uri = new Uri($"{ConnectionString.connectionString}/order/search?&itemPerPage=10000000");
                //add count for page
                try
                {
                    using var client = new HttpClient();
                    var response = await client.GetAsync(uri);


                    // Check if the upload was successful
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        Orders = JsonConvert.DeserializeObject<ListOfOrder>(json);
                        TotalOrder = Orders.listOfOrder.Count;
                        //string newdate = "";
                        //DateTime datetime = DateTime.Now;
                        //for (int i = 0; i < Orders.listOfOrder.Count; i++)
                        //{
                        //    datetime = DateTime.Parse(Orders.listOfOrder[i].date);
                        //    newdate = datetime.ToString("dd/MM/y hh:mm tt");
                        //    Orders.listOfOrder[i].date = newdate;
                        //}
                        //MessageBox.Show($"{Orders.listOfOrder.Count} {newdate}");

                        UpdatePagingCommand.Execute(null);
                    }
                    else { MessageBox.Show($"Fail To Call Data"); }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });


            CallOrderData.Execute(null);

            //Page Division
            UpdatePagingCommand = new RelayCommand(o =>
            {
             
                //BookSales = ListBookReport.saleReport.Where(s => s._id.book[0].name.Contains(SearchValue)).ToList();
                //for (int i = 0; i < BookSales.Count; i++)
                //{
                //    if (BookSales[i]._id.date == null)
                //    {
                //        BookSales[i]._id.date = $"Week {BookSales[i]._id.week} - In {BookSales[i]._id.year}";
                //    }
                //}
                CurPage = 1;
              
                TotalPage = TotalOrder / RowPerPage + (TotalOrder % RowPerPage == 0 ? 0 : 1);
                ListPagesSelectedIndex = CurPage - 1;
                var pages = new List<int>();
                for (int i = 1; i <= TotalPage; i++)
                {
                    pages.Add(i);
                }
                ListPages = pages;
                UpdatePageDataCommand.Execute(null);
            });

            UpdatePagingBaseOnDateCommand = new RelayCommand(async(o) =>
            {

                //BookSales = ListBookReport.saleReport.Where(s => s._id.book[0].name.Contains(SearchValue)).ToList();
                //for (int i = 0; i < BookSales.Count; i++)
                //{
                //    if (BookSales[i]._id.date == null)
                //    {
                //        BookSales[i]._id.date = $"Week {BookSales[i]._id.week} - In {BookSales[i]._id.year}";
                //    }
                //}

                var uri = new Uri($"{ConnectionString.connectionString}/order/search?minDate={MinDate}&maxDate={MaxDate}&page={CurPage}&itemPerPage=10000000");
                //int starIndex = (CurPage - 1) * RowPerPage;
                //if (CurPage < TotalPage)
                //{
                //    CurPageData = BookSales.GetRange(starIndex, RowPerPage);
                //}
                //else
                //{
                //    if (TotalBook == 0) CurPageData = BookSales.GetRange(starIndex, 0);
                //    else CurPageData = BookSales.GetRange(starIndex, TotalBook - ((TotalPage - 1) * RowPerPage));
                //}
                //add count for page
                try
                {
                    using var client = new HttpClient();
                    var response = await client.GetAsync(uri);

                    // Check if the upload was successful
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        Orders = JsonConvert.DeserializeObject<ListOfOrder>(json);
                        string newdate = "";
                        DateTime datetime = DateTime.Now;
                        for (int i = 0; i < Orders.listOfOrder.Count; i++)
                        {
                            datetime = DateTime.Parse(Orders.listOfOrder[i].date);
                            newdate = datetime.ToString("dd/MM/y hh:mm tt");
                            Orders.listOfOrder[i].date = newdate;
                        }
                        //MessageBox.Show($"{Orders.listOfOrder.Count} {newdate}");
                    }
                    else { MessageBox.Show($"Fail To Call Data"); }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                CurPage = 1;
                TotalOrder = Orders.listOfOrder.Count;
                TotalPage = TotalOrder / RowPerPage + (TotalOrder % RowPerPage == 0 ? 0 : 1);
                ListPagesSelectedIndex = CurPage - 1;
                var pages = new List<int>();
                for (int i = 1; i <= TotalPage; i++)
                {
                    pages.Add(i);
                }
                ListPages = pages;
                UpdatePageDataCommand.Execute(null);
            });

            UpdatePageDataCommand = new RelayCommand(async (o) =>
            {
                var uri = new Uri($"{ConnectionString.connectionString}/order/search?minDate={MinDate}&maxDate={MaxDate}&page={CurPage}&itemPerPage={RowPerPage}");
                //int starIndex = (CurPage - 1) * RowPerPage;
                //if (CurPage < TotalPage)
                //{
                //    CurPageData = BookSales.GetRange(starIndex, RowPerPage);
                //}
                //else
                //{
                //    if (TotalBook == 0) CurPageData = BookSales.GetRange(starIndex, 0);
                //    else CurPageData = BookSales.GetRange(starIndex, TotalBook - ((TotalPage - 1) * RowPerPage));
                //}
                //add count for page
                try
                {
                    using var client = new HttpClient();
                    var response = await client.GetAsync(uri);


                    // Check if the upload was successful
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        Orders = JsonConvert.DeserializeObject<ListOfOrder>(json);
                        string newdate = "";
                        DateTime datetime = DateTime.Now;
                        for (int i = 0; i < Orders.listOfOrder.Count; i++)
                        {
                            datetime = DateTime.Parse(Orders.listOfOrder[i].date);
                            newdate = datetime.ToString("dd/MM/y hh:mm tt");
                            Orders.listOfOrder[i].date = newdate;
                        }
                        //MessageBox.Show($"{Orders.listOfOrder.Count} {newdate}");
                    }
                    else { MessageBox.Show($"Fail To Call Data"); }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
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
        }
    }
}