using ReStudyAPI.Models.Operation;

namespace ReStudyAPI.Interfaces.Operation
{
    public interface ICategoryService
    {
        public Task<List<CategoryDto>> GetAllAsync();
        public Task<CategoryDto?> GetByIdAsync(int id);
        public Task<int> CreateAsync(AddCategoryDto dto);
        public Task<bool> UpdateAsync(EditCategoryDto dto);
        public Task<bool> DeleteAsync(int id);
    }
}
