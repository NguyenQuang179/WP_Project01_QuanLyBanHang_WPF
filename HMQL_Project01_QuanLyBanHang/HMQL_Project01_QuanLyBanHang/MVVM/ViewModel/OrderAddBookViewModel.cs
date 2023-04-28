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
using System.Collections;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;
using static System.Reflection.Metadata.BlobBuilder;
using System.ComponentModel;

namespace HMQL_Project01_QuanLyBanHang.MVVM.ViewModel
{

    public class BookItemOrderInfo : Book, INotifyPropertyChanged
    {
        private bool isSelected;
        private int quantity;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public int Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }

        public BookItemOrderInfo(Book book)
        {
            _id = book._id;
            name = book.name;
            author = book.author;
            publishedYear = book.publishedYear;
            imagePath = book.imagePath;
            price = book.price;
            stock = book.stock;
            category = book.category;
            __v = book.__v;

            IsSelected = false;
            Quantity = 1;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

   
    internal class OrderAddBookViewModel : ObservableObject
    {
        private OrderDetails orderD;
        
        public RelayCommand ConfirmBookSelectionCommand { get; set; }
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

        private BookItemOrderInfo bookData;

        public BookItemOrderInfo BookData
        {
            get => bookData;
            set
            {
                bookData = value;
                OnPropertyChanged(nameof(BookData));
            }
        }

        private List<BookItemOrderInfo> _bookOrderInfoList;
        public List<BookItemOrderInfo> bookOrderInfoList
        {
            get => _bookOrderInfoList;
            set
            {
                _bookOrderInfoList = value;
                OnPropertyChanged(nameof(bookOrderInfoList));
            }
        }
        public RelayCommand CallDataCommand { get; set; }

        public RelayCommand ItemClickCommand { get; set; }

        public int Quantity { get; set; }
        public OrderAddBookViewModel(MainViewModel MainVM)
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
                        bookOrderInfoList = Data.listOfBook.Select(book => new BookItemOrderInfo(book)).ToList();
                        System.Windows.MessageBox.Show($"Success Call Data {Data.listOfBook.Count}");
                    }
                    else { System.Windows.MessageBox.Show($"Fail To Call Data"); }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            });
            CallDataCommand.Execute(null);

            ItemClickCommand = new RelayCommand((param) =>
            {
                string id = param.ToString();
                System.Windows.MessageBox.Show($"Item Clicked {id}");
            });

            AddBookCommand = new RelayCommand(o =>
            {
                MainVM.ProductAddViewCommand.Execute(null);
            });

            ConfirmBookSelectionCommand = new RelayCommand(o =>
            {
                var selectedBooks = bookOrderInfoList.Where(b => b.IsSelected)
                    .Select(b => new BookInOrderForDetails
                    {
                        _id = null, 
                        book = new Book
                        {
                            _id = b._id,
                            name = b.name,
                            author = b.author,
                            publishedYear = b.publishedYear,
                            imagePath = b.imagePath,
                            price = b.price,
                            stock = b.stock,
                            category = b.category,
                            __v = b.__v
                        },
                        quantity = b.Quantity
                    })
                    .ToList();
                System.Windows.MessageBox.Show(selectedBooks.ToString());

                if (selectedBooks.Any())
                {
                  
                    //Get Current Order Detail Book List
                    List<BookInOrderForDetails> curList = MainVM.OrderManagementVM.OrderDetailVM.OrderD.order.listOfBook;

                    foreach (var newBook in selectedBooks)
                    {
                        //Check if book._id of newBook already exist inCurList if yes then sum both new quantity
                        var existingBook = curList.FirstOrDefault(b => b.book._id == newBook.book._id);
                        if (existingBook != null)
                        {
                            existingBook.quantity += newBook.quantity;
                        }
                        else
                        {
                            // If book._id doesn't exist inCurList then add it into curList.
                            curList.Add(newBook);
                        }
                    }


                    MainVM.CurrentView = MainVM.OrderManagementVM.OrderDetailVM;

                }
            });
        }

    }
}
