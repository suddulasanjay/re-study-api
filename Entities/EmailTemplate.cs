using LinqToDB.Mapping;
namespace ReStudyAPI.Entities
{
    [Table(Name = "luemailtemplate", Schema = "ref")]
    public class EmailTemplate : BaseEntity
    {
        [PrimaryKey, Column(Name = "id"), NotNull]
        public int Id { get; set; }

        [Column(Name = "templatename"), NotNull]
        public required string TemplateName { get; set; }

        [Column(Name = "subject"), NotNull]
        public required string Subject { get; set; }

        [Column(Name = "templatebody"), Nullable]
        public string? TemplateBody { get; set; }
    }
}
