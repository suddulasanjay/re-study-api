using LinqToDB.Configuration;

namespace ReStudyAPI.Data
{
    public class CustomConnectionStringSettings : IConnectionStringSettings
    {
        public required string Name { get; set; }
        public required string ConnectionString { get; set; }
        public required string ProviderName { get; set; }
        public bool IsGlobal => false;
    }
}
