using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Templates.Items.Cash;
using Edelstein.Provider;

namespace Edelstein.Core.Templates.Items
{
    public class ItemTemplateCollection : AbstractLazyDataTemplateCollection
    {
        private readonly IDataDirectoryCollection _collection;

        public ItemTemplateCollection(IDataDirectoryCollection collection)
            => _collection = collection;

        public override async Task<IDataTemplate> Load(int id)
        {
            var type = (ItemTemplateType) (id / 1000000);
            var subType = id % 1000000 / 10000;
            var header = id / 10000;

            switch (type)
            {
                case ItemTemplateType.Equip:
                {
                    return new ItemEquipTemplate(
                        id,
                        _collection.Resolve("Character").Children
                            .SelectMany(c => c.Children)
                            .FirstOrDefault(c => c.Name == $"{id:D8}.img")
                            ?.Resolve("info")
                            ?.ResolveAll()
                    );
                }
                case ItemTemplateType.Consume:
                {
                    var property = _collection.Resolve($"Item/Consume/{header:D4}.img/{id:D8}");

                    switch (subType)
                    {
                        default: return new ItemBundleTemplate(id, property.Resolve("info").ResolveAll());
                    }
                }
                case ItemTemplateType.Install:
                {
                    var property = _collection.Resolve($"Item/Install/{header:D4}.img/{id:D8}");
                    return new ItemBundleTemplate(id, property.Resolve("info").ResolveAll());
                }
                case ItemTemplateType.Etc:
                {
                    var property = _collection.Resolve($"Item/Etc/{header:D4}.img/{id:D8}");
                    return new ItemBundleTemplate(id, property.Resolve("info").ResolveAll());
                }
                case ItemTemplateType.Cash:
                {
                    if (subType == 0)
                    {
                        return new PetItemTemplate(
                            id,
                            _collection.Resolve($"Item/Pet/{id:D7}.img/info").ResolveAll()
                        );
                    }

                    var property = _collection.Resolve($"Item/Cash/{header:D4}.img/{id:D8}");
                    return new ItemBundleTemplate(id, property.Resolve("info").ResolveAll());
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}