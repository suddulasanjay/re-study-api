using LinqToDB.Configuration;

namespace ReStudyAPI.Data
{
    public class AppLinqToDBSettings : ILinqToDBSettings
    {
        private readonly IConnectionStringSettings[] _connectionStrings;
        public AppLinqToDBSettings(IConnectionStringSettings[] connectionStrings)
        {
            _connectionStrings = connectionStrings;
        }
        public IEnumerable<IDataProviderSettings> DataProviders => new List<IDataProviderSettings>();
        public string DefaultConfiguration => "SqlServer";
        public string DefaultDataProvider => "SqlServer";
        public IEnumerable<IConnectionStringSettings> ConnectionStrings => _connectionStrings;
    }
}
