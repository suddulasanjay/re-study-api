namespace ReStudyAPI.Models.Operation
{
    public class AddCategoryDto
    {
        public int? SubjectId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
