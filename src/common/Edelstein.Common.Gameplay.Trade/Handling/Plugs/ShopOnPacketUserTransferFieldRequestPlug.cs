﻿using System.Net;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Trade.Contracts;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Server.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Trade.Handling.Plugs;

public class ShopOnPacketUserTransferFieldRequestPlug : IPipelinePlug<TradeOnPacketUserTransferFieldRequest>
{
    private readonly IServerService _serverService;
    
    public ShopOnPacketUserTransferFieldRequestPlug(IServerService serverService) 
        => _serverService = serverService;

    public async Task Handle(IPipelineContext ctx, TradeOnPacketUserTransferFieldRequest message)
    {
        if (message.User.FromServerID == null) return;
        
        var response = await _serverService.GetByID(new ServerGetByIDRequest(
            message.User.FromServerID
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
        
        await message.User.Migrate(server.ID, packet.Build());
    }
}
