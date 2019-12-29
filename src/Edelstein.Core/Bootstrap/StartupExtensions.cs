using Edelstein.Core.Bootstrap.Providers;
using Edelstein.Network;

namespace Edelstein.Core.Bootstrap
{
    public static class StartupExtensions
    {
        public static IStartup FromConfiguration(
            this IStartup startup,
            string[] args,
            string jsonPath = "appsettings.json",
            string envPrefix = "APP_"
        )
            => startup
                .FromCommandLine(args)
                .FromJson(jsonPath, true)
                .FromEnvironment(envPrefix);

        public static IStartup WithConfiguredLogging(
            this IStartup startup
        )
            => startup
                .WithProvider(new ConfiguredLoggingProvider());

        public static IStartup WithConfiguredCaching(
            this IStartup startup,
            string section = "Caching"
        )
            => startup
                .WithProvider(new ConfiguredCachingProvider(section));

        public static IStartup WithConfiguredDatabase(
            this IStartup startup,
            string section = "Database"
        )
            => startup
                .WithProvider(new ConfiguredDatabaseProvider(section));

        public static IStartup WithConfiguredServer<TAdapterFactory>(
            this IStartup startup,
            string section = "Server"
        ) where TAdapterFactory : class, ISocketAdapterFactory
            => startup
                .WithProvider(new ConfiguredServerProvider<TAdapterFactory>(section));
    }
}