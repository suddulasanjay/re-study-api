namespace ReStudyAPI.Models.Operation
{
    public class ConceptDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public int RepetitionGap { get; set; }
        public int Duration { get; set; }
    }
}
