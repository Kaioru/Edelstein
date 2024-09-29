using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;

namespace Edelstein.Protocol.Gameplay.Game.Items;

public interface IItemUseManagerContext<out TTemplate>
    where TTemplate : IItemTemplate
{
    TTemplate Template { get; }
    
    bool SkipHandle { get; set; }
    bool SkipConsumption { get; set; }
}
