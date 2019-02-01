using System.Linq;
using System.Threading.Tasks;
using Edelstein.Provider.Parser;
using MoreLinq.Extensions;

namespace Edelstein.Provider.Templates.Etc
{
    public class CommodityTemplateCollection : AbstractEagerTemplateCollection
    {
        public CommodityTemplateCollection(IDataDirectoryCollection collection) : base(collection)
        {
        }

        public override Task LoadAll()
        {
            var property = Collection.Resolve("Etc/Commodity.img");

            property.Children
                .ToDictionary(
                    c => c.Resolve<int>("SN") ?? 0,
                    c => CommodityTemplate.Parse(c.Resolve<int>("SN") ?? 0, c)
                )
                .ForEach(kv => Templates.Add(kv.Key, kv.Value));
            return Task.CompletedTask;
        }
    }
}