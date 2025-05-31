namespace ReStudyAPI.Models.Security
{
    public class RoleDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
