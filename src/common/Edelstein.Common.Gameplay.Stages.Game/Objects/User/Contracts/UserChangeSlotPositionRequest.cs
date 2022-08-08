using Edelstein.Protocol.Gameplay.Inventories;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Contracts;

public record UserChangeSlotPositionRequest(
    IFieldUser User,
    ItemInventoryType Type,
    short From,
    short To,
    short Count
) : IUserChangeSlotPositionRequest;
