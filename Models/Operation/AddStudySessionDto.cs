namespace ReStudyAPI.Models.Operation
{
    public class AddStudySessionDto
    {
        public int ConceptId { get; set; }
        public double Duration { get; set; }
        public int ConceptStateId { get; set; }
        public string? Comment { get; set; }
    }
}
