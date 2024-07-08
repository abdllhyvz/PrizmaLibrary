using Microsoft.AspNetCore.Identity;

namespace LibraryManagementService.Entities
{
    public interface IUserRepository:IRepository<IdentityUser>
    {
    }
}
