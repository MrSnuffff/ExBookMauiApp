using ExBookMauiApp.HttpRequests;
using ExBookMauiApp.Models.UserModels;
using ExBookMauiApp.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Memory;

namespace ExBookMauiApp.Pages;

public partial class Profile : ContentPage
{
    
    public Profile()
    {
        InitializeComponent();
    }

    protected async override void OnAppearing()
    {
        await LoadOrFetchProfileAsync();
        await LoadOrFetchUserBooksAsync();
    }
    private void AddBookToLayout(Userbook book)
    {
        var tapGestureRecognizer = new TapGestureRecognizer();
        tapGestureRecognizer.Tapped += (s, e) =>
        {
            Navigation.PushAsync(new BookDetailPage(book));
        };

        var bookFrame = new Frame
        {
            CornerRadius = 15,
            Margin = new Thickness(10, 5),
            Padding = 0,
            BackgroundColor = Color.FromHex("#F59930"),
            HasShadow = false
        };

        bookFrame.GestureRecognizers.Add(tapGestureRecognizer);

        var bookLayout = new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
            Padding = new Thickness(10),
            Spacing = 10
        };

        bookFrame.Content = bookLayout;

        var bookImage = new Image
        {
            Source = string.IsNullOrEmpty(book.book_photo) ? "default_book_image.jpg" : book.book_photo,
            WidthRequest = 50,
            HeightRequest = 50,
            Aspect = Aspect.AspectFill
        };

        var bookInfoLayout = new StackLayout
        {
            Orientation = StackOrientation.Vertical,
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.FillAndExpand
        };

        var bookTitleLabel = new Label
        {
            Text = book.Book.title,
            FontSize = 15,
            TextColor = Colors.Black,
            LineBreakMode = LineBreakMode.TailTruncation,
            MaxLines = 1,
            HorizontalOptions = LayoutOptions.FillAndExpand
        };

        var bookDescriptionLabel = new Label
        {
            Text = book.description ?? "No description available",
            FontSize = 14,
            TextColor = Colors.Gray,
            LineBreakMode = LineBreakMode.WordWrap,
            MaxLines = 2,
            HorizontalOptions = LayoutOptions.FillAndExpand
        };

        bookInfoLayout.Children.Add(bookTitleLabel);
        bookInfoLayout.Children.Add(bookDescriptionLabel);

        bookLayout.Children.Add(bookImage);
        bookLayout.Children.Add(bookInfoLayout);

        Books.Clear();
        Books.Children.Add(bookFrame);
    }

    private async Task LoadOrFetchProfileAsync()
    {
        string profileJson = await SecureStorage.Default.GetAsync("UserProfile");

        if (string.IsNullOrEmpty(profileJson))
        {
            await ExBookWebApiRequests.GetUserProfile();
        }
        UpdateProfileFields();
    }
    private async Task LoadOrFetchUserBooksAsync()
    {
        if (App.memoryCache.TryGetValue("UserBooksCache", out List<Userbook> cachedBooks))
        {
            var books = cachedBooks;
            if (books != null && books.Count > 0)
            {
                foreach (var book in books)
                    AddBookToLayout(book);
            }
            else
            {
                
            }
        }
        else
        {
            await ExBookWebApiRequests.GetUserBooks();
            LoadOrFetchUserBooksAsync();
        }
    }
    private async Task UpdateProfileFields()
    {
        string profileJson = await SecureStorage.Default.GetAsync("UserProfile");
        ProfileModel profile = JsonConvert.DeserializeObject<ProfileModel>(profileJson);

        username.Text = profile.username;

        if (profile.coins != null)
            coin.Text = profile.coins.ToString();
        
        if(profile.photo_url != null)
            ProfilePhoto.Source = App.ConnectionString + profile.photo_url;

        book_count.Text = Books.Children.Count.ToString();
    }
    private async Task HandlePhotoAsync(Func<Task<FileResult>> photoPicker)
    {
        FileResult result = await photoPicker();
        if (result != null)
        {
            await ExBookWebApiRequests.AddUserPhoto(result);
            await ExBookWebApiRequests.GetUserProfile();
            await UpdateProfileFields();
        }
    }
    public async void OnFrameTapped(object sender, EventArgs e)
    {
        await HandlePhotoAsync(() => MediaPicker.Default.CapturePhotoAsync());
    }
    public async void SelectPhoto(object sender, EventArgs e)
    {
        await HandlePhotoAsync(() => MediaPicker.Default.PickPhotoAsync());
    }
    public async void TakePhoto(object sender, EventArgs e)
    {
        await HandlePhotoAsync(() => MediaPicker.Default.CapturePhotoAsync());
    }
}
