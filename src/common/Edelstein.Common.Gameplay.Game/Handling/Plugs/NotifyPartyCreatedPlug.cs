using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Social;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

public class NotifyPartyCreatedPlug : IPipelinePlug<NotifyPartyCreated>
{
    private readonly IGameStage _stage;

    public NotifyPartyCreatedPlug(IGameStage stage) => _stage = stage;
    
    public async Task Handle(IPipelineContext ctx, NotifyPartyCreated message)
    {
        var user = await _stage.Users.Retrieve(message.CharacterID);
        if (user == null) return;
        
        user.Party = message.Party;
        
        using var packet = new PacketWriter(PacketSendOperations.PartyResult);
        packet.WriteByte((byte)PartyResultOperations.CreateNewPartyDone);
        packet.WriteInt(message.Party.ID);
        
        packet.WriteInt(0);
        packet.WriteInt(0);
        packet.WriteInt(0);
        
        packet.WriteShort(0);
        packet.WriteShort(0);
        _ = user.Dispatch(packet.Build());
    }
}
