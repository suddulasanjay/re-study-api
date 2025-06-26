using LinqToDB.Mapping;

namespace ReStudyAPI.Entities
{
    [Table(Schema = "trans", Name = "notification")]
    public class Notification : BaseEntity
    {
        [PrimaryKey, Identity]
        [Column("id")]
        public int Id { get; set; }

        [Column("userid"), NotNull]
        public int UserId { get; set; }

        [Column("conceptid"), NotNull]
        public int ConceptId { get; set; }

        [Column("notificationtypeid"), NotNull]
        public int NotificationTypeId { get; set; }

        [Column("isread"), NotNull]
        public bool IsRead { get; set; } = false;
    }
}
