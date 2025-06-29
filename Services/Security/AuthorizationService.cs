using LinqToDB;
using Microsoft.Extensions.Options;
using ReStudyAPI.Data;
using ReStudyAPI.Interfaces.Security;
using ReStudyAPI.Models.Common;
using ReStudyAPI.Models.Security;
using ReStudyAPI.Utility.Constants;
using ReStudyAPI.Utility.Helpers;
using System.Net.Http.Headers;
using System.Text.Json;
namespace ReStudyAPI.Services.Security
{
    public class AuthorizationService : Interfaces.Security.IAuthorizationService
    {
        private readonly SSOConfiguration _ssoConfiguration;
        private readonly HttpClient _httpClient;
        private readonly IUserService _userService;
        private readonly AppDBContext _db;
        private readonly IEmailUtility _emailUtility;

        public AuthorizationService(IOptions<SSOConfiguration> ssoConfiguration, HttpClient httpClient, IUserService userService, AppDBContext db, IEmailUtility emailUtility)
        {
            _ssoConfiguration = ssoConfiguration.Value;
            _httpClient = httpClient;
            _userService = userService;
            _db = db;
            _emailUtility = emailUtility;
        }

        public async Task<SSOTokenResponseDto?> GetToken(SSOTokenRequestDto tokenRequestDto)
        {
            if (string.IsNullOrEmpty(tokenRequestDto.GrantType))
            {
                throw new Exception($"{AppClaims.GrantType} cannot be null");
            }


            var request = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(AppClaims.GrantType, tokenRequestDto.GrantType)
                };

            if (tokenRequestDto.GrantType == AppClaims.AuthorizationCode)
            {
                if (string.IsNullOrEmpty(tokenRequestDto.RedirectUri))
                {
                    throw new Exception($"{AppClaims.RedirectUri} cannot be null");
                }

                request.Add(new KeyValuePair<string, string>(AppClaims.Code, tokenRequestDto.GrantValue));
                request.Add(new KeyValuePair<string, string>(AppClaims.RedirectUri, tokenRequestDto.RedirectUri));
            }
            else if (tokenRequestDto.GrantType == AppClaims.RefreshToken)
            {
                request.Add(new KeyValuePair<string, string>(AppClaims.RefreshToken, tokenRequestDto.GrantValue));
            }
            else
            {
                throw new Exception($"Invalid {AppClaims.GrantType}");
            }

            if (!string.IsNullOrWhiteSpace(tokenRequestDto.Scope))
            {
                request.Add(new KeyValuePair<string, string>(AppClaims.Scope, tokenRequestDto.Scope));
            }

            request.Add(new KeyValuePair<string, string>(AppClaims.ClientId, _ssoConfiguration.ClientId));
            request.Add(new KeyValuePair<string, string>(AppClaims.ClientSecret, _ssoConfiguration.ClientSecret));

            var requestObject = new FormUrlEncodedContent(request);
            var response = await _httpClient.PostAsync(new Uri(_ssoConfiguration.BaseUrl + _ssoConfiguration.TokenPath), requestObject);

            if (!response.IsSuccessStatusCode)
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                throw new Exception($"SSO Token request failed: {errorDetails}");
            }

            var tokenResponse = JsonSerializer.Deserialize<SSOTokenResponseDto>(await response.Content.ReadAsStringAsync());
            if (tokenResponse?.AccessToken == null)
            {
                throw new Exception("Failed to obtain access token");
            }

            if (tokenRequestDto.GrantType == AppClaims.AuthorizationCode)
            {
                var userInfo = await GetUserInfoAsync(tokenResponse.AccessToken);
                if (userInfo == null)
                {
                    throw new Exception("Failed to retrieve user info");
                }
                await EnsureSSOReferenceAsync(userInfo);
            }

            return tokenResponse;
        }

        private async Task EnsureSSOReferenceAsync(UserInfoDto userInfo)
        {
            var alreadyMapped = await _userService.SSOUserIdExistsAsync(userInfo.Id);
            if (alreadyMapped)
            {
                return;
            }
            var userWithSameEmail = await _userService.GetUserByEmailAsync(userInfo.Email);
            if (userWithSameEmail != null)
            {
                if (userWithSameEmail.SsoUserId.HasValue && (userWithSameEmail.SsoUserId.Value != userInfo.Id))
                {
                    throw new Exception($"User Already Exists with given mail {userInfo.Email}");
                }
                else
                {
                    userWithSameEmail.SsoUserId = userInfo.Id;
                    await _db.UpdateAsync(userWithSameEmail, _db.Users.TableName);
                }
            }
            else
            {
                await _userService.CreateAsync(userInfo);
                var body = (await _db.EmailTemplates.FirstOrDefaultAsync(x => x.TemplateName == "user_welcome" && x.Status == CommonStatus.Enabled))?.TemplateBody;
                if (!string.IsNullOrEmpty(body))
                {
                    await _emailUtility.SendEmailAsync(userInfo.Email, "Welcome User", body);
                }
            }
        }

        private async Task<UserInfoDto?> GetUserInfoAsync(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_ssoConfiguration.BaseUrl}{_ssoConfiguration.UserInfoPath}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var userInfo = JsonSerializer.Deserialize<UserInfoDto>(content, options);

            return userInfo;
        }
    }
}
