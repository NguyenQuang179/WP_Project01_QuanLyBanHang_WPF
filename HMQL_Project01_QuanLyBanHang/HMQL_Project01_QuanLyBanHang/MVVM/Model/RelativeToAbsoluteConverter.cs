using System;
using System.Globalization;
using System.Windows.Data;

namespace HMQL_Project01_QuanLyBanHang.MVVM.View
{
    public class RelativeToAbsoluteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string relative = (string)value;

            string folder = AppDomain.CurrentDomain.BaseDirectory;

            string absolute = $"{folder}{relative}";
            return absolute;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}