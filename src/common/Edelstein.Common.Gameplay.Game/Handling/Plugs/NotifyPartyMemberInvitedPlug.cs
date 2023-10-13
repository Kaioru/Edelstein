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
        
        using var packet = new PacketWriter(PacketSendOperations.PartyResult);
        packet.WriteByte((byte)PartyRequestOperations.InviteParty);
        packet.WriteInt(message.PartyID);
        packet.WriteString(message.InviterName);
        packet.WriteInt(message.InviterLevel);
        packet.WriteInt(message.InviterJob);
        packet.WriteByte(0);
        _ = user.Dispatch(packet.Build());
    }
}
