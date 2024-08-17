using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Network.Packets;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Handling;

public class PacketHandlerManager<TStageSystem, TStageSystemUser>(
    ILogger<PacketHandlerManager<TStageSystem, TStageSystemUser>> logger
) : IPacketHandlerManager<TStageSystem, TStageSystemUser>
    where TStageSystemUser : IStageSystemUser<TStageSystem, TStageSystemUser>
    where TStageSystem : IStageSystem<TStageSystem, TStageSystemUser>
{
    private readonly Dictionary<short, IPacketHandler<TStageSystem, TStageSystemUser>> _handlers = new();

    public void Add(short operation, IPacketHandler<TStageSystem, TStageSystemUser> handler)
    {
        if (_handlers.ContainsKey(operation))
            logger.LogPacketHandlerOverridden(
                operation, 
                Enum.GetName((PacketRecvOperation)operation)!, 
                handler.GetType().Name
            );
        else
            logger.LogPacketHandlerAdded(
                operation, 
                Enum.GetName((PacketRecvOperation)operation)!, 
                handler.GetType().Name
            );
        
        _handlers[operation] = handler;
    }

    public void Remove(short operation, IPacketHandler<TStageSystem, TStageSystemUser> handler)
        => _handlers.Remove(operation);

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
