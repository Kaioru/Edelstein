using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Social;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class NotifyPartyCreatedPlug : IPipelinePlug<NotifyPartyCreated>
{
    private readonly IGameStage _stage;

    public NotifyPartyCreatedPlug(IGameStage stage) => _stage = stage;
    
    public async Task Handle(IPipelineContext ctx, NotifyPartyCreated message)
    {
        var user = await _stage.Users.Retrieve(message.CharacterID);
        if (user == null) return;
        
        user.Party = message.Party;
        
        var p = new PacketWriter(PacketSendOperations.PartyResult);
        p.WriteByte((byte)PartyResultOperations.CreateNewPartyDone);
        p.WriteInt(message.Party.ID);
        
        p.WriteInt(0);
        p.WriteInt(0);
        p.WriteInt(0);
        
        p.WriteShort(0);
        p.WriteShort(0);
        _ = user.Dispatch(p.Build());
    }
}
