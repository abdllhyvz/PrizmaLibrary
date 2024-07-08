using Newtonsoft.Json;

namespace LibraryManagementMVC.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; } = true;
        public string Author { get; set; }
        public static Book FromJson(string json) => JsonConvert.DeserializeObject<Book>(json, usersresponsemodel.Converter.Settings);
        public static List<Book> FromJsonList(string json) => JsonConvert.DeserializeObject<List<Book>>(json, usersresponsemodel.Converter.Settings);

    }
}
