using LinqToDB.Mapping;
namespace ReStudyAPI.Entities
{
    [Table(Schema = "trans", Name = "concept")]
    public class Concept : BaseEntity
    {
        [PrimaryKey, Identity, Column(Name = "id")]
        public int Id { get; set; }

        [Column(Name = "name"), NotNull]
        public required string Name { get; set; }

        [Column(Name = "description"), Nullable]
        public string? Description { get; set; }

        [Column(Name = "categoryid"), NotNull]
        public int CategoryId { get; set; }

        [Column(Name = "duration"), NotNull]
        public int Duration { get; set; }

        [Column(Name = "scheduleddate"), NotNull]
        public DateTime ScheduledDate { get; set; }

        [Column(Name = "repetitiongap"), NotNull]
        public int RepetitionGap { get; set; } = 1;
    }
}
