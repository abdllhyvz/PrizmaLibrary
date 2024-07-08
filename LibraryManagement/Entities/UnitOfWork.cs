using LibraryManagementService.Entities.Repositories;
using LibraryManagementService.DBModels;

namespace LibraryManagementService.Entities
{
    public class UnitOfWork : IUnitOfWork,IDisposable
    {
        private readonly DBContext _dbContext;
        public IBookRepository Books { get; }
        public IRentalRepository Rentals { get; }
        public IRoleRepository Roles { get; }
        public IUserRepository Users { get; }

        public UnitOfWork(DBContext dbContext)
        {
            _dbContext = dbContext;
            Books = new BookRepository(dbContext);
            Rentals = new RentalRepository(dbContext);
            Roles = new RoleRepository(dbContext);
            Users = new UserRepository(dbContext);
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose() { 
            _dbContext.Dispose();
        }
    }
}
