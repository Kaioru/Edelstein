using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Common.Gameplay.Packets;

public class PacketHandlerManager<TStageUser> : IPacketHandlerManager<TStageUser> where TStageUser : IStageUser
{
    private readonly Dictionary<short, IPacketHandler<TStageUser>> _handlers;

    public PacketHandlerManager() => _handlers = new Dictionary<short, IPacketHandler<TStageUser>>();

    public PacketHandlerManager(IEnumerable<IPacketHandler<TStageUser>> handlers) : this()
    {
        foreach (var handler in handlers) Add(handler);
    }

    public void Add(IPacketHandler<TStageUser> handler) =>
        _handlers[handler.Operation] = handler;

    public void Remove(IPacketHandler<TStageUser> handler) =>
        _handlers.Remove(handler.Operation);

    public Task Process(TStageUser user, IPacketReader reader)
    {
        var operation = reader.ReadShort();
        var handler = _handlers.GetValueOrDefault(operation);

        if (handler != null && handler.Check(user))
            return handler.Handle(user, reader);
        return Task.CompletedTask;
    }
}
