using LinqToDB.Mapping;
namespace ReStudyAPI.Entities
{
    [Table(Schema = "trans", Name = "user")]
    public class User : BaseEntity
    {
        [PrimaryKey, Identity, Column(Name = "id")]
        public int Id { get; set; }

        [Column(Name = "firstname"), NotNull]
        public required string FirstName { get; set; }

        [Column(Name = "lastname"), NotNull]
        public required string LastName { get; set; }

        [Column(Name = "email"), Nullable]
        public string? Email { get; set; }

        [Column(Name = "mobile"), Nullable]
        public string? Mobile { get; set; }

        [Column(Name = "ssouserid"), Nullable]
        public int? SsoUserId { get; set; }

        [Column(Name = "roleid"), NotNull]
        public int RoleId { get; set; }
    }
}
