using Edelstein.Protocol.Gameplay.Inventories;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts.Pipelines;

public interface IUserGatherItemRequest : IFieldUserContract
{
    ItemInventoryType Type { get; }
}
