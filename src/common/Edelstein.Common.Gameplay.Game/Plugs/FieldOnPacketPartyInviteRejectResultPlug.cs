using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Social;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Services.Social.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class FieldOnPacketPartyInviteRejectResultPlug : IPipelinePlug<FieldOnPacketPartyInviteRejectResult>
{
    public async Task Handle(IPipelineContext ctx, FieldOnPacketPartyInviteRejectResult message)
    {
        var response = await message.User.StageUser.Context.Services.Party.InviteReject(new PartyInviteRejectRequest(
            message.User.Character.ID,
            message.User.Character.Name,
            message.PartyID
        ));
        
        if (response.Result == PartyResult.Success) return;
        
        var result = response.Result switch
        {
            _ => PartyResultOperations.JoinPartyUnknown
        };
        var p = new PacketWriter(PacketSendOperations.PartyResult);
        p.WriteByte((byte)result);
        await message.User.Dispatch(p.Build());
    }
}
