using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Social;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Services.Social.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

public class FieldOnPacketPartyCreateRequestPlug : IPipelinePlug<FieldOnPacketPartyCreateRequest>
{
    public async Task Handle(IPipelineContext ctx, FieldOnPacketPartyCreateRequest message)
    {
        var response = await message.User.StageUser.Context.Services.Party.Create(new PartyCreateRequest(
            message.User.Character.ID,
            message.User.Character.Name,
            message.User.Character.Job,
            message.User.Character.Level,
            message.User.StageUser.Context.Options.ChannelID,
            message.User.Field?.ID ?? 999999999
        ));
        
        if (response.Result == PartyResult.Success) return;
        
        var result = response.Result switch
        {
            PartyResult.FailedAlreadyInParty => PartyResultOperations.CreateNewPartyAlreayJoined,
            _ => PartyResultOperations.CreateNewPartyUnknown
        };
        using var packet = new PacketWriter(PacketSendOperations.PartyResult);
        packet.WriteByte((byte)result);
        await message.User.Dispatch(packet.Build());
    }
}
