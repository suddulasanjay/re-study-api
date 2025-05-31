namespace ReStudyAPI.Models.Security
{
    public class UserDto
    {
        public int Id { get; set; }
        public required string Firstname { get; set; }
        public required string Lastname { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public int? SsoUserId { get; set; }
        public int RoleId { get; set; }
    }
}
