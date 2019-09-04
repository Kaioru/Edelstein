namespace Edelstein.Core.Bootstrap
{
    public class StartupOptions
    {
        public StartupDistributionOption Distribution { get; set; } =
            new StartupDistributionOption
            {
                Type = StartupDistributionType.InMemory
            };

        public StartupDatabaseOption Database { get; set; } =
            new StartupDatabaseOption
            {
                Type = StartupDatabaseType.InMemory
            };

        public StartupDataParserOption DataParser { get; set; } =
            new StartupDataParserOption
            {
                Type = StartupDataParserType.NX,
                Path = "./data"
            };

        public StartupScriptOption Script { get; set; } =
            new StartupScriptOption
            {
                Type = StartupScriptType.Lua,
                Path = "./scripts"
            };
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