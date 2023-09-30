using System.Collections.Frozen;
using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Social;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class NotifyPartyDisbandedPlug : IPipelinePlug<NotifyPartyDisbanded>
{
    private readonly IGameStage _stage;

    public NotifyPartyDisbandedPlug(IGameStage stage) => _stage = stage;
    
    public async Task Handle(IPipelineContext ctx, NotifyPartyDisbanded message)
    {
        var users = await _stage.Users.RetrieveAll();
        var partied = users
            .Where(u => u.Party?.PartyID == message.PartyID)
            .ToImmutableHashSet();

        foreach (var user in partied)
        {
            if (user.Party == null) continue;

            user.Party = null;
            
            var p = new PacketWriter(PacketSendOperations.PartyResult);
            p.WriteByte((byte)PartyResultOperations.WithdrawPartyDone);
            p.WriteInt(message.PartyID);
            p.WriteInt(message.CharacterID);
            p.WriteBool(false);
            _ = user.Dispatch(p.Build());
        }
    }
}
