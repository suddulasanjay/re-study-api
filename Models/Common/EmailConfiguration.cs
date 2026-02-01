namespace ReStudyAPI.Models.Common
{
    public class EmailConfiguration
    {
        public bool IsSMTP { get; set; }
        public required string SMTPAddress { get; set; }
        public int SMTPPort { get; set; }
        public required string SenderEmailAddress { get; set; }
        public required string SenderUserName { get; set; }
        public required string SenderName { get; set; }
        public required string SenderPassword { get; set; }
    }
}
