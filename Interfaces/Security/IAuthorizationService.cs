using ReStudyAPI.Models.Security;

namespace ReStudyAPI.Interfaces.Security
{
    public interface IAuthorizationService
    {
        public Task<SSOTokenResponseDto?> GetToken(SSOTokenRequestDto tokenRequestDto);
    }
}
