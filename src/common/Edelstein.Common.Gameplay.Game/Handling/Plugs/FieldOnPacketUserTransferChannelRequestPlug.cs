using System.Net;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Server.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

public class FieldOnPacketUserTransferChannelRequestPlug : IPipelinePlug<FieldOnPacketUserTransferChannelRequest>
{
    private readonly IServerService _serverService;
    
    public FieldOnPacketUserTransferChannelRequestPlug(IServerService serverService) 
        => _serverService = serverService;
    
    public async Task Handle(IPipelineContext ctx, FieldOnPacketUserTransferChannelRequest message)
    {
        var response = await _serverService.GetGameByWorldAndChannel(new ServerGetGameByWorldAndChannelRequest(
            message.User.StageUser.Context.Options.WorldID,
            message.ChannelID
        ));
        var server = response.Server;
        if (server == null) return;
        
        var packet = new PacketWriter(PacketSendOperations.MigrateCommand);
        var endpoint = new IPEndPoint(IPAddress.Parse(server.Host), server.Port);
        var address = endpoint.Address.MapToIPv4().GetAddressBytes();
        var port = (short)endpoint.Port;
        
        packet.WriteBool(true);
        foreach (var b in address) 
            packet.WriteByte(b);
        packet.WriteShort(port);
        
        await message.User.StageUser.Migrate(server.ID, packet.Build());
    }
}
