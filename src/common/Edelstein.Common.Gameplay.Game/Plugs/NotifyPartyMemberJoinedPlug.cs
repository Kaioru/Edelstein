using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Social;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class NotifyPartyMemberJoinedPlug : IPipelinePlug<NotifyPartyMemberJoined>
{
    private readonly IGameStage _stage;

    public NotifyPartyMemberJoinedPlug(IGameStage stage) => _stage = stage;
    
    public async Task Handle(IPipelineContext ctx, NotifyPartyMemberJoined message)
    {
        
        var users = await _stage.Users.RetrieveAll();
        var partied = users
            .Where(u => 
                u.Party?.PartyID == message.PartyID || 
                u.Character?.ID == message.PartyMember.CharacterID)
            .ToImmutableArray();
        
        foreach (var user in partied)
        {
            if (user.Character == null) continue;
            if (user.Character.ID == message.PartyMember.CharacterID)
                user.Party = message.Party;
            else
                user.Party?.Members.Add(message.PartyMember.CharacterID, message.PartyMember);

            if (user.Party == null) continue;
            
            var p = new PacketWriter(PacketSendOperations.PartyResult);
            p.WriteByte((byte)PartyResultOperations.JoinPartyDone);
            p.WriteInt(message.PartyID);
            p.WriteString(message.PartyMember.CharacterName);
            p.WritePartyInfo(user.Party);
            _ = user.Dispatch(p.Build());
        }
    }
}
