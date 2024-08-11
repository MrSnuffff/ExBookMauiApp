using ExBookMauiApp.Models;

namespace ExBookMauiApp.Pages;

public partial class BookDetailPage : ContentPage
{
    public BookDetailPage(Userbook selectedBook)
    {
        InitializeComponent();

        BindingContext = selectedBook;

        var bookImage = new Image
        {
            Source = string.IsNullOrEmpty(selectedBook.book_photo) ? "default_book_image.jpg" : selectedBook.book_photo,
            WidthRequest = 200,
            HeightRequest = 200,
            Aspect = Aspect.AspectFit
        };
        BookInfo.Children.Insert(1, bookImage);
    }
}
