using Edelstein.Protocol.Gameplay.Inventories;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts;

public interface IUserSortItemRequest : IFieldUserContract
{
    ItemInventoryType Type { get; }
}
