using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Network.Packets;
using Edelstein.Protocol.Gameplay.Stages.Login.Messages;
using Edelstein.Protocol.Gameplay.Stages.Login.Templates;
using Edelstein.Protocol.Util.Pipelines;
using Edelstein.Protocol.Util.Templates;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Stages.Login.Plugs;

public class WorldRequestPlug : IPipelinePlug<IWorldRequest>
{
    private readonly ILogger _logger;
    private readonly ITemplateManager<IWorldTemplate> _templates;

    public WorldRequestPlug(ILogger<WorldRequestPlug> logger, ITemplateManager<IWorldTemplate> templates)
    {
        _logger = logger;
        _templates = templates;
    }

    public async Task Handle(IPipelineContext ctx, IWorldRequest message)
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

            var packet = new PacketOut(PacketSendOperations.WorldInformation);

            packet.WriteByte(worldID);
            packet.WriteString(template.Name);
            packet.WriteByte(template.State);
            packet.WriteString(""); // WorldEventDesc
            packet.WriteShort(0); // WorldEventEXP_WSE, WorldSpecificEvent
            packet.WriteShort(0); // WorldEventDrop_WSE, WorldSpecificEvent
            packet.WriteBool(template.BlockCharCreation);

            packet.WriteByte(0);
            packet.WriteShort(0);

            await message.User.Dispatch(packet);
        }

        await message.User.Dispatch(new PacketOut(PacketSendOperations.WorldInformation).WriteByte(0xFF));
        await message.User.Dispatch(new PacketOut(PacketSendOperations.LatestConnectedWorld).WriteInt(0));
    }
}
