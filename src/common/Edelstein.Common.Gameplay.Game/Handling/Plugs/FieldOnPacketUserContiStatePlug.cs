using Edelstein.Common.Gameplay.Handling;
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
        var p = new PacketWriter(PacketSendOperations.CONTISTATE);

        p.WriteByte((byte)contimove.State);
        p.WriteBool(contimove.State == ContiMoveState.Event);

        await message.User.Dispatch(p.Build());
    }
}
