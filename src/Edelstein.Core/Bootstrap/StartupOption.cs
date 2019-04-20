using Edelstein.Core.Bootstrap.Types;

namespace Edelstein.Core.Bootstrap
{
    public class StartupOption
    {
        public DistributedType? DistributedType { get; set; }
        public string? DistributedConnectionString { get; set; }
        
        public DatabaseType? DatabaseType { get; set; }
        public string? DatabaseConnectionString { get; set; }
        
        public ProviderType? ProviderType { get; set; }
        public string? ProviderFolderPath { get; set; }
        
        public ScriptType? ScriptType { get; set; }
        public string? ScriptFolderPath { get; set; }
    }
}