using Edelstein.Protocol.Gameplay.Inventories;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts.Pipelines;

public interface IUserSortItemRequest : IFieldUserContract
{
    ItemInventoryType Type { get; }
}
