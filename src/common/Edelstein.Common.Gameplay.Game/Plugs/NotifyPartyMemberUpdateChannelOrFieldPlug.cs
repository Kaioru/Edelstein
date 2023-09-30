using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Social;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class NotifyPartyMemberUpdateChannelOrFieldPlug : IPipelinePlug<NotifyPartyMemberUpdateChannelOrField>
{
    private readonly IGameStage _stage;
    
    public NotifyPartyMemberUpdateChannelOrFieldPlug(IGameStage stage) => _stage = stage;
    
    public async Task Handle(IPipelineContext ctx, NotifyPartyMemberUpdateChannelOrField message)
    {
        var users = await _stage.Users.RetrieveAll();
        var partied = users
            .Where(u => u.Party?.PartyID == message.PartyID)
            .ToImmutableHashSet();
        
        foreach (var user in partied)
        {
            if (user.Party == null) continue;
            if (user.Party.CharacterID == message.CharacterID)
            {
                user.Party.ChannelID = message.ChannelID;
                user.Party.FieldID = message.FieldID;
            }

            if (user.Party.Members.TryGetValue(message.CharacterID, out var member))
            {
                member.ChannelID = message.ChannelID;
                member.FieldID = message.FieldID;
            }
            
            var p = new PacketWriter(PacketSendOperations.PartyResult);
            p.WriteByte((byte)PartyResultOperations.UserMigration);
            p.WriteInt(message.PartyID);
            p.WritePartyInfo(user.Party);
            _ = user.Dispatch(p.Build());
        }
    }
}
