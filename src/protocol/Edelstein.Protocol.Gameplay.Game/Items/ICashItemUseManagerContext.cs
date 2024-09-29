using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;

namespace Edelstein.Protocol.Gameplay.Game.Items;

public interface ICashItemUseManagerContext<out TTemplate, out TInfo, out TInfoEx> :
    IItemUseManagerContext<TTemplate, TInfo>
    where TTemplate : IItemTemplate
    where TInfo : ICashItemUseInfo
    where TInfoEx : ICashItemUseInfoEx
{
    TInfoEx InfoEx { get; }
}
