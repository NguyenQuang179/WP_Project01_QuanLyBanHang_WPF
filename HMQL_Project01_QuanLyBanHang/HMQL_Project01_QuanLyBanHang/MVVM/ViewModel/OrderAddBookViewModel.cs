﻿using HMQL_Project01_QuanLyBanHang.Core;
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
        public bool IsAddBookForEditingOrder { get; set; }

        private OrderDetails orderD;

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
        public RelayCommand ConfirmBookSelectionCommand { get; set; }
        public RelayCommand SearchCommand { get; set; }
        public RelayCommand ApplySortCommand { get; set; }
        public RelayCommand AddBookCommand { get; set; }
        public RelayCommand ReturnToViewCommand { get; private set; }
        public RelayCommand DeleteBookCommand { get; set; }

        public RelayCommand CancelCommand { get; set;  }

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
                var uri = new Uri($"{ConnectionString.connectionString}/book/search?page=1&itemPerPage=10000000&name={SearchValue}");

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
                    }
                    else { System.Windows.MessageBox.Show($"Fail To Call Data"); }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            });
            CallDataCommand.Execute(null);



            AddBookCommand = new RelayCommand(o =>
            {
                MainVM.ProductAddViewCommand.Execute(null);
            });

            ReturnToViewCommand = new RelayCommand(o => {
                List<BookInOrderForDetails> curList;
                long totalPrice;
                Object lastView;
                //Get Current Order Detail Book List
                if (IsAddBookForEditingOrder)
                {
                    curList = MainVM.OrderManagementVM.OrderDetailVM.OrderD.order.listOfBook;
                    totalPrice = MainVM.OrderManagementVM.OrderDetailVM.TotalPrice;
                    lastView = MainVM.OrderManagementVM.OrderDetailVM;
                }
                else
                {
                    curList = MainVM.OrderManagementVM.OrderCreateVM.OrderD.order.listOfBook;
                    totalPrice = MainVM.OrderManagementVM.OrderCreateVM.TotalPrice;
                    lastView = MainVM.OrderManagementVM.OrderCreateVM;
                }
                foreach (var newBookOrder in bookOrderInfoList)
                {
                    newBookOrder.Quantity = 1;
                    if (newBookOrder.IsSelected == false)
                        continue;
                    newBookOrder.IsSelected = false;
                }
                MainVM.CurrentView = lastView;
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


                if (selectedBooks.Any())
                {
                    List<BookInOrderForDetails> curList;
                    long totalPrice;
                    Object lastView;
                   
                    //Get Current Order Detail Book List
                    if (IsAddBookForEditingOrder)
                    {
                        curList = MainVM.OrderManagementVM.OrderDetailVM.OrderD.order.listOfBook;
                        totalPrice = MainVM.OrderManagementVM.OrderDetailVM.TotalPrice;
                        lastView = MainVM.OrderManagementVM.OrderDetailVM;
                    }
                    else
                    {
                        curList = MainVM.OrderManagementVM.OrderCreateVM.OrderD.order.listOfBook;
                        totalPrice = MainVM.OrderManagementVM.OrderCreateVM.TotalPrice;
                        lastView = MainVM.OrderManagementVM.OrderCreateVM;
                    }

                    int tempPrice = 0;
                    foreach (var newBook in selectedBooks)
                    {
                        //Check if book._id of newBook already exist inCurList if yes then sum both new quantity
                        var existingBook = curList.FirstOrDefault(b => b.book._id == newBook.book._id);
                        if (existingBook != null)
                        {
                            //
                            existingBook.quantity += newBook.quantity;
                            newBook.quantity = 1;
                            
                        }
                        else
                        {
                            // If book._id doesn't exist inCurList then add it into curList.
                            curList.Add(newBook);
                        }
                        //Update total
                        tempPrice += newBook.quantity * newBook.book.price;

                    }
                    //SetAll Selected to false;
                    //Reset Everything
                    foreach(var newBookOrder in bookOrderInfoList)
                    {
                        newBookOrder.Quantity = 1;
                        if (newBookOrder.IsSelected == false)
                            continue;
                        newBookOrder.IsSelected = false;
                    }
                    SearchValue = "";
                    CallDataCommand.Execute(null);
                    totalPrice = tempPrice + totalPrice;

                    if (IsAddBookForEditingOrder)
                    {
                        int bookCount = 0;
                        foreach (var books in curList)
                        {
                            bookCount += books.quantity;
                        }

                        MainVM.OrderManagementVM.OrderDetailVM.BookQuantity = bookCount;
                        MainVM.OrderManagementVM.OrderDetailVM.TotalPrice = totalPrice;
                        
                    }
                    else
                    {
                        int bookCount = 0;
                        foreach (var books in curList)
                        {
                            bookCount += books.quantity;
                        }
                        
                        MainVM.OrderManagementVM.OrderCreateVM.TotalPrice = totalPrice;
                        MainVM.OrderManagementVM.OrderCreateVM.BookQuantity = bookCount;
                    }
                        
                    MainVM.CurrentView = lastView;

                }
                else
                {
                    System.Windows.MessageBox.Show("Please Select An Item");
                }
            });
        }

    }
}
