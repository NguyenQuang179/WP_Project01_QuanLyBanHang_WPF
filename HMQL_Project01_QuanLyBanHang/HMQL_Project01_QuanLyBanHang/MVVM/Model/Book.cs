using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMQL_Project01_QuanLyBanHang.MVVM.Model
{
    public class Book
    {
        public string _id { get; set; }
        public string name { get; set; }
        public string author { get; set; }
        public int publishedYear { get; set; }
        public string imagePath { get; set; }
        public int price { get; set; }
        public int stock { get; set; }
        public string category { get; set; }
        public string __v { get; set; }

    }
}

// namespace BookDataBinding
// {
//     public class Book : INotifyPropertyChanged, ICloneable
//     {
//         public String Name { get; set; }
//         public String Cover { get; set; }
//         public String Author { get; set; }
//         public String PublishedYear { get; set; }

//         public event PropertyChangedEventHandler? PropertyChanged;

//         public object Clone()
//         {
//             return MemberwiseClone();
//         }
//     }
// }
