namespace ReStudyAPI.Models.Operation
{
    public class AddSubjectDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public bool IsPreset { get; set; }
    }
}
