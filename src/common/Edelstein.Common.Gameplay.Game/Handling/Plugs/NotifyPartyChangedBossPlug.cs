using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Social;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

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

            using var packet = new PacketWriter(PacketSendOperations.PartyResult);
            packet.WriteByte((byte)PartyResultOperations.ChangePartyBossDone);
            packet.WriteInt(message.BossID);
            packet.WriteBool(message.IsDisconnected);
            _ = user.Dispatch(packet.Build());
        }
    }
}
