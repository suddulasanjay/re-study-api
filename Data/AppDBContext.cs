using LinqToDB;
using LinqToDB.Data;
using ReStudyAPI.Entities;
namespace ReStudyAPI.Data
{
    public class AppDBContext : DataConnection
    {
        public AppDBContext(DataOptions options) : base(options) { }
        public ITable<EmailTemplate> EmailTemplates => this.GetTable<EmailTemplate>();
        public ITable<Role> Roles => this.GetTable<Role>();
        public ITable<User> Users => this.GetTable<User>();
        public ITable<Subject> Subjects => this.GetTable<Subject>();
        public ITable<UserSubject> UserSubjects => this.GetTable<UserSubject>();
        public ITable<UserCategory> UserCategories => this.GetTable<UserCategory>();
        public ITable<Category> Categories => this.GetTable<Category>();
        public ITable<ConceptState> ConceptStates => this.GetTable<ConceptState>();
        public ITable<Concept> Concepts => this.GetTable<Concept>();
        public ITable<UserConceptActivity> UserConceptActivities => this.GetTable<UserConceptActivity>();
        public ITable<NotificationType> NotificationTypes => this.GetTable<NotificationType>();
        public ITable<Notification> Notifications => this.GetTable<Notification>();
    }
}
