using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;

namespace Edelstein.Protocol.Gameplay.Game.Items.Consume;

public interface IItemOptionUpgradeItemUseManagerContext : IItemUseManagerContext<IItemBundleTemplate, UserItemOptionUpgradeItemUseRequest>
{
    double Prob { get; set; }
}
