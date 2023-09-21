﻿using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Social;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Services.Social.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class FieldOnPacketPartyLeaveRequestPlug : IPipelinePlug<FieldOnPacketPartyLeaveRequest>
{
    public async Task Handle(IPipelineContext ctx, FieldOnPacketPartyLeaveRequest message)
    {
        if (message.User.StageUser.Party == null) return;
        
        var response = await message.User.StageUser.Context.Services.Party.Disband(new PartyDisbandRequest(
            message.User.Character.ID,
            message.User.StageUser.Party.ID
        ));
        
        if (response.Result == PartyResult.Success) return;
        
        var result = response.Result switch
        {
            _ => PartyResultOperations.WithdrawPartyUnknown
        };
        var p = new PacketWriter(PacketSendOperations.PartyResult);
        p.WriteByte((byte)result);
        await message.User.Dispatch(p.Build());
    }
}
