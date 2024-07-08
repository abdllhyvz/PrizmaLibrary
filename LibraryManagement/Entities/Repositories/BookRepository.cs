using LibraryManagementService.DBModels;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementService.Entities.Repositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(DBContext dBContext) : base(dBContext) { }

        public async Task<List<Book>> SearchByTitle(string title)
        {
            return await dbContext.Set<Book>().Where(b => b.Title.Contains(title)).ToListAsync();
        }

        public async Task<List<Book>> SearchByAuthor(string author)
        {
            return await dbContext.Set<Book>().Where(b => b.Author.Contains(author)).ToListAsync();
        }
    }
}
