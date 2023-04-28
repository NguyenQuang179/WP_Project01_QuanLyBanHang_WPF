using HMQL_Project01_QuanLyBanHang.Core;
using HMQL_Project01_QuanLyBanHang.MVVM.Model;
using LiveCharts.Maps;
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

namespace HMQL_Project01_QuanLyBanHang.MVVM.ViewModel
{
    class CategoryManagementViewModel : ObservableObject
    {
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

        private ListCategory data;
        public ListCategory Data
        {
            get => data;
            set
            {
                data = value;
                OnPropertyChanged(nameof(Data));
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

        private int totalCategory;
        public int TotalCategory
        {
            get => totalCategory;
            set
            {
                totalCategory = value;
                OnPropertyChanged(nameof(TotalCategory));
            }
        }

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
                if (Data != null) PageComboboxChangeCommand.Execute(null);
            }
        }

        public RelayCommand CallData { get; set; }

        public RelayCommand UpdatePagingData { get; set; }
        public RelayCommand PageComboboxChangeCommand { get; set; }
        public RelayCommand NextPageCommand { get; set; }
        public RelayCommand PrevPageCommand { get; set; }

        public CategoryManagementViewModel() {
            CurPage = 1;
            TotalPage = 1;
            ListPagesSelectedIndex = CurPage - 1;
            TotalCategory = 0;
            RowPerPage = 10;
            SearchValue = "";
            CallData = new RelayCommand(async o =>
            {
                CurPage = 1;
                ListPagesSelectedIndex = CurPage - 1;
                var uri = new Uri($"{ConnectionString.connectionString}/category/searchCategory/?page={CurPage}&itemPerPage={RowPerPage}&name={SearchValue}");
                try
                {
                    using var client = new HttpClient();
                    var response = await client.GetAsync(uri);
                    // Check if the upload was successful
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        Data = JsonConvert.DeserializeObject<ListCategory>(json);
                        TotalCategory = Data.numOfCategory;
                        TotalPage = Data.numOfPage;
                        var pages = new List<int>();
                        for (int i = 1; i <= TotalPage; i++)
                        {
                            pages.Add(i);
                        }
                        ListPages = pages;
                    }
                    else 
                    { 
                        MessageBox.Show($"Fail To Call Data"); 
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });

            CallData.Execute(null);

            UpdatePagingData = new RelayCommand(async o =>
            {
                var uri = new Uri($"{ConnectionString.connectionString}/category/searchCategory/?page={CurPage}&itemPerPage={RowPerPage}&name={SearchValue}");
                try
                {
                    using var client = new HttpClient();
                    var response = await client.GetAsync(uri);
                    // Check if the upload was successful
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        Data = JsonConvert.DeserializeObject<ListCategory>(json);
                    }
                    else
                    {
                        MessageBox.Show($"Fail To Call Data");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });

            PageComboboxChangeCommand = new RelayCommand(o =>
            {
                CurPage = ListPagesSelectedIndex + 1;
                UpdatePagingData.Execute(null);
            });

            NextPageCommand = new RelayCommand(o =>
            {
                if (CurPage == TotalPage) { return; }
                CurPage++;
                ListPagesSelectedIndex = CurPage - 1;
                UpdatePagingData.Execute(null);
            });

            PrevPageCommand = new RelayCommand(o =>
            {
                if (CurPage == 1) { return; }
                CurPage--;
                ListPagesSelectedIndex = CurPage - 1;
                UpdatePagingData.Execute(null);
            });
        }
    }
}
