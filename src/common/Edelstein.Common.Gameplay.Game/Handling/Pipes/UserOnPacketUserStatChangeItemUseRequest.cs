using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates.Special;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Game.Items;
using Edelstein.Protocol.Gameplay.Game.Items.Consume;

namespace Edelstein.Common.Gameplay.Game.Handling.Pipes;

public class UserOnPacketUserStatChangeItemUseRequest(
    IItemUseManager<IStatChangeItemUse, IItemStatChangeTemplate> manager
) : AbstractUserOnPacketUserItemUseRequest<UserStatChangeItemUseRequest, IStatChangeItemUse, IItemStatChangeTemplate>(
    manager, 
    ItemInventoryType.Consume
);
