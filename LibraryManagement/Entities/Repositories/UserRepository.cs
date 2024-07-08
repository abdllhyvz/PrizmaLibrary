using LibraryManagementService.DBModels;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagementService.Entities.Repositories
{
    public class UserRepository:Repository<IdentityUser>,IUserRepository
    {
        public UserRepository(DBContext dbContext) : base(dbContext) { }
    }
}
