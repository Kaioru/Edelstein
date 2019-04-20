using System;
using System.Linq;
using Edelstein.Provider.Templates.Item.Cash;

namespace Edelstein.Provider.Templates.Item
{
    public class ItemTemplateCollection : AbstractLazyTemplateCollection
    {
        public override TemplateCollectionType Type => TemplateCollectionType.Item;

        public ItemTemplateCollection(IDataDirectoryCollection collection) : base(collection)
        {
        }

        public override ITemplate Load(int id)
        {
            var type = (ItemTemplateType) (id / 1000000);
            var subType = id % 1000000 / 10000;
            var header = id / 10000;

            switch (type)
            {
                case ItemTemplateType.Equip:
                    return new ItemEquipTemplate(
                        id,
                        Collection.Resolve("Character").Children
                            .SelectMany(c => c.Children)
                            .FirstOrDefault(c => c.Name == $"{id:D8}.img")
                            ?.ResolveAll()
                    );
                case ItemTemplateType.Consume:
                {
                    var property = Collection.Resolve($"Item/Consume/{header:D4}.img/{id:D8}").ResolveAll();
                    return new ItemBundleTemplate(id, property);
                }

                case ItemTemplateType.Install:
                {
                    var property = Collection.Resolve($"Item/Install/{header:D4}.img/{id:D8}").ResolveAll();
                    return new ItemBundleTemplate(id, property);
                }

                case ItemTemplateType.Etc:
                {
                    var property = Collection.Resolve($"Item/Etc/{header:D4}.img/{id:D8}").ResolveAll();
                    return new ItemBundleTemplate(id, property);
                }

                case ItemTemplateType.Cash:
                {
                    if (subType == 0)
                    {
                        return new ItemPetTemplate(
                            id,
                            Collection.Resolve($"Item/Pet/{id:D7}.img").ResolveAll()
                        );
                    }

                    var property = Collection.Resolve($"Item/Consume/{header:D4}.img/{id:D8}").ResolveAll();
                    return new ItemBundleTemplate(id, property);
                }

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}