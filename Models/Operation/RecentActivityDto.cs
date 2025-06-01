namespace ReStudyAPI.Models.Operation
{
    public class RecentActivityDto
    {
        public required string Concept { get; set; }
        public string? Subject { get; set; }
        public DateTime ActivityDate { get; set; }
        public int Status { get; set; }
    }

}
