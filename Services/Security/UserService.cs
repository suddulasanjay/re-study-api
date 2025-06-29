using AutoMapper;
using LinqToDB;
using ReStudyAPI.Data;
using ReStudyAPI.Entities;
using ReStudyAPI.Interfaces.Security;
using ReStudyAPI.Models.Security;
using ReStudyAPI.Utility.Constants;
using ReStudyAPI.Utility.Helpers;

namespace ReStudyAPI.Services.Security
{
    public class UserService : IUserService
    {
        private readonly ICurrentSessionHelper _currentSessionHelper;
        private readonly AppDBContext _db;
        private readonly IMapper _mapper;

        public UserService(ICurrentSessionHelper currentSessionHelper, AppDBContext db, IMapper mapper)
        {
            _currentSessionHelper = currentSessionHelper;
            _db = db;
            _mapper = mapper;
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            var users = await _db.Users.Where(x => x.Status == CommonStatus.Enabled).ToListAsync();
            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id && u.Status == CommonStatus.Enabled);
            return user == null ? null : _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> GetAsync()
        {
            var session = _currentSessionHelper.GetCurrentSession();
            int userId = await _currentSessionHelper.GetUserId(session);
            var user = await _db.Users.FirstOrDefaultAsync(u => session != null && u.Id == userId && u.Status == CommonStatus.Enabled);
            return user == null ? null : _mapper.Map<UserDto>(user);
        }

        public async Task<int> CreateAsync(AddUserDto addUserdto)
        {
            var user = _mapper.Map<User>(addUserdto);
            user.CreatedTime = user.ModifiedTime = DateTime.UtcNow;
            await _db.InsertAsync(user);
            return user.Id;
        }

        public async Task<int> CreateAsync(UserInfoDto userInfo)
        {
            var user = _mapper.Map<User>(userInfo);
            user.CreatedTime = user.ModifiedTime = DateTime.UtcNow;
            await _db.InsertAsync(user);
            return user.Id;
        }

        public async Task<bool> UpdateAsync(EditUserDto editUserdto)
        {
            var updated = await _db.Users
                .Where(u => u.Id == editUserdto.Id && u.Status == CommonStatus.Enabled)
                .Set(u => u.FirstName, editUserdto.FirstName)
                .Set(u => u.LastName, editUserdto.LastName)
                //.Set(u => u.Email, dto.Email)
                //.Set(u => u.Mobile, dto.Mobile)
                .Set(u => u.RoleId, editUserdto.RoleId)
                .Set(u => u.ModifiedTime, DateTime.UtcNow)
                .UpdateAsync();

            return updated > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deleted = await _db.Users
                .Where(u => u.Id == id && u.Status == CommonStatus.Enabled)
                .Set(u => u.Status, CommonStatus.Deleted)
                .UpdateAsync();
            return deleted > 0;
        }

        public async Task<bool> SSOUserIdExistsAsync(int ssoUserId)
        {
            return await _db.Users.AnyAsync(x => x.SsoUserId == ssoUserId);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _db.Users.FirstOrDefaultAsync(x => x.Email != null && x.Email.ToLower() == email.ToLower());
        }
    }
}
