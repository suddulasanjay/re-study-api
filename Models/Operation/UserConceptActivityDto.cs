namespace ReStudyAPI.Models.Operation
{
    public class UserConceptActivityDto
    {
        public int ConceptId { get; set; }
        public int CategoryId { get; set; }
        public int? SubjectId { get; set; }
        public int ProgressId { get; set; }
        public required string ConceptName { get; set; }
        public required string CategoryName { get; set; }
        public string? SubjectName { get; set; }
        public DateTime ActivityDate { get; set; }
    }
}
