using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;

namespace Edelstein.Common.Gameplay.Models.Inventories.Templates;

public record ItemPetTemplate : ItemTemplate, IItemPetTemplate
{

    public ItemPetTemplate(int id, IDataProperty info) : base(id, info) => Life = info.Resolve<short>("life") ?? -1;
    public int Life { get; }
}
