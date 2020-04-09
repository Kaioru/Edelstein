using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Templates.Etc.MakeCharInfo;
using Edelstein.Core.Templates.Etc.SetItemInfo;
using Edelstein.Core.Templates.Fields;
using Edelstein.Core.Templates.Items;
using Edelstein.Core.Templates.Items.ItemOption;
using Edelstein.Core.Templates.Mob;
using Edelstein.Core.Templates.NPC;
using Edelstein.Core.Templates.Reactor;
using Edelstein.Core.Templates.Server.Continent;
using Edelstein.Core.Templates.Server.Shop;
using Edelstein.Core.Templates.Strings;
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
                            ),
                        [DataTemplateType.NPC] =
                            Tuple.Create<Type, IDataTemplateCollection>(
                                typeof(NPCTemplate),
                                new NPCTemplateCollection(directory)
                            ),
                        [DataTemplateType.Mob] =
                            Tuple.Create<Type, IDataTemplateCollection>(
                                typeof(MobTemplate),
                                new MobTemplateCollection(directory)
                            ),
                        [DataTemplateType.Reactor] =
                            Tuple.Create<Type, IDataTemplateCollection>(
                                typeof(ReactorTemplate),
                                new ReactorTemplateCollection(directory)
                            ),
                        [DataTemplateType.ItemOption] =
                            Tuple.Create<Type, IDataTemplateCollection>(
                                typeof(ItemOptionTemplate),
                                new ItemOptionTemplateCollection(directory)
                            ),
                        [DataTemplateType.SetItemInfo] =
                            Tuple.Create<Type, IDataTemplateCollection>(
                                typeof(SetItemInfoTemplate),
                                new SetItemInfoTemplateCollection(directory)
                            ),
                        [DataTemplateType.Continent] =
                            Tuple.Create<Type, IDataTemplateCollection>(
                                typeof(ContinentTemplate),
                                new ContinentTemplateCollection(directory)
                            ),
                        [DataTemplateType.ItemString] =
                            Tuple.Create<Type, IDataTemplateCollection>(
                                typeof(ItemStringTemplate),
                                new ItemStringTemplateCollection(directory)
                            ),
                        [DataTemplateType.FieldString] =
                            Tuple.Create<Type, IDataTemplateCollection>(
                                typeof(FieldStringTemplate),
                                new FieldStringTemplateCollection(directory)
                            ),
                        [DataTemplateType.Commodity] =
                            Tuple.Create<Type, IDataTemplateCollection>(
                                typeof(CommodityTemplate),
                                new CommodityTemplateCollection(directory)
                            ),
                        [DataTemplateType.CashPackage] =
                            Tuple.Create<Type, IDataTemplateCollection>(
                                typeof(CashPackageTemplate),
                                new CashPackageTemplateCollection(directory)
                            ),
                        [DataTemplateType.ModifiedCommodity] =
                            Tuple.Create<Type, IDataTemplateCollection>(
                                typeof(ModifiedCommodityTemplate),
                                new ModifiedCommodityTemplateCollection(directory)
                            ),
                        [DataTemplateType.Best] =
                            Tuple.Create<Type, IDataTemplateCollection>(
                                typeof(BestTemplate),
                                new BestTemplateCollection(directory)
                            ),
                        [DataTemplateType.CategoryDiscount] =
                            Tuple.Create<Type, IDataTemplateCollection>(
                                typeof(CategoryDiscountTemplate),
                                new CategoryDiscountTemplateCollection(directory)
                            ),
                        [DataTemplateType.NotSale] =
                            Tuple.Create<Type, IDataTemplateCollection>(
                                typeof(NotSaleTemplate),
                                new NotSaleTemplateCollection(directory)
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