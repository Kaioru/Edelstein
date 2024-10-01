using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;

namespace Edelstein.Protocol.Gameplay.Game.Items.Consume;

public interface IItemOptionUpgradeItemUseManagerContext : IItemUseManagerContext<IItemBundleTemplate, UserItemOptionUpgradeItemUseRequest>
{
    double SuccessRate { get; set; }
    double CursedRate { get; set; }
    
    bool SkipSuccess { get; set; }
    bool SkipCursed { get; set; }
}
