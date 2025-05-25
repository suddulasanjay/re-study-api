using LinqToDB.Mapping;
namespace ReStudyAPI.Entities
{
    [Table(Schema = "ref", Name = "conceptstate")]
    public class ConceptState : BaseEntity
    {
        [PrimaryKey, Column(Name = "id")]
        public int Id { get; set; }

        [Column(Name = "name"), NotNull]
        public required string Name { get; set; }
    }
}
