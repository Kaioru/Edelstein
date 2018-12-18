using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Provider.Parser;
using MoreLinq.Extensions;

namespace Edelstein.Provider.Templates.Item.Set
{
    public class SetItemInfoTemplateCollection : AbstractEagerTemplateCollection
    {
        public SetItemInfoTemplateCollection(IDataDirectoryCollection collection) : base(collection)
        {
        }

        public override Task LoadAll()
        {
            var prop = Collection.Resolve("Etc/SetItemInfo.img");

            prop.Children
                .ToDictionary(
                    p => Convert.ToInt32(p.Name),
                    p => SetItemInfoTemplate.Parse(Convert.ToInt32(p.Name), p)
                )
                .ForEach(kv => Templates.Add(kv.Key, kv.Value));
            return Task.CompletedTask;
        }
    }
}