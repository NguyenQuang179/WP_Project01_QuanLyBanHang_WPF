using HMQL_Project01_QuanLyBanHang.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Windows;
using HMQL_Project01_QuanLyBanHang.MVVM.Model;
using System.Net.Http;
using System.IO;
using System.Security.Policy;

namespace HMQL_Project01_QuanLyBanHang.MVVM.ViewModel
{
    internal class ProductAddViewModel : ObservableObject
    {
        private string bookName;

        public string BookName
        {
            get => bookName;
            set
            {
                bookName = value;
                OnPropertyChanged(nameof(BookName));
            }
        }

        private string author;

        public string Author
        {
            get => author;
            set
            {
                author = value;
                OnPropertyChanged(nameof(Author));
            }
        }

        private string category;

        public string Category
        {
            get => category;
            set
            {
                category = value;
                OnPropertyChanged(nameof(Category));
            }
        }

        private string publishedYear;

        public string PublishedYear
        {
            get => publishedYear;
            set
            {
                publishedYear = value;
                OnPropertyChanged(nameof(PublishedYear));
            }
        }

        private int price;

        public int Price
        {
            get => price;
            set
            {
                price = value;
                OnPropertyChanged(nameof(Price));
            }
        }

        private int stock;

        public int Stock
        {
            get => stock;
            set
            {
                stock = value;
                OnPropertyChanged(nameof(Stock));
            }
        }

        public RelayCommand BackCommand { get; set; }

        public RelayCommand AddBookToAPICommand { get; set; }

        public ProductAddViewModel(MainViewModel mainVM)
        {
            BackCommand = new RelayCommand(o =>
            {
                mainVM.ProductListViewCommand.Execute(null);
            });

            AddBookToAPICommand = new RelayCommand(async o =>
            {
                MessageBox.Show($"{BookName} {Author} {Price} {Stock}");
                var uri = new Uri($"{ConnectionString.connectionString}/book/add");

                try
                {
                    var client = new HttpClient();
                    var formData = new MultipartFormDataContent();
                    formData.Add(new StringContent(BookName), "name");
                    formData.Add(new StringContent(Author), "author");
                    formData.Add(new StringContent(Category), "category_Name");
                    formData.Add(new StringContent(PublishedYear), "publishedYear");
                    formData.Add(new StringContent(Price.ToString()), "price");
                    formData.Add(new StringContent(Stock.ToString()), "stock");
                    // Send the request and get the response
                    var response = await client.PostAsync(uri, formData);
                    // Check if the upload was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Handle the successful upload
                        var json = await response.Content.ReadAsStringAsync();
                        MessageBox.Show(json);
                    }
                    else
                    {
                        // Handle the failed upload
                        var json = await response.Content.ReadAsStringAsync();
                        MessageBox.Show(json);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
        }
    }
}