using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDataBinding
{
    public class BookDAO
    {
        public ObservableCollection<Book> GetAll()
        {
            var list = new ObservableCollection<Book>();
            string data = "BookData/data.csv"; // comma separated values
            var reader = new StreamReader(data);
            string? buffer = reader.ReadLine();

            while (!reader.EndOfStream)
            {
                buffer = reader.ReadLine();
                var tokens = buffer.Split(new string[] { "," },
                    StringSplitOptions.None);
                foreach (var token in tokens)
                {
                    System.Diagnostics.Debug.WriteLine(token);
                }
                string name = tokens[0];
                string cover = tokens[1];
                string author = tokens[2];
                string publishedYear = tokens[3];

                list.Add(new Book()
                {
                    Name = name,
                    Cover = cover,
                    Author = author,
                    PublishedYear = publishedYear
                });
            }

            reader.Close();
            return list;
        }
    }
}