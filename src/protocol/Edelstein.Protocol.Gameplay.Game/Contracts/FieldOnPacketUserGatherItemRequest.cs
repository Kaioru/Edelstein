using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Inventories;

namespace Edelstein.Protocol.Gameplay.Game.Contracts;

public record FieldOnPacketUserGatherItemRequest(
    IFieldUser User,
    ItemInventoryType Type
);
