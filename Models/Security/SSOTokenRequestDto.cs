using System.ComponentModel.DataAnnotations;

namespace ReStudyAPI.Models.Security
{
    public class SSOTokenRequestDto
    {
        [Required]
        public required string GrantType { get; set; }
        public string GrantValue { get; set; }
        public string? Scope { get; set; }
        public string? RedirectUri { get; set; }
    }
}
