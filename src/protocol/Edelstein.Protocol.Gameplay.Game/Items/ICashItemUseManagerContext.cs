using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;

namespace Edelstein.Protocol.Gameplay.Game.Items;

public interface ICashItemUseManagerContext<out TTemplate, out TInfoEx> :
    IItemUseManagerContext<TTemplate, ICashItemUseInfo>
    where TTemplate : IItemTemplate
    where TInfoEx : ICashItemUseInfoEx
{
    TInfoEx InfoEx { get; }
}
