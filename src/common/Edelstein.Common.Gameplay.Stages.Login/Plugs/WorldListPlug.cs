using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Services.Server.Contracts;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Gameplay.Stages.Login.Templates;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Util.Pipelines;
using Edelstein.Protocol.Util.Templates;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Stages.Login.Plugs;

public class WorldListPlug : IPipelinePlug<IWorldList>
{
    private readonly ILogger _logger;
    private readonly IServerService _servers;
    private readonly ITemplateManager<IWorldTemplate> _templates;

    public WorldListPlug(
        ILogger<WorldListPlug> logger,
        IServerService servers,
        ITemplateManager<IWorldTemplate> templates
    )
    {
        _logger = logger;
        _servers = servers;
        _templates = templates;
    }

    public async Task Handle(IPipelineContext ctx, IWorldList message)
    {
        var worlds = message.User.Context.Options.Worlds;

        foreach (var worldID in worlds)
        {
            var template = await _templates.Retrieve(worldID);

            if (template == null)
            {
                _logger.LogWarning("Unable to retrieve data for world {World}", worldID);
                continue;
            }

            var packet = new PacketWriter(PacketSendOperations.WorldInformation);

            packet.WriteByte(worldID);
            packet.WriteString(template.Name);
            packet.WriteByte(template.State);
            packet.WriteString(""); // WorldEventDesc
            packet.WriteShort(100); // WorldEventEXP_WSE, WorldSpecificEvent
            packet.WriteShort(100); // WorldEventDrop_WSE, WorldSpecificEvent
            packet.WriteBool(template.BlockCharCreation);

            var gameStages =
                (await _servers.GetGameByWorld(new ServerGetGameByWorldRequest(worldID))).Servers
                .OrderBy(s => s.ChannelID)
                .ToImmutableList();

            packet.WriteByte((byte)gameStages.Count);

            foreach (var stage in gameStages)
            {
                packet.WriteString(stage.ID);
                packet.WriteInt(1); // TODO: UserNo
                packet.WriteByte((byte)stage.WorldID);
                packet.WriteByte((byte)stage.ChannelID);
                packet.WriteBool(stage.IsAdultChannel);
            }

            packet.WriteShort(0); // Balloon
            packet.WriteInt(0);
            packet.WriteBool(false);

            await message.User.Dispatch(packet);
        }

        await message.User.Dispatch(
            new PacketWriter(PacketSendOperations.WorldInformation)
                .WriteInt(0xFF)
        );

        if (message.User.Account?.LatestConnectedWorld != null)
            await message.User.Dispatch(
                new PacketWriter(PacketSendOperations.LatestConnectedWorld)
                    .WriteInt((int)message.User.Account.LatestConnectedWorld)
            );
    }
}
