using Duey.Abstractions;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;

namespace Edelstein.Common.Gameplay.Entities.Inventories.Templates;

public record ItemPetTemplate : ItemTemplate, IItemPetTemplate
{
    public int Life { get; }
    
    public ItemPetTemplate(int id, IDataNode info) : base(id, info) 
        => Life = info.ResolveShort("life") ?? -1;

}
