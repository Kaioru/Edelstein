using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Social;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Services.Social.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

public class FieldOnPacketPartyInviteRequestPlug : IPipelinePlug<FieldOnPacketPartyInviteRequest>
{
    public async Task Handle(IPipelineContext ctx, FieldOnPacketPartyInviteRequest message)
    {
        if (message.User.StageUser.Party == null) return;

        var response = await message.User.StageUser.Context.Services.Party.Invite(new PartyInviteRequest(
            message.User.Character.ID,
            message.User.Character.Name,
            message.User.Character.Level,
            message.User.Character.Job,
            message.User.StageUser.Party.ID,
            message.CharacterName
        ));
        
        var result = response.Result switch
        {
            PartyResult.Success => PartyResultOperations.InvitePartySent,
            PartyResult.FailedAlreadyInvited => PartyResultOperations.InvitePartyAlreadyInvitedByInviter,
            _ => PartyResultOperations.InvitePartyAlreadyInvited
        };
        using var packet = new PacketWriter(PacketSendOperations.PartyResult);
        packet.WriteByte((byte)result);
        if (result == PartyResultOperations.InvitePartySent)
            packet.WriteString(message.CharacterName);
        await message.User.Dispatch(packet.Build());
    }
}
