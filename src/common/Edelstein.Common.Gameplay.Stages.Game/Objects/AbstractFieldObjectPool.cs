using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects;

public abstract class AbstractFieldObjectPool : IFieldObjectPool
{
    public abstract IReadOnlyCollection<IFieldObject> Objects { get; }

    public abstract Task Enter(IFieldObject obj);
    public abstract Task Leave(IFieldObject obj);

    public abstract IFieldObject? GetObject(int id);
    public abstract IEnumerable<IFieldObject> GetObjects();


    public Task Dispatch(IPacket packet) =>
        Task.WhenAll(Objects
            .OfType<IAdapter>()
            .Select(a => a.Dispatch(packet)));

    public Task Dispatch(IPacket packet, IFieldObject obj) =>
        Task.WhenAll(Objects
            .OfType<IAdapter>()
            .Where(a => a != obj)
            .Select(a => a.Dispatch(packet)));
}
