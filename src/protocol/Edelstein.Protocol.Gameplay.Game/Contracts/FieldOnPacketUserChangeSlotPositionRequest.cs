using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Inventories;

namespace Edelstein.Protocol.Gameplay.Game.Contracts;

public record FieldOnPacketUserChangeSlotPositionRequest(
    IFieldUser User,
    ItemInventoryType Type,
    short From,
    short To,
    short Count
);
