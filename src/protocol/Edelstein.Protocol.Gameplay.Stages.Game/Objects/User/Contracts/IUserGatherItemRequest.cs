using Edelstein.Protocol.Gameplay.Inventories;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts;

public interface IUserGatherItemRequest : IFieldUserContract
{
    ItemInventoryType Type { get; }
}
