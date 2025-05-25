using LinqToDB.Mapping;
namespace ReStudyAPI.Entities
{
    public abstract class BaseEntity
    {
        [Column(Name = "status"), NotNull]
        public string Status { get; set; } = "E";

        [Column(Name = "createdtime"), NotNull]
        public DateTime CreatedTime { get; set; }

        [Column(Name = "modifiedtime"), NotNull]
        public DateTime ModifiedTime { get; set; }

        [Column(Name = "modifiedbyuserid"), Nullable]
        public int? ModifiedByUserId { get; set; }
    }
}
