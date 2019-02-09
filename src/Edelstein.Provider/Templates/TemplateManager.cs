using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Provider.Logging;
using Edelstein.Provider.Parser;
using Edelstein.Provider.Templates.Etc;
using Edelstein.Provider.Templates.Field;
using Edelstein.Provider.Templates.Item;
using Edelstein.Provider.Templates.Item.Option;
using Edelstein.Provider.Templates.Item.Set;
using Edelstein.Provider.Templates.NPC;
using Edelstein.Provider.Templates.Server.Best;
using Edelstein.Provider.Templates.Server.CategoryDiscount;
using Edelstein.Provider.Templates.Server.Continent;
using Edelstein.Provider.Templates.Server.FieldSet;
using Edelstein.Provider.Templates.Server.ModifiedCommodity;
using Edelstein.Provider.Templates.Server.NotSale;
using Edelstein.Provider.Templates.Server.NPCShop;
using Edelstein.Provider.Templates.String;

namespace Edelstein.Provider.Templates
{
    public class TemplateManager : ITemplateManager
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        private readonly IDictionary<Type, ITemplateCollection> _dictionary;

        public TemplateManager(IDataDirectoryCollection collection)
        {
            _dictionary = new Dictionary<Type, ITemplateCollection>
            {
                [typeof(ItemOptionTemplate)] = new ItemOptionTemplateCollection(collection),
                [typeof(SetItemInfoTemplate)] = new SetItemInfoTemplateCollection(collection),
                [typeof(ItemTemplate)] = new ItemTemplateCollection(collection),
                [typeof(FieldTemplate)] = new FieldTemplateCollection(collection),
                [typeof(NPCTemplate)] = new NPCTemplateCollection(collection),

                [typeof(ItemStringTemplate)] = new ItemStringTemplateCollection(collection),
                [typeof(FieldStringTemplate)] = new FieldStringTemplateCollection(collection),

                [typeof(ContinentTemplate)] = new ContinentTemplateCollection(collection),
                [typeof(FieldSetTemplate)] = new FieldSetTemplateCollection(collection),

                [typeof(CommodityTemplate)] = new CommodityTemplateCollection(collection),
                [typeof(CashPackageTemplate)] = new CashPackageTemplateCollection(collection),

                [typeof(NotSaleTemplate)] = new NotSaleTemplateCollection(collection),
                [typeof(BestTemplate)] = new BestTemplateCollection(collection),
                [typeof(CategoryDiscountTemplate)] = new CategoryDiscountTemplateCollection(collection),
                [typeof(ModifiedCommodityTemplate)] = new ModifiedCommodityTemplateCollection(collection),
                [typeof(NPCShopTemplate)] = new NPCShopTemplateCollection(collection)
            };
        }

        public T Get<T>(int id)
            => _dictionary.ContainsKey(typeof(T))
                ? (T) _dictionary[typeof(T)].Get(id)
                : default(T);

        public IEnumerable<T> GetAll<T>()
            => _dictionary.ContainsKey(typeof(T))
                ? _dictionary[typeof(T)].GetAll().OfType<T>()
                : new List<T>();

        public Task<T> GetAsync<T>(int id)
            => Task.Run(() => Get<T>(id));

        public async Task Load()
        {
            Logger.Info("Loading templates..");
            await Task.WhenAll(_dictionary.Values
                .OfType<AbstractEagerTemplateCollection>()
                .Select(async c =>
                {
                    var watch = Stopwatch.StartNew();
                    await c.LoadAll();
                    Logger.Info(
                        $"Loaded {c.GetType().Name} with {c.Cache.Count()} templates in {watch.ElapsedMilliseconds}ms");
                }));
            Logger.Info("Finished loading templates..");
        }
    }
}