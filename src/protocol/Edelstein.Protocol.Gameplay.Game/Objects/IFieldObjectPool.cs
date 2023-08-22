using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Objects;

public interface IFieldObjectPool
{
    IReadOnlyCollection<IFieldObject> Objects { get; }

    Task Enter(IFieldObject obj);
    Task Leave(IFieldObject obj);

    IFieldObject? GetObject(int id);

    Task Dispatch(IPacket packet);
    Task Dispatch(IPacket packet, IFieldObject obj);
}
