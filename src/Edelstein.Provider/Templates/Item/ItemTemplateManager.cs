using System.Linq;
using System.Threading.Tasks;
using Edelstein.Provider.Parser;

namespace Edelstein.Provider.Templates.Item
{
    public class ItemTemplateManager : AbstractLazyTemplateCollection
    {
        public ItemTemplateManager(IDataDirectoryCollection collection) : base(collection)
        {
        }

        public override Task<ITemplate> Load(int id)
        {
            IDataProperty prop = null;
            ItemTemplate item = null;
            var type = id / 1000000;
            var subType = id % 1000000 / 10000;
            var header = id / 10000;
    
            // TODO implement item types
            switch (type)
            {
                case 1:
                    item = new ItemEquipTemplate();
                    prop = Collection.Resolve("Character").Children
                        .SelectMany(c => c.Children)
                        .FirstOrDefault(c => c.Name == $"{id:D8}.img");
                    break;
                case 2:
                    prop = Collection.Resolve($"Item/Consume/{header:D4}.img/{id:D8}");
                    break;
                case 3:
                    prop = Collection.Resolve($"Item/Install/{header:D4}.img/{id:D8}");
                    break;
                case 4:
                    prop = Collection.Resolve($"Item/Etc/{header:D4}.img/{id:D8}");
                    break;
                case 5:
                    switch (subType)
                    {
                        case 0:
                            prop = Collection.Resolve($"Item/Pet/{id:D7}.img");
                            break;
                    }

                    prop = prop ?? Collection.Resolve($"Item/Cash/{header:D4}.img/{id:D8}");
                    break;
            }

            if (item == null) item = new ItemBundleTemplate();

            item.Parse(id, prop);
            return Task.FromResult<ITemplate>(item);
        }
    }
}