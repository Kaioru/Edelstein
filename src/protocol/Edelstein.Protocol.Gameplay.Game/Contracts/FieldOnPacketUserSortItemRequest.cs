using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Inventories;

namespace Edelstein.Protocol.Gameplay.Game.Contracts;

public record FieldOnPacketUserSortItemRequest(
    IFieldUser User,
    ItemInventoryType Type
);
