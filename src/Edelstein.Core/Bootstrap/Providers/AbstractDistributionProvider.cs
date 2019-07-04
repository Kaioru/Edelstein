namespace Edelstein.Core.Bootstrap.Providers
{
    public abstract class AbstractDistributionProvider : AbstractProvider
    {
        protected string ConnectionString { get; }
        
        protected AbstractDistributionProvider(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}