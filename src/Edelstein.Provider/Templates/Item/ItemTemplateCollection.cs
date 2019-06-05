using System;
using System.Linq;
using Edelstein.Provider.Templates.Item.Cash;
using Edelstein.Provider.Templates.Item.Consume;

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
                            ?.Resolve("info")
                            ?.ResolveAll()
                    );
                case ItemTemplateType.Consume:
                {
                    var property = Collection.Resolve($"Item/Consume/{header:D4}.img/{id:D8}");

                    switch (subType)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 5:
                        case 21:
                        case 36:
                        case 38:
                        case 45:
                            return new StatChangeItemTemplate(
                                id,
                                property.Resolve("info").ResolveAll(),
                                property.Resolve("spec").ResolveAll()
                            );
                        case 3:
                            return new PortalScrollItemTemplate(
                                id,
                                property.Resolve("info").ResolveAll(),
                                property.Resolve("spec").ResolveAll()
                            );
                        case 10:
                            return new MobSummonItemTemplate(
                                id,
                                property.Resolve("info").ResolveAll(),
                                property.Resolve("mob").ResolveAll()
                            );
                        default: return new ItemBundleTemplate(id, property.Resolve("info").ResolveAll());
                    }
                }

                case ItemTemplateType.Install:
                {
                    var property = Collection.Resolve($"Item/Install/{header:D4}.img/{id:D8}");
                    return new ItemBundleTemplate(id, property.Resolve("info").ResolveAll());
                }

                case ItemTemplateType.Etc:
                {
                    var property = Collection.Resolve($"Item/Etc/{header:D4}.img/{id:D8}");
                    return new ItemBundleTemplate(id, property.Resolve("info").ResolveAll());
                }

                case ItemTemplateType.Cash:
                {
                    if (subType == 0)
                    {
                        return new PetItemTemplate(
                            id,
                            Collection.Resolve($"Item/Pet/{id:D7}.img/info").ResolveAll()
                        );
                    }

                    var property = Collection.Resolve($"Item/Cash/{header:D4}.img/{id:D8}");
                    return new ItemBundleTemplate(id, property.Resolve("info").ResolveAll());
                }

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}