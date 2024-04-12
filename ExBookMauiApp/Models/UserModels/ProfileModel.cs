namespace ExBookMauiApp.Models.UserModels
{
    public class ProfileModel
    {
        public string username { get; set; } = null!;

        public string last_name { get; set; } = null!;

        public string first_name { get; set; } = null!;

        public string email { get; set; } = null!;

        public string phone_number { get; set; } = null!;

        public int? coins { get; set; } = null!;

        public byte[] photo { get; set; } = null!;
    }
}
