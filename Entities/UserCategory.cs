using LinqToDB.Mapping;
namespace ReStudyAPI.Entities
{
    [Table(Schema = "trans", Name = "usercategory")]
    public class UserCategory : BaseEntity
    {
        [PrimaryKey, Identity, Column(Name = "id")]
        public int Id { get; set; }

        [Column(Name = "userid"), NotNull]
        public int UserId { get; set; }

        [Column(Name = "categoryid"), NotNull]
        public int CategoryId { get; set; }
    }
}
