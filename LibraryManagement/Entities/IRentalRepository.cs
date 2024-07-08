using LibraryManagementService.DBModels;

namespace LibraryManagementService.Entities
{
    public interface IRentalRepository:IRepository<Rental>
    {
        Task<List<Rental>> ActiveByUser(string mail);
        Task<List<Rental>> GetRentsAllData();
    }
}
