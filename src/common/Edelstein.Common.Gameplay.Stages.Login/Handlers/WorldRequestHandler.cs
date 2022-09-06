using System.Collections.Immutable;
using Edelstein.Common.Services.Server.Contracts;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Templates;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Templates;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers;

public class WorldRequestHandler : AbstractLoginPacketHandler
{
    private readonly ILogger _logger;
    private readonly IServerService _serverService;
    private readonly ITemplateManager<IWorldTemplate> _templates;

    public WorldRequestHandler(
        ILogger<WorldRequestHandler> logger,
        IServerService serverService,
        ITemplateManager<IWorldTemplate> templates
    )
    {
        _logger = logger;
        _serverService = serverService;
        _templates = templates;
    }

    public override short Operation => 160;
    public override bool Check(ILoginStageUser user) => user.State == LoginState.SelectWorld;

    public override async Task Handle(ILoginStageUser user, IPacketReader reader)
    {
        var worlds = user.Context.Options.Worlds;

        foreach (var worldID in worlds)
        {
            var template = await _templates.Retrieve(worldID);

            if (template == null)
            {
                _logger.LogWarning("Unable to retrieve data for world {World}", worldID);
                continue;
            }

            var packet = new PacketWriter();

            packet.WriteShort(1);

            packet.WriteByte(worldID);
            packet.WriteString(template.Name);
            packet.WriteByte(template.State);
            packet.WriteString(""); // WorldEventDesc
            packet.WriteShort(100); // WorldEventEXP_WSE, WorldSpecificEvent
            packet.WriteShort(100); // WorldEventDrop_WSE, WorldSpecificEvent
            packet.WriteBool(template.BlockCharCreation);

            var gameStages =
                (await _serverService.GetGameByWorld(new ServerGetGameByWorldRequest(worldID))).Servers
                .OrderBy(s => s.ChannelID)
                .ToImmutableList();

            packet.WriteByte((byte)gameStages.Count);

            foreach (var stage in gameStages)
            {
                packet.WriteString(stage.ID);
                packet.WriteInt(0); // TODO: UserNo
                packet.WriteByte((byte)stage.WorldID);
                packet.WriteByte((byte)stage.ChannelID);
                packet.WriteBool(stage.IsAdultChannel);
            }

            packet.WriteShort(0); // Balloon
            packet.WriteInt(0);
            packet.WriteBool(false);

            await user.Dispatch(packet);
        }


        var packet2 = new PacketWriter();

        packet2.WriteShort(1);
        packet2.WriteInt(255);

        await user.Dispatch(packet2);
    }
}
