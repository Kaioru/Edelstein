using Duey.Abstractions;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;

namespace Edelstein.Common.Gameplay.Models.Inventories.Templates;

public record ItemPetTemplate : ItemTemplate, IItemPetTemplate
{
    public ItemPetTemplate(int id, IDataNode info) : base(id, info) 
        => Life = info.ResolveShort("life") ?? -1;
    
    public int Life { get; }
}
