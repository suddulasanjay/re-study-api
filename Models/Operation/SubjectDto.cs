namespace ReStudyAPI.Models.Operation
{
    public class SubjectDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public bool IsPreset { get; set; }
    }
}
