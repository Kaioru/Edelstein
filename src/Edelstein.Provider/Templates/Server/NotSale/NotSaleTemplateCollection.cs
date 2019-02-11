using System.Linq;
using System.Threading.Tasks;
using Edelstein.Provider.Parser;
using MoreLinq.Extensions;

namespace Edelstein.Provider.Templates.Server.NotSale
{
    public class NotSaleTemplateCollection : AbstractEagerTemplateCollection
    {
        public NotSaleTemplateCollection(IDataDirectoryCollection collection) : base(collection)
        {
        }

        public override Task LoadAll()
        {
            var property = Collection.Resolve("Server/NotSale.img");

            property.Children
                .ToDictionary(
                    c => c.Resolve<int>() ?? 0,
                    c => NotSaleTemplate.Parse(c.Resolve<int>() ?? 0, c)
                )
                .ForEach(kv => Templates.Add(kv.Key, kv.Value));
            return Task.CompletedTask;
        }
    }
}