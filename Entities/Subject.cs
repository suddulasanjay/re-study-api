using LinqToDB.Mapping;
namespace ReStudyAPI.Entities
{
    [Table(Name = "subject", Schema = "trans")]
    public class Subject : BaseEntity
    {
        [PrimaryKey, Identity, Column(Name = "id")]
        public int Id { get; set; }

        [Column(Name = "name"), NotNull]
        public required string Name { get; set; }

        [Column(Name = "description"), Nullable]
        public string? Description { get; set; }

        [Column(Name = "ispreset"), NotNull]
        public bool IsPreset { get; set; }
    }
}
