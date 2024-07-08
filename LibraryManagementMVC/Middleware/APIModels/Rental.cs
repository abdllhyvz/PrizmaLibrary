using Microsoft.AspNetCore.Identity;
using usersresponsemodel;

namespace LibraryManagementService.DBModels
{
    public class Rental
    {
        public int Id { get; set; }
        public DateOnly? RentDate { get; set; }
        public DateOnly? ReturnDate { get; set; }

        public UsersResponseModel User { get; set; }
        public Book Book { get; set; }
    }
}
