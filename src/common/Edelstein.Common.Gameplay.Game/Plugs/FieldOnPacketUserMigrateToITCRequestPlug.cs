using System.Net;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Server.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class FieldOnPacketUserMigrateToITCRequestPlug : IPipelinePlug<FieldOnPacketUserMigrateToITCRequest>
{
    private readonly IServerService _serverService;

    public FieldOnPacketUserMigrateToITCRequestPlug(IServerService serverService) => _serverService = serverService;
    
    public async Task Handle(IPipelineContext ctx, FieldOnPacketUserMigrateToITCRequest message)
    {
        var response = await _serverService.GetTradeByWorld(new ServerGetTradeByWorldRequest(
            message.User.StageUser.Context.Options.WorldID
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
