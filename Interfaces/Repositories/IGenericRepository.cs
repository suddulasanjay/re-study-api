namespace ReStudyAPI.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<List<T>> GetAllAsync();
        public Task<T?> GetByIdAsync(int id);
        public Task<int> CreateAsync(T entity);
        public Task<bool> UpdateAsync(T entity);
        public Task<bool> SoftDeleteAsync(int id, int? modifiedByUserId = null);
    }
}
