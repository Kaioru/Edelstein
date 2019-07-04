namespace Edelstein.Core.Bootstrap.Providers
{
    public abstract class AbstractDatabaseProvider : AbstractProvider
    {
        protected string ConnectionString { get; }
        
        protected AbstractDatabaseProvider(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}