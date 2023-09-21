﻿using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Social;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Services.Social.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class FieldOnPacketPartyChangeLeaderRequestPlug : IPipelinePlug<FieldOnPacketPartyChangeLeaderRequest>
{
    public async Task Handle(IPipelineContext ctx, FieldOnPacketPartyChangeLeaderRequest message)
    {
        if (message.User.StageUser.Party == null) return;
        
        var response = await message.User.StageUser.Context.Services.Party.ChangeBoss(new PartyChangeBossRequest(
            message.User.Character.ID,
            message.User.StageUser.Party.ID,
            message.CharacterID,
            false
        ));
        
        if (response.Result == PartyResult.Success) return;
        
        var result = response.Result switch
        {
            _ => PartyResultOperations.ChangePartyBossUnknown
        };
        var p = new PacketWriter(PacketSendOperations.PartyResult);
        p.WriteByte((byte)result);
        await message.User.Dispatch(p.Build());
    }
}
