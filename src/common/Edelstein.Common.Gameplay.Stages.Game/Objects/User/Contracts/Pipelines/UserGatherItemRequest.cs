using Edelstein.Protocol.Gameplay.Inventories;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Contracts.Pipelines;

public record UserGatherItemRequest(
    IFieldUser User,
    ItemInventoryType Type
) : IUserGatherItemRequest;
