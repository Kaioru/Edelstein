using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Social;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Services.Social.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class FieldOnPacketPartyInviteAcceptResultPlug : IPipelinePlug<FieldOnPacketPartyInviteAcceptResult>
{
    public async Task Handle(IPipelineContext ctx, FieldOnPacketPartyInviteAcceptResult message)
    {
        var response = await message.User.StageUser.Context.Services.Party.InviteAccept(new PartyInviteAcceptRequest(
            message.User.Character.ID,
            message.User.Character.Name,
            message.User.Character.Job,
            message.User.Character.Level,
            message.User.StageUser.Context.Options.ChannelID,
            message.User.Field?.ID ?? 999999999,
            message.PartyID
        ));
        
        if (response.Result == PartyResult.Success) return;
        
        var result = response.Result switch
        {
            PartyResult.FailedFull => PartyResultOperations.JoinPartyAlreadyFull,
            PartyResult.FailedAlreadyInParty => PartyResultOperations.JoinPartyAlreadyJoined,
            _ => PartyResultOperations.JoinPartyUnknown
        };
        var p = new PacketWriter(PacketSendOperations.PartyResult);
        p.WriteByte((byte)result);
        await message.User.Dispatch(p.Build());
    }
}
