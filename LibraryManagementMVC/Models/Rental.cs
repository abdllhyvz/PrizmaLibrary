using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using usersresponsemodel;

namespace LibraryManagementMVC.Models
{
    public class Rental
    {
        public int Id { get; set; }
        public DateOnly? RentDate { get; set; }
        public DateOnly? ReturnDate { get; set; }

        public UsersResponseModel User { get; set; }
        public Book Book { get; set; }
        public static List<Rental> FromJsonList(string json) => JsonConvert.DeserializeObject<List<Rental>>(json, usersresponsemodel.Converter.Settings);

    }
}
