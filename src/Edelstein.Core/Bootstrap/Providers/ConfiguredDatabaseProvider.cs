using System;
using Edelstein.Core.Bootstrap.Providers.Database;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Edelstein.Core.Bootstrap.Providers
{
    public class ConfiguredDatabaseProvider : IProvider
    {
        private readonly string _section;

        public ConfiguredDatabaseProvider(string section)
            => _section = section;

        public void Provide(HostBuilderContext context, IServiceCollection collection)
        {
            var section = context.Configuration.GetSection(_section);
            var type = Enum.Parse<DatabaseProviderTypes>(section["Type"]);

            switch (type)
            {
                case DatabaseProviderTypes.LiteDB:
                    new LiteDBDatabaseProvider(section["ConnectionString"]).Provide(context, collection);
                    break;
                case DatabaseProviderTypes.Postgres:
                    new MartenDatabaseProvider(section["ConnectionString"]).Provide(context, collection);
                    break;
            }
        }
    }
}