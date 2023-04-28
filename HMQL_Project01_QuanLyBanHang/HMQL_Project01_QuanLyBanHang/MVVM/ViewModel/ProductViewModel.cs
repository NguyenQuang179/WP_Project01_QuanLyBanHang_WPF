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

namespace HMQL_Project01_QuanLyBanHang.MVVM.ViewModel
{
    internal class ProductViewModel : ObservableObject
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

        public RelayCommand SaveBookToAPICommand { get; set; }

        public RelayCommand BrowseImage { get; set; }

        public ProductViewModel(MainViewModel mainVM)
        {
            BackCommand = new RelayCommand(o =>
            {
                mainVM.ProductListViewCommand.Execute(null);
            });

            BrowseImage = new RelayCommand(o =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == true)
                {
                    //BookImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                }
            });
        }
    }
}