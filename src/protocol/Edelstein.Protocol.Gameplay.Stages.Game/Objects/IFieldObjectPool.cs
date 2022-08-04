using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects;

public interface IFieldObjectPool :
    IFieldUserFactory,
    IFieldNPCFactory
{
    IReadOnlyCollection<IFieldObject> Objects { get; }

    Task Enter(IFieldObject obj);
    Task Leave(IFieldObject obj);

    IFieldObject? GetObject(int id);
    IEnumerable<IFieldObject> GetObjects();

    Task Dispatch(IPacket packet);
    Task Dispatch(IPacket packet, IFieldObject obj);
}
