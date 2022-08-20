using Edelstein.Protocol.Gameplay.Inventories;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Contracts.Pipelines;

public record UserChangeSlotPositionRequest(
    IFieldUser User,
    ItemInventoryType Type,
    short From,
    short To,
    short Count
) : IUserChangeSlotPositionRequest;
