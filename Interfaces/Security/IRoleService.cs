using ReStudyAPI.Models.Security;

namespace ReStudyAPI.Interfaces.Security
{
    public interface IRoleService
    {
        public Task<List<RoleDto>> GetAllAsync();
        public Task<RoleDto?> GetByIdAsync(int id);
    }
}
