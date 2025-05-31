using ReStudyAPI.Models.Security;

namespace ReStudyAPI.Interfaces.Security
{
    public interface IUserService
    {
        public Task<List<UserDto>> GetAllAsync();
        public Task<UserDto?> GetByIdAsync(int id);
        public Task<int> CreateAsync(AddUserDto dto);
        public Task<bool> UpdateAsync(EditUserDto dto);
        public Task<bool> DeleteAsync(int id);
    }
}
