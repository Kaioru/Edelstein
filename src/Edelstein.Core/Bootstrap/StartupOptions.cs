namespace Edelstein.Core.Bootstrap
{
    public class StartupOptions
    {
        public StartupDistributionOption Distribution { get; set; }
        public StartupDatabaseOption Database { get; set; }
        public StartupDataParserOption DataParser { get; set; }
        public StartupScriptOption Script { get; set; }
    }

    public enum StartupDistributionType
    {
        InMemory,
        Redis
    }

    public class StartupDistributionOption
    {
        public StartupDistributionType Type { get; set; }
        public string ConnectionString { get; set; }
    }
    
    public enum StartupDatabaseType
    {
        InMemory,
        LiteDB,
        PostgreSQL
    }

    public class StartupDatabaseOption
    {
        public StartupDatabaseType Type { get; set; }
        public string ConnectionString { get; set; }
    }
    
    public enum StartupDataParserType
    {
        NX
    }

    public class StartupDataParserOption
    {
        public StartupDataParserType Type { get; set; }
        public string Path { get; set; }
    }
    
    public enum StartupScriptType
    {
        Lua,
        Python
    }

    public class StartupScriptOption
    {
        public StartupScriptType Type { get; set; }
        public string Path { get; set; }
    }
}