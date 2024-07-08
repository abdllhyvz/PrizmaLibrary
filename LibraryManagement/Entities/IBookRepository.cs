using LibraryManagementService.DBModels;

namespace LibraryManagementService.Entities
{
    public interface IBookRepository:IRepository<Book>
    {
        Task<List<Book>> SearchByTitle(string title);
        Task<List<Book>> SearchByAuthor(string author);
    }
}
