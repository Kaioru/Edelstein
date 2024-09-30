using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Game.Items;
using Edelstein.Protocol.Gameplay.Game.Items.Consume;

namespace Edelstein.Common.Gameplay.Game.Handling.Pipes;

public class UserOnPacketUserItemOptionUpgradeRequest(
    IItemUseManager<IItemOptionUpgradeItemUseManagerContext, UserItemOptionUpgradeItemUseRequest, IItemBundleTemplate> manager
) : AbstractUserOnPacketUserItemUseRequest<UserItemOptionUpgradeItemUseRequest, IItemOptionUpgradeItemUseManagerContext, IItemBundleTemplate>(
    manager, 
    ItemInventoryType.Consume
);
