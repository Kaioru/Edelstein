namespace Edelstein.Core.Bootstrap
{
    public static class StartupExtensions
    {
        public static IStartup FromConfiguration(
            this Startup startup,
            string[] args,
            string jsonPath = "appsettings.json",
            string envPrefix = "APP_"
        )
            => startup
                .FromCommandLine(args)
                .FromJson(jsonPath, true)
                .FromEnvironment(envPrefix);
    }
}