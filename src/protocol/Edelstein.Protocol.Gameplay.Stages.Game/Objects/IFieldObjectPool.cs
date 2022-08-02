using Edelstein.Protocol.Util.Buffers.Bytes;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects;

public interface IFieldObjectPool
{
    Task Enter(IFieldObject obj);
    Task Leave(IFieldObject obj);

    IFieldObject GetObject(int id);
    T GetObject<T>(int id) where T : IFieldObject;

    IEnumerable<IFieldObject> GetObjects();
    IEnumerable<T> GetObjects<T>() where T : IFieldObject;

    Task Dispatch(IPacket packet);
    Task Dispatch(IPacket packet, IFieldObject source);
}
