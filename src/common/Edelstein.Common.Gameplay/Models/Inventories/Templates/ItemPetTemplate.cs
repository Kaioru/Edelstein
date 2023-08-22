using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;

namespace Edelstein.Common.Gameplay.Models.Inventories.Templates;

public record ItemPetTemplate : ItemTemplate, IItemPetTemplate
{
    public int Life { get; }
    
    public ItemPetTemplate(int id, IDataProperty info) : base(id, info) => Life = info.Resolve<short>("life") ?? -1;
}
