using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDataBinding
{
    public class Book : INotifyPropertyChanged, ICloneable
    {
        public String Name { get; set; }
        public String Cover { get; set; }
        public String Author { get; set; }
        public String PublishedYear { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}