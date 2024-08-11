using System.Collections.Generic;

namespace ExBookMauiApp.Models
{
    public class Book
    {
        public int? book_id { get; set; }
        public string? title { get; set; }
        public string? isbn13 { get; set; }
        public string? isbn10 { get; set; }
        public int? categories_id { get; set; }
        public int? language_id { get; set; }
        public int? pageCount { get; set; }
        public virtual ICollection<Author> authors { get; set; }

    }
}

