using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Game.Objects.Drop;

public interface IFieldDrop : IFieldObject
{
    bool IsMoney { get; }
    int Info { get; }
    
    DropOwnType OwnType { get; }
    int OwnerID { get; }
    int SourceID { get; }

    Task Pickup(IFieldUser user);
}
