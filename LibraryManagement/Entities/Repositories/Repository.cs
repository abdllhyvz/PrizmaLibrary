
using LibraryManagementService.DBModels;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementService.Entities.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected DBContext dbContext;
        internal DbSet<T> Set;

        public Repository(DBContext context)
        {
            dbContext = context;
            this.Set = dbContext.Set<T>();
        }

        public async Task<bool> Add(T entity)
        {
            await Set.AddAsync(entity);
            return true;
        }

        public async Task<IEnumerable<T>> All()
        {
            return await Set.ToListAsync();
        }

        public async Task<bool> Delete(T entity)
        {
            Set.Remove(entity);
            return true;
        }

        public async Task<T> FindById(int id)
        {
            return await Set.FindAsync(id);
        }

        public async Task<bool> Update(T entity)
        {
            Set.Update(entity);
            return true;
        }
    }
}
