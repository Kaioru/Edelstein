using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Util.Buffers.Bytes;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Packets;

public class PacketHandlerManager<TStageUser> : IPacketHandlerManager<TStageUser>
    where TStageUser : IStageUser<TStageUser>
{
    private readonly Dictionary<short, IPacketHandler<TStageUser>> _handlers;
    private readonly ILogger _logger;

    public PacketHandlerManager(ILogger<PacketHandlerManager<TStageUser>> logger)
    {
        _logger = logger;
        _handlers = new Dictionary<short, IPacketHandler<TStageUser>>();
    }

    public PacketHandlerManager(
        ILogger<PacketHandlerManager<TStageUser>> logger,
        IEnumerable<IPacketHandler<TStageUser>> handlers) : this(logger)
    {
        foreach (var handler in handlers) Add(handler);
    }

    public void Add(IPacketHandler<TStageUser> handler)
    {
        if (_handlers.ContainsKey(handler.Operation))
            _logger.LogWarning(
                "Overriding packet handler for operation 0x{Operation:X} ({OperationName}) to {Handler}",
                handler.Operation, Enum.GetName((PacketRecvOperations)handler.Operation), handler.GetType().Name
            );
        else
            _logger.LogDebug(
                "Set packet handler for operation 0x{Operation:X} ({OperationName}) to {Handler}",
                handler.Operation, Enum.GetName((PacketRecvOperations)handler.Operation), handler.GetType().Name
            );
        _handlers[handler.Operation] = handler;
    }

    public void Remove(IPacketHandler<TStageUser> handler)
    {
        _logger.LogWarning(
            "Removing packet handler for operation 0x{Operation:X} ({OperationName})",
            handler.Operation, Enum.GetName((PacketRecvOperations)handler.Operation)
        );
        _handlers.Remove(handler.Operation);
    }

    public async Task Process(TStageUser user, IPacket packet)
    {
        var reader = new PacketReader(packet.Buffer);
        var operation = reader.ReadShort();
        var handler = _handlers.GetValueOrDefault(operation);

        if (handler == null)
        {
            _logger.LogWarning(
                "Unhandled packet operation 0x{Operation:X} ({OperationName})",
                operation, Enum.GetName((PacketRecvOperations)operation)
            );
            return;
        }

        if (handler.Check(user)) await handler.Handle(user, reader);

        _logger.LogDebug(
            "Handled packet operation 0x{Operation:X} ({OperationName}) with {Available} available bytes left",
            operation, Enum.GetName((PacketRecvOperations)operation), reader.Available
        );
    }
}
