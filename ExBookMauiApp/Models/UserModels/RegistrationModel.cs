namespace ExBookMauiApp.Models.UserModels
{
    public class RegistrationModel
    {
        public string username { get; set; } = null!;

        public string first_name { get; set; } = null!;

        public string last_name { get; set; } = null!;

        public string email { get; set; } = null!;

        public string phone_number { get; set; } = null!;

        public string passwordHash { get; set; } = null!;
    }
}
