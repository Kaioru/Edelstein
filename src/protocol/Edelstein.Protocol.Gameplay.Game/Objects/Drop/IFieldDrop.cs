using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Objects.Drop;

public interface IFieldDrop : IFieldObject
{
    bool IsMoney { get; }
    int Info { get; }
    
    DropOwnType OwnType { get; }
    int OwnerID { get; }
    int SourceID { get; }

    Task PickUp(IFieldUser user);
}
