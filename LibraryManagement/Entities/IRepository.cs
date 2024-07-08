namespace LibraryManagementService.Entities
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> All();
        Task<T?> FindById(int id);
        Task<bool> Add(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);
    }
}
