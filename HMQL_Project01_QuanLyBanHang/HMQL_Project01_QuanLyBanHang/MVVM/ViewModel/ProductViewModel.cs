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

        private bool isSaveEnabled = false;

        public bool IsSaveEnabled
        {
            get => isSaveEnabled;
            set
            {
                isSaveEnabled = value;
                OnPropertyChanged(nameof(IsSaveEnabled));
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

        public ProductViewModel(MainViewModel mainVM, string book_id)
        {
            BackGroundColorForUnusedButton = Brushes.LightGray;

            IsBrowseEnabled = false;

            IsSaveEnabled = false;

            //MessageBox.Show($"Book id: {book_id}");

            Data = new rootBookDetail();

            BackCommand = new RelayCommand(o =>
            {
                mainVM.ProductListViewCommand.Execute(null);
            });

            BrowseImageCommand = new RelayCommand(o =>
            {
                //ImagePath = SelectImage();
            });

            EditBookCommand = new RelayCommand(o =>
            {
                MessageBox.Show($"Edit Button Click");
                BackGroundColorForUnusedButton = (Brush)bc.ConvertFrom("#FF3A36DB");
                IsBrowseEnabled = true;
                IsSaveEnabled = true;
            });

            SaveBookCommand = new RelayCommand(o =>
            {
                MessageBox.Show($"Save Button Click");
                BackGroundColorForUnusedButton = Brushes.LightGray;
                IsBrowseEnabled = false;
                IsSaveEnabled = false;
            });

            DeleteBookCommand = new RelayCommand(o =>
            {
                MessageBox.Show($"Delete Button Click");
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
                            var image_uri = new Uri($"{ConnectionString.connectionString}/getImg/{book_id}");
                            var responseImg = await client.GetAsync(image_uri);
                            var imageStream = await responseImg.Content.ReadAsStringAsync();
                            var dataImg = JsonConvert.DeserializeObject<Img>(imageStream);
                            //ImagePath = dataImg.data.data;
                        }
                        else
                        {
                            MessageBox.Show($"Picture image is null");
                        }
                        // Handle the successful upload
                        //MessageBox.Show($"Success Call Data {Data.listOfBook.Count}");
                    }
                    else { MessageBox.Show($"Fail To Call Data"); }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
            CallDataCommand.Execute(null);

            //ImagePath = Data.book.imagePath;
        }
    }
}