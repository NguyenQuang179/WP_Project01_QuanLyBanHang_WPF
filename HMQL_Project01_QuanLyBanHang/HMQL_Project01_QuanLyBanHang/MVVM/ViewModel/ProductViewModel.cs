using HMQL_Project01_QuanLyBanHang.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using HMQL_Project01_QuanLyBanHang.MVVM.Model;
using Newtonsoft.Json;
using System.Net.Http;
using System.IO;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace HMQL_Project01_QuanLyBanHang.MVVM.ViewModel
{
    internal class ProductViewModel : ObservableObject
    {
        private BitmapImage _imagePath;

        public BitmapImage ImagePath
        {
            get { return _imagePath; }
            set
            {
                _imagePath = value;
                OnPropertyChanged(nameof(ImagePath));
            }
        }

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

        private Brush backGroundColorForUnusedButton;

        public Brush BackGroundColorForUnusedButton
        {
            get => backGroundColorForUnusedButton;
            set
            {
                backGroundColorForUnusedButton = value;
                OnPropertyChanged(nameof(BackGroundColorForUnusedButton));
            }
        }

        private bool isBrowseEnabled = false;

        public bool IsBrowseEnabled
        {
            get => isBrowseEnabled;
            set
            {
                isBrowseEnabled = value;
                OnPropertyChanged(nameof(IsBrowseEnabled));
            }
        }

        private bool isSaveEnabled;

        public bool IsSaveEnabled
        {
            get => isSaveEnabled;
            set
            {
                isSaveEnabled = value;
                OnPropertyChanged(nameof(IsSaveEnabled));
            }
        }

        private bool isTextReadOnly;

        public bool IsTextReadOnly
        {
            get => isTextReadOnly;
            set
            {
                isTextReadOnly = value;
                OnPropertyChanged(nameof(IsTextReadOnly));
            }
        }

        private string textBoxBorderThickness;

        public string TextBoxBorderThickness
        {
            get => textBoxBorderThickness;
            set
            {
                textBoxBorderThickness = value;
                OnPropertyChanged(nameof(TextBoxBorderThickness));
            }
        }

        private rootBookDetail data;

        public rootBookDetail Data
        {
            get => data;
            set
            {
                data = value;
                OnPropertyChanged(nameof(Data));
            }
        }

        public RelayCommand BackCommand { get; set; }

        public RelayCommand SaveBookCommand { get; set; }

        public RelayCommand BrowseImageCommand { get; set; }

        public RelayCommand EditBookCommand { get; set; }

        public RelayCommand DeleteBookCommand { get; set; }

        public RelayCommand CallDataCommand { get; set; }

        private BrushConverter bc = new BrushConverter();

        public BitmapImage bitmapImage { get; set; }

        public string Image_path = "";

        private static string SelectImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
            string ImagePath = "";
            if (openFileDialog.ShowDialog() == true)
            {
                ImagePath = openFileDialog.FileName;
            }

            return ImagePath;
        }

        public ProductViewModel(MainViewModel mainVM, string book_id, ProductListViewModel productlistVM)
        {
            BackGroundColorForUnusedButton = Brushes.LightGray;

            IsBrowseEnabled = false;

            IsSaveEnabled = false;

            IsTextReadOnly = true;

            TextBoxBorderThickness = "0 0 0 0";

            //MessageBox.Show($"Book id: {book_id}");

            Data = new rootBookDetail();

            BackCommand = new RelayCommand(o =>
            {
                mainVM.ProductListViewCommand.Execute(null);
            });

            BrowseImageCommand = new RelayCommand(o =>
            {
                Image_path = SelectImage();
                //MessageBox.Show(Image_path);
                ImagePath = new BitmapImage(new Uri(Image_path));
            });

            EditBookCommand = new RelayCommand(o =>
            {
                //MessageBox.Show($"Edit Button Click");
                BackGroundColorForUnusedButton = (Brush)bc.ConvertFrom("#FF3A36DB");
                IsBrowseEnabled = true;
                IsSaveEnabled = true;
                IsTextReadOnly = false;

                TextBoxBorderThickness = "0 0 0 1";
            });

            SaveBookCommand = new RelayCommand(async o =>
            {
                //MessageBox.Show($"Save Button Click");
                BackGroundColorForUnusedButton = Brushes.LightGray;
                IsBrowseEnabled = false;
                IsSaveEnabled = false;
                IsTextReadOnly = true;
                TextBoxBorderThickness = "0 0 0 0";

                //MessageBox.Show($"{BookName} {Author} {Price} {Stock}");
                var uri = new Uri($"{ConnectionString.connectionString}/book/update/{book_id}");

                try
                {
                    var client = new HttpClient();
                    var formData = new MultipartFormDataContent();
                    if (Image_path != null)
                    {
                        var fileStream = new FileStream(Image_path, FileMode.Open, FileAccess.Read);
                        var fileName = System.IO.Path.GetFileName(Image_path);
                        formData.Add(new StreamContent(fileStream), "file", fileName);
                    }

                    formData.Add(new StringContent(BookName), "name");
                    formData.Add(new StringContent(Author), "author");
                    formData.Add(new StringContent(Category), "category_Name");
                    formData.Add(new StringContent(PublishedYear), "publishedYear");
                    formData.Add(new StringContent(Price.ToString()), "price");
                    formData.Add(new StringContent(Stock.ToString()), "stock");
                    // Send the request and get the response
                    var response = await client.PutAsync(uri, formData);
                    // Check if the upload was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Handle the successful upload
                        var json = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Success {json}");
                    }
                    else
                    {
                        // Handle the failed upload
                        var json = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Failed {json}");
                    }

                    productlistVM.CallDataCommand.Execute(null);
                    mainVM.ProductListViewCommand.Execute(null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"exceptions {ex.Message}");
                }
            });

            DeleteBookCommand = new RelayCommand(async o =>
            {
                //MessageBox.Show($"Delete Button Click");

                //MessageBox.Show($"{BookName} {Author} {Price} {Stock}");
                var uri = new Uri($"{ConnectionString.connectionString}/book/delete/{book_id}");

                try
                {
                    var client = new HttpClient();
                    var formData = new MultipartFormDataContent();
                    // Send the request and get the response
                    var response = await client.DeleteAsync(uri);
                    // Check if the upload was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Handle the successful upload
                        var json = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Success {json}");
                    }
                    else
                    {
                        // Handle the failed upload
                        var json = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Failed {json}");
                    }

                    productlistVM.CallDataCommand.Execute(null);
                    mainVM.ProductListViewCommand.Execute(null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"exceptions {ex.Message}");
                }
            });

            CallDataCommand = new RelayCommand(async o =>
            {
                var uri = new Uri($"{ConnectionString.connectionString}/book/detail/{book_id}");

                try
                {
                    using var client = new HttpClient();
                    var response = await client.GetAsync(uri);

                    // Check if the upload was successful
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        Data = JsonConvert.DeserializeObject<rootBookDetail>(json);
                        if (Data.book.imagePath != null)
                        {
                            var image_uri = new Uri($"{ConnectionString.connectionString}/getImg/{Data.book.imagePath}");
                            var responseImg = await client.GetAsync(image_uri);
                            var imageStream = await responseImg.Content.ReadAsStringAsync();
                            var dataImg = JsonConvert.DeserializeObject<Img>(imageStream);

                            ImagePath = BytesToImage.ConvertToImage(dataImg.data.data);
                        }
                        else
                        {
                            //MessageBox.Show($"Picture image is null");
                        }
                        // Handle the successful upload
                        //MessageBox.Show($"Success Call Data {Data.listOfBook.Count}");

                        BookName = Data.book.name;
                        Author = Data.book.author;
                        Category = Data.book.category.name;
                        PublishedYear = Data.book.publishedYear;
                        Price = Data.book.price;
                        Stock = Data.book.stock;
                    }
                    else { MessageBox.Show($"Fail To Call Data"); }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });

            CallDataCommand.Execute(null);
        }
    }
}