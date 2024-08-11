
namespace ExBookMauiApp.Models
{
   
    public partial class Userbook
    {

        public int book_id { get; set; }
        public int user_id { get; set; }
        public string? description { get; set; }
        public string? book_photo { get; set; }
        public virtual Book Book { get; set; }
    }

}
