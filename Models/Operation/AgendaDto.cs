namespace ReStudyAPI.Models.Operation
{
    public class AgendaDto
    {
        public int ConceptId { get; set; }
        public required string ConceptName { get; set; }
        public int? SubjectId { get; set; }
        public string? SubjectName { get; set; }
        public int ConceptStatus { get; set; }
        public int ConceptDuration { get; set; }

    }
}
