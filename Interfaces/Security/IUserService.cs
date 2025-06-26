using ReStudyAPI.Entities;
using ReStudyAPI.Models.Security;

namespace ReStudyAPI.Interfaces.Security
{
    public interface IUserService
    {
        public Task<List<UserDto>> GetAllAsync();
        public Task<UserDto?> GetByIdAsync(int id);
        public Task<bool> SSOUserIdExistsAsync(int ssoUserId);
        public Task<User?> GetUserByEmailAsync(string email);
        public Task<int> CreateAsync(AddUserDto dto);
        public Task<int> CreateAsync(UserInfoDto dto);
        public Task<bool> UpdateAsync(EditUserDto dto);
        public Task<bool> DeleteAsync(int id);
    }
}
