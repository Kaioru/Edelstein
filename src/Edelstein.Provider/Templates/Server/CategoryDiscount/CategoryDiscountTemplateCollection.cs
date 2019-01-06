using System.Linq;
using System.Threading.Tasks;
using Edelstein.Provider.Parser;
using MoreLinq.Extensions;

namespace Edelstein.Provider.Templates.Server.CategoryDiscount
{
    public class CategoryDiscountTemplateCollection : AbstractEagerTemplateCollection
    {
        public CategoryDiscountTemplateCollection(IDataDirectoryCollection collection) : base(collection)
        {
        }

        public override Task LoadAll()
        {
            var property = Collection.Resolve("Server/CategoryDiscount.img");

            var id = 0;
            property.Children
                .SelectMany(c => c.Children)
                .ToDictionary(
                    c => id++,
                    c => CategoryDiscountTemplate.Parse(id, c)
                )
                .ForEach(kv => Templates.Add(kv.Key, kv.Value));
            return Task.CompletedTask;
        }
    }
}