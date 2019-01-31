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

            var id = 0;
            property.Children
                .ToDictionary(
                    c => id++,
                    c => NotSaleTemplate.Parse(id, c)
                )
                .ForEach(kv => Templates.Add(kv.Key, kv.Value));
            return Task.CompletedTask;
        }
    }
}