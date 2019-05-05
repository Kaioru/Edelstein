using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Provider.Logging;
using Edelstein.Provider.Templates.Etc.MakeCharInfo;
using Edelstein.Provider.Templates.Field;
using Edelstein.Provider.Templates.Field.Continent;
using Edelstein.Provider.Templates.Field.Life.Mob;
using Edelstein.Provider.Templates.Field.Life.NPC;
using Edelstein.Provider.Templates.Item;
using Edelstein.Provider.Templates.Item.Option;
using Edelstein.Provider.Templates.Item.Set;
using Edelstein.Provider.Templates.Shop;

namespace Edelstein.Provider.Templates
{
    public class TemplateManager : ITemplateManager
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        private readonly IDictionary<Type, ITemplateCollection> _dictionary;

        public TemplateManager(IDataDirectoryCollection collection, TemplateCollectionType types)
        {
            _dictionary = new Dictionary<Type, ITemplateCollection>
                {
                    [typeof(ItemTemplate)] = new ItemTemplateCollection(collection),
                    [typeof(FieldTemplate)] = new FieldTemplateCollection(collection),
                    [typeof(NPCTemplate)] = new NPCTemplateCollection(collection),
                    [typeof(MakeCharInfoTemplate)] = new MakeCharInfoTemplateCollection(collection),
                    [typeof(CommodityTemplate)] = new CommodityTemplateCollection(collection),
                    [typeof(CashPackageTemplate)] = new CashPackageTemplateCollection(collection),
                    [typeof(ModifiedCommodityTemplate)] = new ModifiedCommodityTemplateCollection(collection),
                    [typeof(BestTemplate)] = new BestTemplateCollection(collection),
                    [typeof(NotSaleTemplate)] = new NotSaleTemplateCollection(collection),
                    [typeof(CategoryDiscountTemplate)] = new CategoryDiscountTemplateCollection(collection),
                    [typeof(SetItemInfoTemplate)] = new SetItemInfoTemplateCollection(collection),
                    [typeof(ItemOptionTemplate)] = new ItemOptionTemplateCollection(collection),
                    [typeof(MobTemplate)] = new MobTemplateCollection(collection),
                    [typeof(ContinentTemplate)] = new ContinentTemplateCollection(collection)
                }
                .Where(c => types.HasFlag(c.Value.Type))
                .ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        public T Get<T>(int id) where T : ITemplate
            => _dictionary.ContainsKey(typeof(T))
                ? (T) _dictionary[typeof(T)].Get(id)
                : default;

        public IEnumerable<T> GetAll<T>() where T : ITemplate
            => _dictionary.ContainsKey(typeof(T))
                ? _dictionary[typeof(T)].GetAll().OfType<T>()
                : new List<T>();

        public async Task Load()
        {
            Logger.Info("Loading templates..");
            await Task.WhenAll(_dictionary.Values
                .OfType<AbstractEagerTemplateCollection>()
                .Select(c => Task.Run(() =>
                {
                    var watch = Stopwatch.StartNew();

                    c.LoadAll();
                    Logger.Info(
                        $"Loaded {c.GetType().Name} with {c.GetAll().Count()} templates in {watch.ElapsedMilliseconds}ms"
                    );
                })));
            Logger.Info("Finished loading templates..");
        }
    }
}