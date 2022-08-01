using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Login.Types;
using Edelstein.Common.Services.Server.Contracts;
using Edelstein.Common.Util.Buffers.Bytes;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Messages;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Server.Contracts;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Plugs;

public class SelectWorldPlug : IPipelinePlug<ISelectWorld>
{
    private readonly IServerService _serverService;

    public SelectWorldPlug(IServerService serverService) => _serverService = serverService;

    public async Task Handle(IPipelineContext ctx, ISelectWorld message)
    {
        var packet = new ByteWriter(PacketSendOperations.SelectWorldResult);
        var gameStage = await _serverService.GetGameByWorldAndChannel(
            new ServerGetGameByWorldAndChannelRequest(
                message.WorldID,
                message.ChannelIndex + 1
            )
        );
        var result = gameStage.Result == ServerResult.Success || gameStage.Server == null
            ? LoginResult.Success
            : LoginResult.DBFail;

        packet.WriteByte((byte)result);

        if (result == LoginResult.Success)
        {
            message.User.State = LoginState.SelectCharacter;
            message.User.SelectedWorldID = (byte)gameStage.Server!.WorldID;
            message.User.SelectedChannelID = (byte)gameStage.Server!.ChannelID;

            packet.WriteByte(0);
            packet.WriteBool(true);
            packet.WriteInt(0);
            packet.WriteInt(0);
        }

        await message.User.Dispatch(packet);
    }
}
