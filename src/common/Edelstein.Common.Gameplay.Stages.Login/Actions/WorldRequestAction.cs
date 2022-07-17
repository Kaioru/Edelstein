using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Network.Packets;
using Edelstein.Protocol.Gameplay.Stages.Login.Messages;
using Edelstein.Protocol.Util.Pipelines;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Stages.Login.Actions;

public class WorldRequestAction : IPipelineAction<IWorldRequest>
{
    private readonly ILogger _logger;

    public WorldRequestAction(ILogger<WorldRequestAction> logger) => _logger = logger;

    public async Task Handle(IPipelineContext ctx, IWorldRequest message) =>
        // TODO: loading worlds
        await message.User.Dispatch(new PacketOut(PacketSendOperations.WorldInformation).WriteByte(0xFF));
    // await message.User.Dispatch(new PacketOut(PacketSendOperations.LatestConnectedWorld).WriteByte(0));
}
