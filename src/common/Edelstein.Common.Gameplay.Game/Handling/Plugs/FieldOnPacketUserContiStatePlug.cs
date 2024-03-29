﻿using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Continents;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

public class FieldOnPacketUserContiStatePlug : IPipelinePlug<FieldOnPacketUserContiState>
{
    private readonly IContiMoveManager _manager;
    
    public FieldOnPacketUserContiStatePlug(IContiMoveManager manager) => _manager = manager;
    
    public async Task Handle(IPipelineContext ctx, FieldOnPacketUserContiState message)
    {
        if (message.User.Field == null) return;
        var contimove = await _manager.RetrieveByField(message.User.Field);
        if (contimove == null) return;
        using var packet = new PacketWriter(PacketSendOperations.CONTISTATE);

        packet.WriteByte((byte)contimove.State);
        packet.WriteBool(contimove.State == ContiMoveState.Event);

        await message.User.Dispatch(packet.Build());
    }
}
