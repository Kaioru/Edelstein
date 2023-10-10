using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Utilities.Packets;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Login.Handlers;

public class ClientDumpLogHandler : IPacketHandler<IGameStageUser>
{
    private ILogger _logger;
    
    public ClientDumpLogHandler(ILogger<ClientDumpLogHandler> logger) => _logger = logger;
    
    public short Operation => (short)PacketRecvOperations.ClientDumpLog;

    public bool Check(IGameStageUser user) => true;
    public Task Handle(IGameStageUser user, IPacketReader reader)
    {
        var callType = reader.ReadShort();
        var errorCode = reader.ReadInt();
        var backupBufferSize = reader.ReadShort();
        var rawSeq = reader.ReadInt();
        var type = reader.ReadShort();
        var backupBuffer = reader.ReadBytes((short)(backupBufferSize - 6));

        _logger.LogWarning(
            "Received client dump log of type {Type} with buffer {Buffer}",
            (PacketSendOperations)type,
            Convert.ToHexString(backupBuffer )
        );
        
        return Task.CompletedTask;
    }
}
