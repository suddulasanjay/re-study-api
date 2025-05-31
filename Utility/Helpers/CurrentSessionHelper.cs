namespace ReStudyAPI.Utility.Helpers
{
    public class CurrentSessionHelper : ICurrentSessionHelper
    {
        private readonly IHttpContextAccessor _accessor;

        public CurrentSessionHelper(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public CurrentSession? GetCurrentSession()
        {
            try
            {
                var context = _accessor.HttpContext;
                var session = new CurrentSession();
                session.UserId = 1;
                if (context != null && context.User.Identity.IsAuthenticated)
                {
                    var UserId = context.User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;
                }
                return session;

            }
            catch (Exception ex)
            {
                throw new Exception("User Not Authenticated");
            }
        }
    }
    public interface ICurrentSessionHelper
    {
        CurrentSession? GetCurrentSession();
    }
}
