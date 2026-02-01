namespace ReStudyAPI.Models.Operation
{
    public class StudySessionDto
    {
        public int ConceptId { get; set; }
        public required string ConceptName { get; set; }
        public string? ConceptDescription { get; set; }
        public int CategoryId { get; set; }
        public required string CategoryName { get; set; }
        public int? SubjectId { get; set; }
        public string? SubjectName { get; set; }
        public double RemainingDuration { get; set; }
        public int ConceptStateId { get; set; }
        public string? Comment { get; set; }
    }
}
