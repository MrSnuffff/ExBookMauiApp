using System.Collections.Generic;

namespace ExBookMauiApp.Models
{
    public class Book
    {
        public int book_id { get; set; }

        public string title { get; set; }

        public string isbn { get; set; }

        public int? author_id { get; set; }

        public int? publisher_id { get; set; }

        public int? language_id { get; set; }

       
    }
}

