using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace HMQL_Project01_QuanLyBanHang.MVVM.Model
{
    public static class BytesToImage
    {
        public static BitmapImage ConvertToImage(byte[] imageData)
        {
            BitmapImage image = new BitmapImage();
            using (MemoryStream ms = new MemoryStream(imageData))
            {
                ms.Position = 0;
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
            }
            return image;
        }

        public static string BitmapToBase64(BitmapImage bi)
        {
            MemoryStream ms = new MemoryStream();
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bi));
            encoder.Save(ms);
            byte[] bitmapdata = ms.ToArray();

            return Convert.ToBase64String(bitmapdata);
        }
    }
}