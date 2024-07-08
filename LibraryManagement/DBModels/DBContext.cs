using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using LibraryManagementService.DBModels;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagementService.DBModels
{
    public class DBContext:IdentityDbContext<IdentityUser>
    {
        public DBContext(DbContextOptions options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Rental> Rental { get; set; }
    }
}
