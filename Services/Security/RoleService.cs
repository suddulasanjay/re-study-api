using AutoMapper;
using LinqToDB;
using ReStudyAPI.Data;
using ReStudyAPI.Interfaces.Security;
using ReStudyAPI.Models.Security;
using ReStudyAPI.Utility.Constants;

namespace ReStudyAPI.Services.Security
{
    public class RoleService : IRoleService
    {
        private readonly AppDBContext _db;
        private readonly IMapper _mapper;

        public RoleService(AppDBContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<List<RoleDto>> GetAllAsync()
        {
            var roles = await _db.Roles.Where(x => x.Status == CommonStatus.Enabled).ToListAsync();
            return _mapper.Map<List<RoleDto>>(roles);
        }

        public async Task<RoleDto?> GetByIdAsync(int id)
        {
            var role = await _db.Roles.FirstOrDefaultAsync(r => r.Id == id && r.Status == CommonStatus.Enabled);
            return _mapper.Map<RoleDto>(role);
        }
    }
}
