using System.Net;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Services.Server.Contracts;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Server.Contracts;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Plugs;

public class UserTransferChannelRequestPlug : IPipelinePlug<IUserTransferChannelRequest>
{
    private readonly IServerService _serverService;

    public UserTransferChannelRequestPlug(IServerService serverService) => _serverService = serverService;

    public async Task Handle(IPipelineContext ctx, IUserTransferChannelRequest message)
    {
        var response = await _serverService.GetGameByWorldAndChannel(new ServerGetGameByWorldAndChannelRequest(
            message.User.StageUser.Context.Options.WorldID,
            message.ChannelID
        ));

        if (response.Result != ServerResult.Success || response.Server == null) return;

        var packet = new PacketWriter(PacketSendOperations.MigrateCommand);

        var endpoint = new IPEndPoint(IPAddress.Parse(response.Server.Host), response.Server.Port);
        var address = endpoint.Address.MapToIPv4().GetAddressBytes();
        var port = endpoint.Port;

        packet.WriteBool(true);

        foreach (var b in address)
            packet.WriteByte(b);
        packet.WriteShort((short)port);

        await message.User.StageUser.OnMigrateOut(response.Server.ID);
        await message.User.Dispatch(packet);
    }
}
