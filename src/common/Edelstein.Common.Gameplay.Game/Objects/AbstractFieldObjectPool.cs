using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Game.Objects;

public abstract class AbstractFieldObjectPool : IFieldObjectPool
{
    public abstract IReadOnlyCollection<IFieldObject> Objects { get; }

    public abstract Task Enter(IFieldObject obj);
    public abstract Task Leave(IFieldObject obj);

    public abstract IFieldObject? GetObject(int id);

    public virtual Task Dispatch(IPacket packet) =>
        Task.WhenAll(Objects
            .OfType<IAdapter>()
            .Select(a => a.Dispatch(packet)));

    public virtual Task Dispatch(IPacket packet, IFieldObject obj) =>
        Task.WhenAll(Objects
            .OfType<IAdapter>()
            .Where(a => a != obj)
            .Select(a => a.Dispatch(packet)));
}
