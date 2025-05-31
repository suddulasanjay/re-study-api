namespace ReStudyAPI.Models.Operation
{
    public class AddConceptDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public int duration { get; set; }
        public DateTime ScheduledDate { get; set; }
        public int RepetitionGap { get; set; }
    }
}
