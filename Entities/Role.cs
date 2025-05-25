using LinqToDB.Mapping;
namespace ReStudyAPI.Entities
{
    [Table(Schema = "ref", Name = "role")]
    public class Role : BaseEntity
    {
        [PrimaryKey, Column(Name = "id")]
        public int Id { get; set; }

        [Column(Name = "name"), NotNull]
        public required string Name { get; set; }

        [Column(Name = "description"), Nullable]
        public string? Description { get; set; }
    }
}
