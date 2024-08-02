using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Network.Packets;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Handling;

public class PacketHandlerManager<TStageSystemUser, TStageSystem>(
    ILogger logger
) : IPacketHandlerManager<TStageSystemUser, TStageSystem>
    where TStageSystemUser : IStageSystemUser<TStageSystemUser, TStageSystem>
    where TStageSystem : IStageSystem<TStageSystemUser, TStageSystem>
{
    private readonly Dictionary<short, IPacketHandler<TStageSystemUser, TStageSystem>> _handlers = new();

    public PacketHandlerManager(
        ILogger logger,
        IEnumerable<IPacketHandler<TStageSystemUser, TStageSystem>> handlers) : 
        this(logger)
    {
        foreach (var handler in handlers) Add(handler);
    }

    public void Add(IPacketHandler<TStageSystemUser, TStageSystem> handler)
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

    public void Remove(IPacketHandler<TStageSystemUser, TStageSystem> handler)
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
