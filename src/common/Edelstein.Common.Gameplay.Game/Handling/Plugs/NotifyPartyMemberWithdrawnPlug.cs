using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Social;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

public class NotifyPartyMemberWithdrawnPlug : IPipelinePlug<NotifyPartyMemberWithdrawn>
{
    private readonly IGameStage _stage;
    
    public NotifyPartyMemberWithdrawnPlug(IGameStage stage) => _stage = stage;
    
    public async Task Handle(IPipelineContext ctx, NotifyPartyMemberWithdrawn message)
    {
        var users = await _stage.Users.RetrieveAll();
        var partied = users.Where(u => u.Party?.PartyID == message.PartyID).ToImmutableList();
        
        foreach (var user in partied)
        {
            if (user.Party == null) continue;
            
            user.Party.Members.Remove(message.CharacterID);
            
            var p = new PacketWriter(PacketSendOperations.PartyResult);
            p.WriteByte((byte)PartyResultOperations.WithdrawPartyDone);
            p.WriteInt(message.PartyID);
            p.WriteInt(message.CharacterID);
            p.WriteBool(true);
            p.WriteBool(message.IsKicked);
            p.WriteString(message.CharacterName);
            p.WritePartyInfo(user.Party);

            if (user.Party.CharacterID == message.CharacterID)
                user.Party = null;
            
            _ = user.Dispatch(p.Build());
        }
    }
}
