using LibraryManagementService.DBModels;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagementService.Entities.Repositories
{
    public class RoleRepository:Repository<IdentityRole>,IRoleRepository
    {
        public RoleRepository(DBContext dBContext) : base(dBContext) { }
    }
}
