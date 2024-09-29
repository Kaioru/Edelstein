using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;

namespace Edelstein.Protocol.Gameplay.Game.Items;

public interface IItemUseManagerContext<out TTemplate, out TInfo>
    where TTemplate : IItemTemplate
    where TInfo : IItemUseInfo
{
    ItemSlotBase Item { get; }
    TTemplate Template { get; }
    TInfo Info { get; }
    
    bool SkipHandle { get; set; }
    bool SkipConsumption { get; set; }
}
