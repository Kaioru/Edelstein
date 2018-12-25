using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Provider.Parser;
using MoreLinq.Extensions;

namespace Edelstein.Provider.Templates.Server.NPCShop
{
    public class NPCShopTemplateCollection : AbstractEagerTemplateCollection
    {
        public NPCShopTemplateCollection(IDataDirectoryCollection collection) : base(collection)
        {
        }

        public override Task LoadAll()
        {
            var property = Collection.Resolve("Server/NpcShop.img");

            property.Children
                .ToDictionary(
                    c => Convert.ToInt32(c.Name),
                    c => NPCShopTemplate.Parse(Convert.ToInt32(c.Name), c)
                )
                .ForEach(kv => Templates.Add(kv.Key, kv.Value));
            return Task.CompletedTask;
        }
    }
}