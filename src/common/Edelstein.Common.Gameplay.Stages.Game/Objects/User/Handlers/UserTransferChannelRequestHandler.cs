using System.Net;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Services.Server.Contracts;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Server.Contracts;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Handlers;

public class UserTransferChannelRequestHandler : AbstractFieldUserHandler
{
    private IServerService _servers;

    public UserTransferChannelRequestHandler(IServerService servers) => _servers = servers;

    public override short Operation => (short)PacketRecvOperations.UserTransferChannelRequest;

    protected override async Task Handle(IFieldUser user, IPacketReader reader)
    {
        var channelID = reader.ReadByte();

        var response = await _servers.GetGameByWorldAndChannel(new ServerGetGameByWorldAndChannelRequest(
            user.StageUser.Context.Options.WorldID,
            channelID
        ));

        if (response.Result != ServerResult.Success || response.Server == null) return;

        var packet = new PacketWriter(PacketSendOperations.MigrateCommand);

        var endpoint = new IPEndPoint(IPAddress.Parse("8.31.99.141"), response.Server.Port);
        var address = endpoint.Address.MapToIPv4().GetAddressBytes();
        var port = endpoint.Port;

        packet.WriteBool(true);

        foreach (var b in address)
            packet.WriteByte(b);
        packet.WriteShort((short)port);
        packet.WriteInt(0);

        await user.StageUser.OnMigrateOut(response.Server.ID);
        await user.Dispatch(packet);
    }
}
