using LinqToDB.Mapping;
namespace ReStudyAPI.Entities
{
    [Table(Schema = "trans", Name = "usersubject")]
    public class UserSubject : BaseEntity
    {
        [PrimaryKey, Identity, Column(Name = "id")]
        public int Id { get; set; }

        [Column(Name = "userid"), NotNull]
        public int UserId { get; set; }

        [Column(Name = "subjectid"), NotNull]
        public int SubjectId { get; set; }
    }
}
