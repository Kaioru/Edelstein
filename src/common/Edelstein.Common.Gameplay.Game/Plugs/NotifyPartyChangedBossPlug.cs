using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Social;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class NotifyPartyChangedBossPlug : IPipelinePlug<NotifyPartyChangedBoss>
{
    private readonly IGameStage _stage;
    
    public NotifyPartyChangedBossPlug(IGameStage stage) => _stage = stage;
    
    public async Task Handle(IPipelineContext ctx, NotifyPartyChangedBoss message)
    {
        var users = await _stage.Users.RetrieveAll();
        var partied = users.Where(u => u.Party?.PartyID == message.PartyID).ToImmutableList();
        
        foreach (var user in partied)
        {
            if (user.Party == null) continue;
            user.Party.BossCharacterID = message.BossID;

            var p = new PacketWriter(PacketSendOperations.PartyResult);
            p.WriteByte((byte)PartyResultOperations.ChangePartyBossDone);
            p.WriteInt(message.BossID);
            p.WriteBool(message.IsDisconnected);
            _ = user.Dispatch(p.Build());
        }
    }
}
