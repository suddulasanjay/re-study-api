using LinqToDB.Mapping;

namespace ReStudyAPI.Entities
{
    [Table(Schema = "ref", Name = "lunotificationtype")]
    public class NotificationType : BaseEntity
    {
        [PrimaryKey, Identity]
        [Column("id")]
        public int Id { get; set; }

        [Column("name"), NotNull]
        public string Name { get; set; } = null!;

        [Column("description"), Nullable]
        public string? Description { get; set; }
    }
}
