using LinqToDB.Mapping;
namespace ReStudyAPI.Entities
{
    [Table(Schema = "trans", Name = "category")]
    public class Category : BaseEntity
    {
        [PrimaryKey, Identity, Column(Name = "id")]
        public int Id { get; set; }

        [Column(Name = "subjectid"), Nullable]
        public int? SubjectId { get; set; }

        [Column(Name = "name"), NotNull]
        public required string Name { get; set; }

        [Column(Name = "description"), Nullable]
        public string? Description { get; set; }
    }
}
