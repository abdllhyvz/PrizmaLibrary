namespace LibraryManagementService.Entities
{
    public interface IUnitOfWork
    {
        IBookRepository Books { get; }
        IRentalRepository Rentals { get; }
        IRoleRepository Roles { get; }
        IUserRepository Users { get; }

        Task SaveAsync();
    }
}
