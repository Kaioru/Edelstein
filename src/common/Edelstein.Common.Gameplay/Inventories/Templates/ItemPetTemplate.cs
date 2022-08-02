using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Inventories.Templates;

namespace Edelstein.Common.Gameplay.Inventories.Templates;

public record ItemPetTemplate : ItemTemplate, IItemPetTemplate
{
    public ItemPetTemplate(int id, IDataProperty info) : base(id, info) => Life = info.Resolve<short>("life") ?? -1;

    public int Life { get; }
}
