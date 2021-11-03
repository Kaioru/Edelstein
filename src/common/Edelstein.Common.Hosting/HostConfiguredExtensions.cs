using System;
using Edelstein.Common.Datastore.LiteDB;
using Edelstein.Common.Datastore.Marten;
using Edelstein.Common.Hosting.Extensions;
using Edelstein.Common.Hosting.Types;
using Edelstein.Common.Parser.Duey;
using Edelstein.Common.Parser.reWZ;
using Edelstein.Common.Scripting.NLua;
using Microsoft.Extensions.Hosting;
using reWZ;

namespace Edelstein.Common.Hosting
{
    public static class HostConfiguredExtensions
    {
        public static IHostBuilder ConfigureDataStore(this IHostBuilder builder, string sectionName = "DataStore")
        {
            builder.ConfigureServices((ctx, c) =>
            {
                var section = ctx.Configuration.GetSection(sectionName);
                var type = Enum.Parse<HostDataStoreType>(section["Type"]);

                switch (type)
                {
                    case HostDataStoreType.LiteDB:
                        c.AddLiteDBDataStore(section["ConnectionString"]);
                        break;
                    case HostDataStoreType.PostgreSQL:
                        c.AddPostgresDataStore(section["ConnectionString"]);
                        break;
                }
            });
            return builder;
        }

        public static IHostBuilder ConfigureCaching(this IHostBuilder builder, string sectionName = "Caching")
        {
            builder.ConfigureServices((ctx, c) =>
            {
                var section = ctx.Configuration.GetSection(sectionName);
                var type = Enum.Parse<HostCachingType>(section["Type"]);

                switch (type)
                {
                    case HostCachingType.InMemory:
                        c.AddInMemoryCaching();
                        break;
                    case HostCachingType.Redis:
                        c.AddRedisCaching(section["ConnectionString"]);
                        break;
                }
            });
            return builder;
        }

        public static IHostBuilder ConfigureMessaging(this IHostBuilder builder, string sectionName = "Messaging")
        {
            builder.ConfigureServices((ctx, c) =>
            {
                var section = ctx.Configuration.GetSection(sectionName);
                var type = Enum.Parse<HostMessagingType>(section["Type"]);

                switch (type)
                {
                    case HostMessagingType.InMemory:
                        c.AddInMemoryMessaging();
                        break;
                    case HostMessagingType.Redis:
                        c.AddRedisMessaging(section["ConnectionString"]);
                        break;
                }
            });
            return builder;
        }

        public static IHostBuilder ConfigureParser(this IHostBuilder builder, string sectionName = "Parser")
        {
            builder.ConfigureServices((ctx, c) =>
            {
                var section = ctx.Configuration.GetSection(sectionName);
                var type = Enum.Parse<HostParserType>(section["Type"]);

                switch (type)
                {
                    case HostParserType.NX:
                        c.AddNXParser(section["Directory"]);
                        break;
                    case HostParserType.WZ:
                        c.AddWZParser(
                            section["Directory"],
                            Enum.Parse<WZVariant>(section["Variant"]),
                            bool.Parse(section["Encrypted"])
                        );
                        break;
                }
            });
            return builder;
        }

        public static IHostBuilder ConfigureScripting(this IHostBuilder builder, string sectionName = "Scripting")
        {
            builder.ConfigureServices((ctx, c) =>
            {
                var section = ctx.Configuration.GetSection(sectionName);
                var type = Enum.Parse<HostScriptingType>(section["Type"]);

                switch (type)
                {
                    case HostScriptingType.Lua:
                        c.AddLuaScripting(section["Directory"]);
                        break;
                }
            });
            return builder;
        }
    }
}
