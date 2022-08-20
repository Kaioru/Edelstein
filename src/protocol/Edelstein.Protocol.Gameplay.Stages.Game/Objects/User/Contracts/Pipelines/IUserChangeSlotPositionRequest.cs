using Edelstein.Protocol.Gameplay.Inventories;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts.Pipelines;

public interface IUserChangeSlotPositionRequest : IFieldUserContract
{
    ItemInventoryType Type { get; }
    short From { get; }
    short To { get; }
    short Count { get; }
}
