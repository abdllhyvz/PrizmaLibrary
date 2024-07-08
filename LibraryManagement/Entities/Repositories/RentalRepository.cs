using LibraryManagementService.DBModels;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementService.Entities.Repositories
{
    public class RentalRepository:Repository<Rental>,IRentalRepository
    {
        public RentalRepository(DBContext dBContext) : base(dBContext) { }

        public async Task<List<Rental>> ActiveByUser(string mail)
        {
            return await dbContext.Set<Rental>().Include(i => i.Book).Include(i => i.User).Where(b => b.User.Email == mail && b.ReturnDate == null).ToListAsync();
        }

        public async Task<List<Rental>> GetRentsAllData()
        {
            return await dbContext.Set<Rental>().Include(i => i.Book).Include(i => i.User).ToListAsync();
        }
    }
}
