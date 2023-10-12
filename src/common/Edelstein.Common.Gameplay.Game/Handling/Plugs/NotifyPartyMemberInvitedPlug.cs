using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Social;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

public class NotifyPartyMemberInvitedPlug : IPipelinePlug<NotifyPartyMemberInvited>
{
    private readonly IGameStage _stage;

    public NotifyPartyMemberInvitedPlug(IGameStage stage) => _stage = stage;
    
    public async Task Handle(IPipelineContext ctx, NotifyPartyMemberInvited message)
    {
        var user = await _stage.Users.Retrieve(message.CharacterID);
        if (user == null) return;
        
        var p = new PacketWriter(PacketSendOperations.PartyResult);
        p.WriteByte((byte)PartyRequestOperations.InviteParty);
        p.WriteInt(message.PartyID);
        p.WriteString(message.InviterName);
        p.WriteInt(message.InviterLevel);
        p.WriteInt(message.InviterJob);
        p.WriteByte(0);
        _ = user.Dispatch(p.Build());
    }
}
