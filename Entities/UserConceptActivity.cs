using LinqToDB.Mapping;
namespace ReStudyAPI.Entities
{
    [Table(Schema = "trans", Name = "userconceptactivity")]
    public class UserConceptActivity : BaseEntity
    {
        [PrimaryKey, Identity, Column(Name = "id")]
        public int Id { get; set; }

        [Column(Name = "conceptid"), NotNull]
        public int ConceptId { get; set; }

        [Column(Name = "userid"), NotNull]
        public int UserId { get; set; }

        [Column(Name = "activitydate"), NotNull]
        public DateTime ActivityDate { get; set; }

        [Column(Name = "conceptstateid"), NotNull]
        public int ConceptStateId { get; set; }

        [Column(Name = "comment"), Nullable]
        public string? Comment { get; set; }
    }
}
