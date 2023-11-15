using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Social;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

public class NotifyPartyDisbandedPlug : IPipelinePlug<NotifyPartyDisbanded>
{
    private readonly IGameStage _stage;

    public NotifyPartyDisbandedPlug(IGameStage stage) => _stage = stage;
    
    public async Task Handle(IPipelineContext ctx, NotifyPartyDisbanded message)
    {
        var users = await _stage.Users.RetrieveAll();
        var partied = users
            .Where(u => u.Party?.PartyID == message.PartyID)
            .ToImmutableArray();

        foreach (var user in partied)
        {
            if (user.Party == null) continue;

            user.Party = null;
            
            using var packet = new PacketWriter(PacketSendOperations.PartyResult);
            packet.WriteByte((byte)PartyResultOperations.WithdrawPartyDone);
            packet.WriteInt(message.PartyID);
            packet.WriteInt(message.CharacterID);
            packet.WriteBool(false);
            _ = user.Dispatch(packet.Build());
        }
    }
}
