using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Templates.Etc.MakeCharInfo;
using Edelstein.Core.Templates.Fields;
using Edelstein.Core.Templates.Items;
using Edelstein.Provider;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Edelstein.Core.Bootstrap.Providers.Templates
{
    public class DataTemplateProvider : IProvider
    {
        private readonly DataTemplateType _type;

        public DataTemplateProvider(DataTemplateType type)
            => _type = type;

        public void Provide(HostBuilderContext context, IServiceCollection collection)
        {
            collection.AddSingleton<IDataTemplateManager, DataTemplateManager>(f =>
            {
                var manager = new DataTemplateManager();
                var directory = f.GetService<IDataDirectoryCollection>();
                var tasks = new Dictionary<DataTemplateType, Tuple<Type, IDataTemplateCollection>>
                    {
                        [DataTemplateType.Item] =
                            Tuple.Create<Type, IDataTemplateCollection>(
                                typeof(ItemTemplate),
                                new ItemTemplateCollection(directory)
                            ),
                        [DataTemplateType.MakeCharInfo] =
                            Tuple.Create<Type, IDataTemplateCollection>(
                                typeof(MakeCharInfoTemplate),
                                new MakeCharInfoTemplateCollection(directory)
                            ),
                        [DataTemplateType.Field] =
                            Tuple.Create<Type, IDataTemplateCollection>(
                                typeof(FieldTemplate),
                                new FieldTemplateCollection(directory)
                            )
                    }
                    .Where(kv => _type.HasFlag(kv.Key))
                    .Select(kv => manager.Register(kv.Value.Item1, kv.Value.Item2));

                Task.WhenAll(tasks).Wait();
                return manager;
            });
        }
    }
}