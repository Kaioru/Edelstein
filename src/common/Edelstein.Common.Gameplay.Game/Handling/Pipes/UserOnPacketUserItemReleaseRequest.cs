using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Game.Items;
using Edelstein.Protocol.Gameplay.Game.Items.Consume;

namespace Edelstein.Common.Gameplay.Game.Handling.Pipes;

public class UserOnPacketUserItemReleaseRequest(
    IItemUseManager<IReleaseItemUseManagerContext, UserItemReleaseRequest, IItemBundleTemplate> manager
) : AbstractUserOnPacketUserItemUseRequest<UserItemReleaseRequest, IReleaseItemUseManagerContext, IItemBundleTemplate>(
    manager, 
    ItemInventoryType.Consume
);
