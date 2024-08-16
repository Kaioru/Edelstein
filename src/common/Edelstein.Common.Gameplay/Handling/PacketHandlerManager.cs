using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Network.Packets;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Handling;

public class PacketHandlerManager<TStageSystem, TStageSystemUser>(
    ILogger logger
) : IPacketHandlerManager<TStageSystem, TStageSystemUser>
    where TStageSystemUser : IStageSystemUser<TStageSystem, TStageSystemUser>
    where TStageSystem : IStageSystem<TStageSystem, TStageSystemUser>
{
    private readonly Dictionary<short, IPacketHandler<TStageSystem, TStageSystemUser>> _handlers = new();

    public PacketHandlerManager(
        ILogger logger,
        IEnumerable<IPacketHandler<TStageSystem, TStageSystemUser>> handlers) : 
        this(logger)
    {
        foreach (var handler in handlers) Add(handler);
    }

    public void Add(IPacketHandler<TStageSystem, TStageSystemUser> handler)
    {
        if (_handlers.ContainsKey(handler.Operation))
            logger.LogPacketHandlerOverridden(
                handler.Operation, 
                Enum.GetName((PacketRecvOperation)handler.Operation)!, 
                handler.GetType().Name
            );
        else
            logger.LogPacketHandlerAdded(
                handler.Operation, 
                Enum.GetName((PacketRecvOperation)handler.Operation)!, 
                handler.GetType().Name
            );
        
        _handlers[handler.Operation] = handler;
    }

    public void Remove(IPacketHandler<TStageSystem, TStageSystemUser> handler)
        => _handlers.Remove(handler.Operation);

    public void Remove(short operation) 
        => _handlers.Remove(operation);
    
    public async Task Process(TStageSystemUser user, IRawPacket packet)
    {
        var operation = BitConverter.ToInt16(packet.Buffer.Span);

        if (_handlers.TryGetValue(operation, out var handler))
        {
            await handler.Handle(user, packet);
            
            logger.LogPacketHandlerHandled(
                operation, 
                Enum.GetName((PacketRecvOperation)operation)!
            );
        }
        else
            logger.LogPacketHandlerNotFound(
                operation, 
                Enum.GetName((PacketRecvOperation)operation)!
            );
    }
}
