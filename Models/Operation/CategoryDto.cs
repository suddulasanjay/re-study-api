namespace ReStudyAPI.Models.Operation
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public int? SubjectId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }

    }
}
