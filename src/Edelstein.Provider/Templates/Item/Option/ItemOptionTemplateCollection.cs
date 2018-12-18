using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Provider.Parser;
using MoreLinq.Extensions;

namespace Edelstein.Provider.Templates.Item.Option
{
    public class ItemOptionTemplateCollection : AbstractEagerTemplateCollection
    {
        public ItemOptionTemplateCollection(IDataDirectoryCollection collection) : base(collection)
        {
        }

        public override Task LoadAll()
        {
            var prop = Collection.Resolve("Item/ItemOption.img");

            prop.Children
                .ToDictionary(
                    p => Convert.ToInt32(p.Name),
                    p => ItemOptionTemplate.Parse(Convert.ToInt32(p.Name), p)
                )
                .ForEach(kv => Templates.Add(kv.Key, kv.Value));
            return Task.CompletedTask;
        }
    }
}