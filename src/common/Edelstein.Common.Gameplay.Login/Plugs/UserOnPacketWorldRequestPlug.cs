using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Gameplay.Login.Templates;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Server.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;
using Edelstein.Protocol.Utilities.Templates;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Login.Plugs;

public class UserOnPacketWorldRequestPlug : IPipelinePlug<UserOnPacketWorldRequest>
{
    private readonly ILogger _logger;
    private readonly IServerService _serverService;
    private readonly ITemplateManager<IWorldTemplate> _templates;

    public UserOnPacketWorldRequestPlug(
        ILogger<UserOnPacketWorldRequest> logger,
        ITemplateManager<IWorldTemplate> templates,
        IServerService serverService)
    {
        _logger = logger;
        _templates = templates;
        _serverService = serverService;
    }

    public async Task Handle(IPipelineContext ctx, UserOnPacketWorldRequest message)
    {
        var worlds = message.User.Context.Options.Worlds;

        foreach (var worldID in worlds)
        {
            var template = await _templates.Retrieve(worldID);

            using var packet = new PacketWriter(PacketSendOperations.WorldInformation);

            packet.WriteByte(worldID);
            packet.WriteString(template?.Name ?? "NO-NAME");
            packet.WriteByte(template?.State ?? 0);
            packet.WriteString(""); // WorldEventDesc
            packet.WriteShort(0); // WorldEventEXP_WSE, WorldSpecificEvent
            packet.WriteShort(0); // WorldEventDrop_WSE, WorldSpecificEvent
            packet.WriteBool(template?.BlockCharCreation ?? false);

            var gameStages =
                (await _serverService.GetGameByWorld(new ServerGetGameByWorldRequest(worldID))).Servers
                .OrderBy(s => s.ChannelID)
                .ToImmutableArray();

            packet.WriteByte((byte)gameStages.Length);

            foreach (var stage in gameStages)
            {
                packet.WriteString(stage.ID);
                packet.WriteInt(0); // TODO: UserNo
                packet.WriteByte((byte)stage.WorldID);
                packet.WriteByte((byte)stage.ChannelID);
                packet.WriteBool(stage.IsAdultChannel);
            }

            packet.WriteShort(0); // TODO: Balloon

            await message.User.Dispatch(packet.Build());
        }

        await message.User.Dispatch(new PacketWriter(PacketSendOperations.WorldInformation).WriteByte(0xFF).Build());
    }
}
