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
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Win32;

namespace HMQL_Project01_QuanLyBanHang.MVVM.ViewModel
{
    internal class ProductListViewModel : ObservableObject
    {
        public RelayCommand SearchCommand { get; set; }
        public RelayCommand ApplySortCommand { get; set; }
        public RelayCommand AddBookCommand { get; set; }
        public RelayCommand DeleteBookCommand { get; set; }

        public RelayCommand EraseCommand { get; set; }

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

        public RelayCommand BackSpaceCommand { get; set; }

        public RelayCommand ApplySorting { get; set; }

        public ProductViewModel ProductViewVM { get; set; }

        public ProductAddViewModel ProductAddVM { get; set; }

        private string id { get; set; }

        public string Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        private string name { get; set; }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string author { get; set; }

        public string Author
        {
            get => author;
            set
            {
                author = value;
                OnPropertyChanged(nameof(Author));
            }
        }

        private string stock { get; set; }

        public string Stock
        {
            get => stock;
            set
            {
                id = value;
                OnPropertyChanged(nameof(Stock));
            }
        }

        private List<Book> curPageData;

        public List<Book> CurPageData
        {
            get => curPageData;
            set
            {
                curPageData = value;
                OnPropertyChanged(nameof(CurPageData));
            }
        }

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

        private string priceFrom;

        public string PriceFrom
        {
            get => priceFrom;
            set
            {
                priceFrom = value;
                OnPropertyChanged(nameof(PriceFrom));
            }
        }

        private string priceTo;

        public string PriceTo
        {
            get => priceTo;
            set
            {
                priceTo = value;
                OnPropertyChanged(nameof(PriceTo));
            }
        }

        public bool isBackspaceProcessed = false;

        private string rowPerPage;

        public string RowPerPage
        {
            get => rowPerPage;
            set
            {
                rowPerPage = value;
                OnPropertyChanged(nameof(RowPerPage));
            }
        }

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

        private int totalPages;

        public int TotalPages
        {
            get => totalPages;
            set
            {
                totalPages = value;
                OnPropertyChanged(nameof(TotalPages));
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

        private int listPagesSelectedIndex;

        public int ListPagesSelectedIndex
        {
            get => listPagesSelectedIndex;
            set
            {
                listPagesSelectedIndex = value;
                OnPropertyChanged(nameof(ListPagesSelectedIndex));
                if (Data != null) PageComboboxChangeCommand.Execute(null);
            }
        }

        private string filePath;

        public string FilePath
        {
            get => filePath;
            set
            {
                filePath = value;
                OnPropertyChanged(nameof(FilePath));
            }
        }

        public RelayCommand UpdatePagingCommand { get; set; }
        public RelayCommand BrowseFileCommand { get; set; }
        public RelayCommand UploadFileCommand { get; set; }

        //public RelayCommand UpdatePagingCommand { get; set; }

        //public RelayCommand ListPagesSelectedIndex { get; set; }

        public RelayCommand PageComboboxChangeCommand { get; set; }

        public RelayCommand PrevPageCommand { get; set; }

        public RelayCommand NextPageCommand { get; set; }

        public ProductListViewModel(MainViewModel mainVM)
        {
            CurPageData = new List<Book>();
            Data = new ProductListDataModel();
            RowPerPage = "10";
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

                    CurPage = 1;
                    TotalPages = Data.numOfPage;
                    TotalBook = Data.numOfBooks;
                    var pages = new List<int>();
                    for (int i = 1; i <= TotalPages; i++)
                    {
                        pages.Add(i);
                    }
                    ListPages = pages;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
            CallDataCommand.Execute(null);

            EraseCommand = new RelayCommand(o =>
            {
                SearchValue = "";
                CallDataCommand.Execute(null);
            });

            UpdatePagingCommand = new RelayCommand(async o =>
            {
                if (RowPerPage != "")
                {
                    string query_string = "";

                    if (SearchValue == "")
                    {
                        if (PriceFrom != "" && PriceTo != "")
                        {
                            query_string = $"{ConnectionString.connectionString}/book/search?minPrice={PriceFrom}&maxPrice={PriceTo}&page={CurPage}&itemPerPage={RowPerPage}";
                        }
                        else if (PriceFrom != "" && PriceTo == "")
                        {
                            query_string = $"{ConnectionString.connectionString}/book/search?minPrice={PriceFrom}&page={CurPage}&itemPerPage={RowPerPage}";
                        }
                        else if (PriceFrom == "" && PriceTo != "")
                        {
                            query_string = $"{ConnectionString.connectionString}/book/search?mmaxPrice={PriceTo}&page={CurPage}&itemPerPage={RowPerPage}";
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (PriceFrom != "" && PriceTo != "")
                        {
                            query_string = $"{ConnectionString.connectionString}/book/search?name={SearchValue}&minPrice={PriceFrom}&maxPrice={PriceTo}&page={CurPage}&itemPerPage={RowPerPage}";
                        }
                        else if (PriceFrom != "" && PriceTo == "")
                        {
                            query_string = $"{ConnectionString.connectionString}/book/search?name={SearchValue}&minPrice={PriceFrom}&page={CurPage}&itemPerPage={RowPerPage}";
                        }
                        else if (PriceFrom == "" && PriceTo != "")
                        {
                            query_string = $"{ConnectionString.connectionString}/book/search?name={SearchValue}&maxPrice={PriceTo}&page={CurPage}&itemPerPage={RowPerPage}";
                        }
                        else
                        {
                            return;
                        }
                    }

                    var uri = new Uri(query_string);

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
                            TotalPages = Data.numOfPage;
                            TotalBook = Data.numOfBooks;
                            var pages = new List<int>();
                            for (int i = 1; i <= TotalPages; i++)
                            {
                                pages.Add(i);
                            }
                            ListPages = pages;
                        }
                        else { MessageBox.Show($"Fail To Call Data"); }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            });

            PageComboboxChangeCommand = new RelayCommand(async o =>
            {
                CurPage = ListPagesSelectedIndex + 1;
                //MessageBox.Show($"Current Page: {CurPage}");

                string query_string = "";

                if (SearchValue == "")
                {
                    if (PriceFrom != "" && PriceTo != "")
                    {
                        query_string = $"{ConnectionString.connectionString}/book/search?minPrice={PriceFrom}&maxPrice={PriceTo}&page={CurPage}&itemPerPage={RowPerPage}";
                    }
                    else if (PriceFrom != "" && PriceTo == "")
                    {
                        query_string = $"{ConnectionString.connectionString}/book/search?minPrice={PriceFrom}&page={CurPage}&itemPerPage={RowPerPage}";
                    }
                    else if (PriceFrom == "" && PriceTo != "")
                    {
                        query_string = $"{ConnectionString.connectionString}/book/search?mmaxPrice={PriceTo}&page={CurPage}&itemPerPage={RowPerPage}";
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    if (PriceFrom != "" && PriceTo != "")
                    {
                        query_string = $"{ConnectionString.connectionString}/book/search?name={SearchValue}&minPrice={PriceFrom}&maxPrice={PriceTo}&page={CurPage}&itemPerPage={RowPerPage}";
                    }
                    else if (PriceFrom != "" && PriceTo == "")
                    {
                        query_string = $"{ConnectionString.connectionString}/book/search?name={SearchValue}&minPrice={PriceFrom}&page={CurPage}&itemPerPage={RowPerPage}";
                    }
                    else if (PriceFrom == "" && PriceTo != "")
                    {
                        query_string = $"{ConnectionString.connectionString}/book/search?name={SearchValue}&maxPrice={PriceTo}&page={CurPage}&itemPerPage={RowPerPage}";
                    }
                    else
                    {
                        return;
                    }
                }

                var uri = new Uri(query_string);

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

            //UpdatePageDataCommand = new RelayCommand(async o =>
            //{
            //    var uri = new Uri($"{ConnectionString.connectionString}/book/search?page={CurPage}&itemPerPage={RowPerPage}");

            //    try
            //    {
            //        using var client = new HttpClient();
            //        var response = await client.GetAsync(uri);

            //        // Check if the upload was successful
            //        if (response.IsSuccessStatusCode)
            //        {
            //            var json = await response.Content.ReadAsStringAsync();
            //            Data = JsonConvert.DeserializeObject<ProductListDataModel>(json);
            //            // Handle the successful upload
            //            //MessageBox.Show($"Success Call Data {Data.listOfBook.Count}");
            //            //UpdatePagingCommand.Execute(null);
            //        }
            //        else { MessageBox.Show($"Fail To Call Data"); }
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //    }
            //});

            PrevPageCommand = new RelayCommand(async o =>
            {
                //MessageBox.Show($"{ConnectionString.connectionString}/book/search?page={CurPage - 1}");
                if (CurPage > 1)
                {
                    CurPage--;

                    string query_string = "";

                    if (SearchValue == "")
                    {
                        if (PriceFrom != "" && PriceTo != "")
                        {
                            query_string = $"{ConnectionString.connectionString}/book/search?minPrice={PriceFrom}&maxPrice={PriceTo}&page={CurPage}&itemPerPage={RowPerPage}";
                        }
                        else if (PriceFrom != "" && PriceTo == "")
                        {
                            query_string = $"{ConnectionString.connectionString}/book/search?minPrice={PriceFrom}&page={CurPage}&itemPerPage={RowPerPage}";
                        }
                        else if (PriceFrom == "" && PriceTo != "")
                        {
                            query_string = $"{ConnectionString.connectionString}/book/search?mmaxPrice={PriceTo}&page={CurPage}&itemPerPage={RowPerPage}";
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (PriceFrom != "" && PriceTo != "")
                        {
                            query_string = $"{ConnectionString.connectionString}/book/search?name={SearchValue}&minPrice={PriceFrom}&maxPrice={PriceTo}&page={CurPage}&itemPerPage={RowPerPage}";
                        }
                        else if (PriceFrom != "" && PriceTo == "")
                        {
                            query_string = $"{ConnectionString.connectionString}/book/search?name={SearchValue}&minPrice={PriceFrom}&page={CurPage}&itemPerPage={RowPerPage}";
                        }
                        else if (PriceFrom == "" && PriceTo != "")
                        {
                            query_string = $"{ConnectionString.connectionString}/book/search?name={SearchValue}&maxPrice={PriceTo}&page={CurPage}&itemPerPage={RowPerPage}";
                        }
                        else
                        {
                            return;
                        }
                    }

                    var uri = new Uri(query_string);

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
                        ListPagesSelectedIndex--;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            });

            NextPageCommand = new RelayCommand(async o =>
            {
                //MessageBox.Show($"{ConnectionString.connectionString}/book/search?page={CurPage + 1}");

                if (CurPage < TotalPages)
                {
                    CurPage++;

                    string query_string = "";

                    if (SearchValue == "")
                    {
                        if (PriceFrom != "" && PriceTo != "")
                        {
                            query_string = $"{ConnectionString.connectionString}/book/search?minPrice={PriceFrom}&maxPrice={PriceTo}&page={CurPage}&itemPerPage={RowPerPage}";
                        }
                        else if (PriceFrom != "" && PriceTo == "")
                        {
                            query_string = $"{ConnectionString.connectionString}/book/search?minPrice={PriceFrom}&page={CurPage}&itemPerPage={RowPerPage}";
                        }
                        else if (PriceFrom == "" && PriceTo != "")
                        {
                            query_string = $"{ConnectionString.connectionString}/book/search?mmaxPrice={PriceTo}&page={CurPage}&itemPerPage={RowPerPage}";
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (PriceFrom != "" && PriceTo != "")
                        {
                            query_string = $"{ConnectionString.connectionString}/book/search?name={SearchValue}&minPrice={PriceFrom}&maxPrice={PriceTo}&page={CurPage}&itemPerPage={RowPerPage}";
                        }
                        else if (PriceFrom != "" && PriceTo == "")
                        {
                            query_string = $"{ConnectionString.connectionString}/book/search?name={SearchValue}&minPrice={PriceFrom}&page={CurPage}&itemPerPage={RowPerPage}";
                        }
                        else if (PriceFrom == "" && PriceTo != "")
                        {
                            query_string = $"{ConnectionString.connectionString}/book/search?name={SearchValue}&maxPrice={PriceTo}&page={CurPage}&itemPerPage={RowPerPage}";
                        }
                        else
                        {
                            return;
                        }
                    }

                    var uri = new Uri(query_string);

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
                        ListPagesSelectedIndex++;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            });

            BrowseFileCommand = new RelayCommand(o =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "(.xlsx)|*.xlsx";
                string filepath = "";
                if (openFileDialog.ShowDialog() == true)
                {
                    filepath = openFileDialog.FileName;
                }

                FilePath = filepath;
            });

            ApplySortCommand = new RelayCommand(async o =>
            {
                MessageBox.Show($"{SearchValue} {PriceFrom} {PriceTo}");
                string query_string = "";

                if (SearchValue == "")
                {
                    if (PriceFrom != "" && PriceTo != "")
                    {
                        query_string = $"{ConnectionString.connectionString}/book/search?minPrice={PriceFrom}&maxPrice={PriceTo}&itemPerPage={RowPerPage}";
                    }
                    else if (PriceFrom != "" && PriceTo == "")
                    {
                        query_string = $"{ConnectionString.connectionString}/book/search?minPrice={PriceFrom}&itemPerPage={RowPerPage}";
                    }
                    else if (PriceFrom == "" && PriceTo != "")
                    {
                        query_string = $"{ConnectionString.connectionString}/book/search?mmaxPrice={PriceTo}&itemPerPage={RowPerPage}";
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    if (PriceFrom != "" && PriceTo != "")
                    {
                        query_string = $"{ConnectionString.connectionString}/book/search?name={SearchValue}&minPrice={PriceFrom}&maxPrice={PriceTo}&itemPerPage={RowPerPage}";
                    }
                    else if (PriceFrom != "" && PriceTo == "")
                    {
                        query_string = $"{ConnectionString.connectionString}/book/search?name={SearchValue}&minPrice={PriceFrom}&itemPerPage={RowPerPage}";
                    }
                    else if (PriceFrom == "" && PriceTo != "")
                    {
                        query_string = $"{ConnectionString.connectionString}/book/search?name={SearchValue}&maxPrice={PriceTo}&itemPerPage={RowPerPage}";
                    }
                    else
                    {
                        query_string = $"{ConnectionString.connectionString}/book/search?name={SearchValue}&itemPerPage={RowPerPage}";
                    }
                }

                var uri = new Uri(query_string);

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
                    ListPagesSelectedIndex = 0;
                    TotalPages = Data.numOfPage;
                    TotalBook = Data.numOfBooks;
                    var pages = new List<int>();
                    for (int i = 1; i <= TotalPages; i++)
                    {
                        pages.Add(i);
                    }
                    ListPages = pages;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });

            BackSpaceCommand = new RelayCommand(o =>
            {
                //if (SearchValue != "")
                //{
                //    if (!isBackspaceProcessed)
                //    {
                //        // Simulate backspace key press to delete one character
                //        //Keyboard.Focus(Keyboard.FocusedElement);
                //        //var backspaceEvent = new KeyEventArgs(
                //        //    Keyboard.PrimaryDevice,
                //        //    PresentationSource.FromVisual((Visual)Keyboard.FocusedElement),
                //        //    0,
                //        //    Key.Back);
                //        //backspaceEvent.RoutedEvent = Keyboard.KeyDownEvent;
                //        //InputManager.Current.ProcessInput(backspaceEvent);

                //        //// Set the flag to indicate that the backspace key has been processed
                //        //isBackspaceProcessed = true;

                //        // Add additional functionality here
                //        // ...
                //    }
                //    if (SearchValue != "")
                //    {
                //        CallDataCommand.Execute(null);
                //    }
                //}
                //else
                //{
                //    CallDataCommand.Execute(null);
                //}
            });

            UpdateDataListCommand = new RelayCommand(async o =>
            {
                MessageBox.Show($"{SearchValue} {PriceFrom} {PriceTo}");

                if (SearchValue != "")
                {
                    string query_string = "";

                    if (PriceFrom != "" && PriceTo != "")
                    {
                        query_string = $"{ConnectionString.connectionString}/book/search?name={SearchValue}&minPrice={PriceFrom}&maxPrice={PriceTo}&itemPerPage={RowPerPage}";
                    }
                    else if (PriceFrom != "" && PriceTo == "")
                    {
                        query_string = $"{ConnectionString.connectionString}/book/search?name={SearchValue}&minPrice={PriceFrom}&itemPerPage={RowPerPage}";
                    }
                    else if (PriceFrom == "" && PriceTo != "")
                    {
                        query_string = $"{ConnectionString.connectionString}/book/search?name={SearchValue}&maxPrice={PriceTo}&itemPerPage={RowPerPage}";
                    }
                    else
                    {
                        query_string = $"{ConnectionString.connectionString}/book/search?name={SearchValue}&itemPerPage={RowPerPage}";
                    }

                    var uri = new Uri(query_string);

                    try
                    {
                        using var client = new HttpClient();
                        var response = await client.GetAsync(uri);

                        // Check if the upload was successful
                        if (response.IsSuccessStatusCode)
                        {
                            //MessageBox.Show($"Search value: {SearchValue}");
                            var json = await response.Content.ReadAsStringAsync();
                            Data = JsonConvert.DeserializeObject<ProductListDataModel>(json);
                            // Handle the successful upload
                            //MessageBox.Show($"Success Call Data {Data.listOfBook.Count}");
                            //UpdatePagingCommand.Execute(null);
                        }
                        else { MessageBox.Show($"Fail To Call Data"); }
                        ListPagesSelectedIndex = 0;
                        TotalPages = Data.numOfPage;
                        TotalBook = Data.numOfBooks;
                        var pages = new List<int>();
                        for (int i = 1; i <= TotalPages; i++)
                        {
                            pages.Add(i);
                        }
                        ListPages = pages;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else CallDataCommand.Execute(null);
            });

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