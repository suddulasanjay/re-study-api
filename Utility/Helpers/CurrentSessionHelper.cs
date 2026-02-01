using LinqToDB;
using ReStudyAPI.Data;

namespace ReStudyAPI.Utility.Helpers
{
    public class CurrentSessionHelper : ICurrentSessionHelper
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly AppDBContext _db;

        public CurrentSessionHelper(IHttpContextAccessor accessor, AppDBContext dBContext)
        {
            _accessor = accessor;
            _db = dBContext;
        }

        public CurrentSession GetCurrentSession()
        {
            try
            {
                var context = _accessor.HttpContext;
                var session = new CurrentSession();
                if (context != null && context.User.Identity != null && context.User.Identity.IsAuthenticated)
                {
                    var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                    if (int.TryParse(userIdClaim, out int ssoUserId))
                    {
                        session.SSOUserId = ssoUserId;
                    }
                }
                return session;

            }
            catch (Exception ex)
            {
                throw new Exception("User Not Authenticated");
            }
        }

        public async Task<int> GetUserId(CurrentSession session)
        {
            if (session.SSOUserId != default)
            {
                return (await _db.Users.FirstOrDefaultAsync(x => x.SsoUserId == session.SSOUserId))?.Id ?? -1;
            }
            return -1;
        }
    }
    public interface ICurrentSessionHelper
    {
        CurrentSession GetCurrentSession();
        Task<int> GetUserId(CurrentSession session);
    }
}
