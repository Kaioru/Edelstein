using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                var tasks = new Dictionary<DataTemplateType, Tuple<Type, ItemTemplateCollection>>
                    {
                        [DataTemplateType.Item] =
                            Tuple.Create(typeof(ItemTemplate), new ItemTemplateCollection(directory))
                    }
                    .Where(kv => _type.HasFlag(kv.Key))
                    .Select(kv => manager.Register(kv.Value.Item1, kv.Value.Item2));

                Task.WhenAll(tasks).Wait();
                return manager;
            });
        }
    }
}